using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;
using UnityEngine.UIElements;

[Serializable, VolumeComponentMenu("Post-processing/Custom/Scene1ExplosionFullScreenShaderGraph")]
public sealed class Scene1ExplosionFullScreenShaderGraph : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    [Tooltip("Controls the scale of the effect.")]
    public ClampedFloatParameter scale = new ClampedFloatParameter(0f, 0f, 1f);
    Material m_Material;
    private LocalKeyword isInvertActiveKeyword;
    public ClampedFloatParameter screenHeight = new ClampedFloatParameter(9f, 0f, 32f);

    public ClampedFloatParameter screenWidth = new ClampedFloatParameter(16f, 0f, 32f);
    public BoolParameter isInvertActive = new BoolParameter(false);

    public bool IsActive() => m_Material != null && scale.value > 0f;

    // Do not forget to add this post process in the Custom Post Process Orders list (Project Settings > Graphics > HDRP Global Settings).
    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    const string kShaderName = "Shader Graphs/ExplosionFullScreenShaderGraph";

    public override void Setup()
    {
        if (Shader.Find(kShaderName) != null)
        {
            m_Material = new Material(Shader.Find(kShaderName));
            isInvertActiveKeyword = new LocalKeyword(m_Material.shader, "_ISINVERTACTIVE_ON");
        }
        else
        {
            Debug.LogError($"Unable to find shader '{kShaderName}'. Post Process Volume Scene1ExplosionFullScreenShaderGraph is unable to load.");
        }
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (m_Material == null)
            return;

        m_Material.SetFloat("_Scale", scale.value);
        m_Material.SetFloat("_ScreenWidth", screenWidth.value);
        m_Material.SetFloat("_ScreenHeight", screenHeight.value);
        m_Material.SetTexture("_MainTex", source);
        m_Material.SetKeyword(isInvertActiveKeyword, isInvertActive.value);
        HDUtils.DrawFullScreen(cmd, m_Material, destination, shaderPassId: 0);
    }

    public override void Cleanup()
    {
        CoreUtils.Destroy(m_Material);
    }
}
