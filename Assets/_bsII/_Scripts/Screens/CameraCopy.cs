using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Post-processing/Custom/CameraCopy")]
public sealed class CameraCopy : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    [Tooltip("Controls the intensity of the effect.")]
    public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

    public RenderTexture RenderTexture;

    Material m_Material;
    private GameObject _renderTextureQuad;
    private Material _renderTexQuadSharedMaterial;
    private bool _copyToRenderTexture = false;

    public bool IsActive() => m_Material != null && intensity.value > 0f;

    // Do not forget to add this post process in the Custom Post Process Orders list (Project Settings > Graphics > HDRP Global Settings).
    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    const string kShaderName = "Hidden/Shader/PassThrough";

    public override void Setup()
    {
        if (Shader.Find(kShaderName) != null)
            m_Material = new Material(Shader.Find(kShaderName));
        else
            Debug.LogError($"Unable to find shader '{kShaderName}'. Post Process Volume CameraCopy is unable to load.");

        _renderTextureQuad = GameObject.FindGameObjectWithTag("MainOutputRendertexture");
        if (_renderTextureQuad != null)
        {
            _copyToRenderTexture = true;
            SetUpRenderTexture();
            
            _renderTextureQuad.GetComponent<Renderer>().sharedMaterial.SetTexture("_UnlitColorMap", RenderTexture);
        }
        else
        {
            Debug.Log("Quad is null");
        }
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {

        if (m_Material == null)
            return;

        m_Material.SetFloat("_Intensity", intensity.value);
        m_Material.SetTexture("_MainTex", source);

        if (_renderTextureQuad == null)
        {
            _renderTextureQuad = GameObject.FindGameObjectWithTag("MainOutputRendertexture");
            if (_renderTextureQuad == null)
            {
                Debug.Log("_renderTextureQuad still null");
                return;
            }
            _copyToRenderTexture = true;

            SetUpRenderTexture();

            if (_renderTexQuadSharedMaterial == null)
            {
                _renderTexQuadSharedMaterial = _renderTextureQuad.GetComponent<Renderer>().sharedMaterial;
            }
            _renderTexQuadSharedMaterial.SetTexture("_UnlitColorMap", RenderTexture);
        }
        if (_copyToRenderTexture && camera.camera.tag == "MainCamera")
        {
            cmd.Blit(source, RenderTexture, m_Material);
        }

        HDUtils.DrawFullScreen(cmd, m_Material, destination, shaderPassId: 0);
    }

    private void SetUpRenderTexture()
    {
        if (RenderTexture == null)
        {
            RenderTexture = _renderTextureQuad.GetComponent<RenderTextureHolder>().RenderTexture;
        }
    }

    public override void Cleanup()
    {
        CoreUtils.Destroy(m_Material);
        RenderTexture?.Release();
    }
}
