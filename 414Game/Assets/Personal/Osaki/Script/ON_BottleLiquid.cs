/**
 * @file ON_BottleLiquid.cs
 * @brief 液体がモデルに入ってるようにマテリアルを変更するスクリプト
 * @author Osaki Noriaki
 * @date 2023/03/06
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Renderer))]
public class ON_BottleLiquid : MonoBehaviour
{
    // ON_LiquidシェーダーのプロパティID定義
    private static class ShaderPropertyId
    {
        public static readonly int BottleLiquidWaveCenter = Shader.PropertyToID("_WaveCenter");
        public static readonly int BottleLiquidWaveParams = Shader.PropertyToID("_WaveParams");
        public static readonly int BottleLiquidColorForward = Shader.PropertyToID("_LiquidColorForward");
        public static readonly int BottleLiquidColorBack = Shader.PropertyToID("_LiquidColorBack");
    }

    private static readonly Color LiquidColorTopOffset = new Color(0.15f, 0.15f, 0.15f, 0.0f);  // 液面カラーオフセット
    [SerializeField] private Color liquidColor = new Color(0.5f, 0.6f, 0.9f, 1.0f); // 液体カラー
    [SerializeField] private Vector3[] bottleSizeOffsetPoints;  // 瓶形状の概要を表すオフセットポイントリスト
    [Range(0.0f, 1.0f)] [SerializeField] private float fillingRate = 0.5f;  // 充填率
    [Range(0.0f, 2.0f)] [SerializeField] private float positionInfluenceRate = 0.7f;    // 位置差分における動きの影響率
    [Range(0.0f, 2.0f)] [SerializeField] private float rotationInfluenceRate = 0.4f;    // 回転差分における動きの影響率
    [Range(0.0f, 1.0f)] [SerializeField] private float sizeAttenuationRate = 0.92f;     // 波の大きさの減衰率
    [Range(0.0f, 1.0f)] [SerializeField] private float cycleAttenuationRate = 0.97f;    // 波の周期の減衰率 
    [SerializeField] private float cycleOffsetCoef = 12.0f; // 時間による位相変化係数
    [SerializeField] private float deltaSizeMax = 0.15f;    // 差分による変化量最大(波の大きさ)
    [SerializeField] private float deltaCycleMax = 10.0f;   // 差分による変化量最大(波の周期)
    private Material[] targetMaterials; // 制御対象のマテリアル
    private Vector3 prevPosition;   // 前回参照位置
    private Vector3 prevEulerAngle; // 前回参照オイラー角
    private Vector4 waveCurrentParams;  // 現在の液体波パラメータ

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        Renderer targetRenderer = GetComponent<Renderer>();
        if (targetRenderer == null)
            return;

        if(targetMaterials == null || targetMaterials.Length <=0)
        {
            List<Material> targetMaterialList = new List<Material>();
            for(int i = 0; i < targetRenderer.sharedMaterials.Length; ++i)
            {
                Material material = targetRenderer.sharedMaterials[i];
                if (material.shader.name.Contains("ON_Liquid"))
                {
                    targetMaterialList.Add(material);
                }
            }
            targetMaterials = targetMaterialList.ToArray();
        }
        waveCurrentParams = Vector4.zero;
        BackupTransform();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetMaterials == null || targetMaterials.Length <= 0)
            return;

        CalculateWaveParams();
        SetupMaterials();
        BackupTransform();
    }

    // 波パラメータ算出
    private void CalculateWaveParams()
    {
        // waveParamsにそのままベクトル演算
        // x:振幅, y:周期
        Vector4 attenuationRateVec = new Vector4(sizeAttenuationRate, cycleAttenuationRate, 0.0f, 0.0f);
        Vector4 deltaMaxVec = new Vector4(deltaSizeMax, deltaCycleMax, 0.0f, 0.0f);

        // 減衰処理
        waveCurrentParams = Vector4.Scale(waveCurrentParams, attenuationRateVec);

        // 位置と回転の差分から変化値算出
        Transform thisTransform = transform;
        Vector3 currentRotation = thisTransform.eulerAngles;
        Vector3 diffPos = thisTransform.position - prevPosition;
        Vector3 diffRot = new Vector3(Mathf.DeltaAngle(currentRotation.x, prevEulerAngle.x),
                                      Mathf.DeltaAngle(currentRotation.y, prevEulerAngle.y),
                                      Mathf.DeltaAngle(currentRotation.z, prevEulerAngle.z));

        waveCurrentParams += deltaMaxVec * (diffPos.magnitude * positionInfluenceRate);
        waveCurrentParams += deltaMaxVec * (diffRot.magnitude * rotationInfluenceRate);

        waveCurrentParams = Vector4.Min(waveCurrentParams, deltaMaxVec);

        // 時間による位相変化は減少しない
        waveCurrentParams.z = cycleOffsetCoef;
    }

    // 波の中心位置算出
    private Vector4 CalculateWaveCenter()
    {
        (float min, float max) liquidSurfaceHeight = GetLiquidSurfaceHeight();
        return transform.position + Vector3.up * Mathf.Lerp(liquidSurfaceHeight.min, liquidSurfaceHeight.max, fillingRate);
    }

    // マテリアル設定
    private void SetupMaterials()
    {
        Vector4 waveCenter = CalculateWaveCenter();

        for(int i = 0; i < targetMaterials.Length; ++i)
        {
            Material material = targetMaterials[i];
            material.SetVector(ShaderPropertyId.BottleLiquidWaveCenter, waveCenter);
            material.SetVector(ShaderPropertyId.BottleLiquidWaveParams, waveCurrentParams);
            material.SetVector(ShaderPropertyId.BottleLiquidColorForward, liquidColor);
            material.SetVector(ShaderPropertyId.BottleLiquidColorBack, liquidColor + LiquidColorTopOffset);
        }
    }

    // 姿勢情報の保存
    private void BackupTransform()
    {
        prevPosition = transform.position;
        prevEulerAngle = transform.eulerAngles;
    }

    // オブジェクトローカルにおける液面の高さ(最小/最大)を取得
    private (float min, float max)GetLiquidSurfaceHeight()
    {
        if (bottleSizeOffsetPoints == null || bottleSizeOffsetPoints.Length <= 0)
            return (0.0f, 0.0f);

        Transform thisTransform = transform;
        (float min, float max) ret = (float.MaxValue, float.MinValue);
        for(int i = 0; i < bottleSizeOffsetPoints.Length; ++i)
        {
            Vector3 localPoint = thisTransform.TransformPoint(bottleSizeOffsetPoints[i] - thisTransform.position);
            ret.min = Mathf.Min(ret.min, localPoint.y);
            ret.max = Mathf.Max(ret.max, localPoint.y);
        }

        return ret;
    }

#if UNITY_EDITOR
    // 選択時のギズモ表示
    private void OnDrawGizmosSelected()
    {
        if(bottleSizeOffsetPoints == null || bottleSizeOffsetPoints.Length <= 0)
            return;

        // 瓶形状のオフセットポイントの表示
        Gizmos.color = Color.yellow;
        for(int i = 0; i < bottleSizeOffsetPoints.Length; ++i)
        {
            Vector3 point = bottleSizeOffsetPoints[i];
            Gizmos.DrawSphere(transform.TransformPoint(point), 0.05f);
        }
    }
#endif
}
