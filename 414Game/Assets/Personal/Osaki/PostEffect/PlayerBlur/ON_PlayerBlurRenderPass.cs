using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class ON_PlayerBlurRenderPass : ScriptableRenderPass
{
    private const string RenderPassName = nameof(ON_PlayerBlurRenderPass);
    private const string ProfilingSamplerName = "ScrToDest";

    private readonly bool _applyToSceenView;
    private readonly int _mainTexPropertyId = Shader.PropertyToID("_MainTex");
    private readonly int _blurTex1PropertyId = Shader.PropertyToID("_BlurTex1");
    private readonly int _blurTex2PropertyId = Shader.PropertyToID("_BlurTex2");
    private readonly int _blurTex3PropertyId = Shader.PropertyToID("_BlurTex3");
    private readonly Material _material;
    private readonly ProfilingSampler _profilingSampler;

    private RenderTargetHandle _afterPostProcessTexture;
    private RenderTargetIdentifier _cameraColorTarget;
    private RenderTargetHandle _tempRenderTargetHandle;
    private ON_PlayerBlurVolume _volume;

    private RenderTexture BlurTexture;  // ブラー用テクスチャ
    private RenderTexture[] m_rtList;   // フレームをずらしたテクスチャの配列

    public ON_PlayerBlurRenderPass(bool applyToSceneView, Shader shader)
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
        _volume = volumeStack.GetComponent<ON_PlayerBlurVolume>();
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

        // ブラー用設定
        BlurTexture = _volume.BlurTexture;
        // ブラー用フレームをずらしたテクスチャを作成
        int width = Screen.width;
        int height = Screen.height;
        m_rtList = new[] {
                new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32),
                new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32),
                new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32)
            };

        // ブラー用のテクスチャに描画
        var temp = m_rtList[Time.frameCount % m_rtList.Length];
        Blit(cmd, BlurTexture, temp);

        using (new ProfilingScope(cmd, _profilingSampler))
        {
            // プロパティを設定
            _material.SetTexture(_blurTex1PropertyId, m_rtList[0]);
            _material.SetTexture(_blurTex2PropertyId, m_rtList[1]);
            _material.SetTexture(_blurTex3PropertyId, m_rtList[2]);

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
