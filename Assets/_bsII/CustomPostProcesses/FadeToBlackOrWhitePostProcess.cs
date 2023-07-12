using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Post-processing/Custom/FadeToBlackOrWhitePostProcess")]
public sealed class FadeToBlackOrWhitePostProcess : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    // default stuff:
    [Tooltip("Controls the intensity of the effect.")]
    public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

    // custom stuff:
    public ClampedFloatParameter lightenIntensity = new ClampedFloatParameter(0f, 0f, 1f);
    public ClampedFloatParameter darkenIntensity = new ClampedFloatParameter(0f, 0f, 1f);

    // deefault stuff:
    Material m_Material;

    public bool IsActive() => m_Material != null && intensity.value > 0f;

    // Do not forget to add this post process in the Custom Post Process Orders list (Project Settings > Graphics > HDRP Global Settings).
    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    // custom stuff:
    const string kShaderName = "Shader Graphs/FadeToBlackOrWhite";

    public override void Setup()
    {
        if (Shader.Find(kShaderName) != null)
            m_Material = new Material(Shader.Find(kShaderName));
        else
            Debug.LogError($"Unable to find shader '{kShaderName}'. Post Process Volume FadeToBlackOrWhite is unable to load.");
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (m_Material == null)
            return;

        // default stuff:
        m_Material.SetFloat("_Intensity", intensity.value);
        m_Material.SetTexture("_MainTex", source);

        // custom stuff:
        m_Material.SetFloat("_whitenIntensity", lightenIntensity.value);
        m_Material.SetFloat("_darkenIntensity", darkenIntensity.value);

        // default stuff:
        HDUtils.DrawFullScreen(cmd, m_Material, destination, shaderPassId: 0);
    }

    public override void Cleanup()
    {
        CoreUtils.Destroy(m_Material);
    }
}
