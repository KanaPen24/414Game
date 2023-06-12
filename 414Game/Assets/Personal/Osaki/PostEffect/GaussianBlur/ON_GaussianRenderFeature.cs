using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ON_GaussianRenderFeature : ScriptableRendererFeature
{
    [SerializeField] private Shader _shader;
    [SerializeField] private PostprocessTiming _timing = PostprocessTiming.BeforePostprocess;
    [SerializeField] private bool _applyToSceneView = true;

    private ON_GaussianRenderPass _postProcessPass;

    public override void Create()
    {
        _postProcessPass = new ON_GaussianRenderPass(_applyToSceneView, _shader);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        _postProcessPass.Setup(renderer.cameraColorTarget, _timing);
        renderer.EnqueuePass(_postProcessPass);
    }
}
