using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class ON_BraunTubeRenderPass : ScriptableRenderPass
{
    private const string RenderPassName = nameof(ON_BraunTubeRenderPass);
    private const string ProfilingSamplerName = "ScrToDest";

    private readonly bool _applyToSceenView;
    private readonly int _mainTexPropertyId = Shader.PropertyToID("_MainTex");
    private readonly Material _material;
    private readonly ProfilingSampler _profilingSampler;

    private RenderTargetHandle _afterPostProcessTexture;
    private RenderTargetIdentifier _cameraColorTarget;
    private RenderTargetHandle _tempRenderTargetHandle;
    private ON_BraunTubeVolume _volume;

    public ON_BraunTubeRenderPass(bool applyToSceneView, Shader shader)
    {
        if(shader == null)
        {
            return;
        }
        _applyToSceenView = applyToSceneView;
        _profilingSampler = new ProfilingSampler(ProfilingSamplerName);
        _tempRenderTargetHandle.Init("_TempRT");

        // マテリアル作成
        _material = CoreUtils.CreateEngineMaterial(shader);

        // RenderPassEvent.AfterRenderingではポストエフェクトを掛けた後のカラーテクスチャがこの名前で取得出来る
        _afterPostProcessTexture.Init("_AfterPostProcessTexture");
    }

    public void Setup(RenderTargetIdentifier cameraColorTarget, PostprocessTiming timing)
    {
        _cameraColorTarget = cameraColorTarget;
        renderPassEvent = ON_TimeEffectRenderPass.GetRenderPassEvent(timing);

        // volumeコンポーネントを取得
        var volumeStack = VolumeManager.instance.stack;
        _volume = volumeStack.GetComponent<ON_BraunTubeVolume>();
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if(_material == null)
        {
            return;
        }

        // カメラのポストエフェクトが無効になっていたら何もしない
        if(!renderingData.cameraData.postProcessEnabled)
        {
            return;
        }

        // カメラがシーンビューカメラかつシーンビューへ適応しない場合には何もしない
        if(!_applyToSceenView && renderingData.cameraData.cameraType == CameraType.SceneView)
        {
            return;
        }
        if(!_volume.isActive())
        {
            return;
        }

        // renderingPassEventがAfterRenderingの場合、カメラのカラーターゲットではなく_AfterPostProcessTextureを使う
        var source = renderPassEvent == RenderPassEvent.AfterRendering && renderingData.cameraData.resolveFinalTarget
            ? _afterPostProcessTexture.Identifier() : _cameraColorTarget;

        // コマンドバッファを作成
        var cmd = CommandBufferPool.Get(RenderPassName);
        cmd.Clear();

        // Cameraのターゲットと同じDescription(Depthは無し)のRenderTextureを取得する
        var tempTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
        tempTargetDescriptor.depthBufferBits = 0;
        cmd.GetTemporaryRT(_tempRenderTargetHandle.id, tempTargetDescriptor);

        using (new ProfilingScope(cmd, _profilingSampler))
        {
            // volumeからプロパティを反映
            _material.SetFloat("_Rate", _volume.rate.value);
            cmd.SetGlobalTexture(_mainTexPropertyId, source);

            // 元のテクスチャから一時的なテクスチャへエフェクトを適応しつつ描画
            Blit(cmd, source, _tempRenderTargetHandle.Identifier(), _material);
        }

        // 一時的なテクスチャから元のテクスチャへ結果を描画
        Blit(cmd, _tempRenderTargetHandle.Identifier(), source);

        // 一時的なレンダーテクスチャを解放
        cmd.ReleaseTemporaryRT(_tempRenderTargetHandle.id);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}
