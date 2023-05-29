using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFX_Drain : MonoBehaviour
{
    [SerializeField] private Transform[] ControlPoints; // 制御点
    [SerializeField] private float Length1;             // 第一制御点の縦幅
    [SerializeField] private float Length2;             // 第二制御点の縦幅 
    [SerializeField] private Transform StandardPoint;     // 第二制御点の基準点
    [SerializeField] private VisualEffect DrainEffect; // 吸収エフェクト

    private void Update()
    {
        SetEndPos(StandardPoint.transform.position);
    }
    // 始点の指定
    public void SetStartPos(Vector3 pos)
    {
        ControlPoints[0].position = pos;
        Vector3 Fulcrum = ControlPoints[0].position;
        Fulcrum.y += Length1;
        ControlPoints[1].position = Fulcrum;

        Fulcrum = ControlPoints[0].position;
        Fulcrum.y -= Length1;
        ControlPoints[3].position = Fulcrum;
    }

    // 終点の指定
    public void SetEndPos(Vector3 pos)
    {
        ControlPoints[ControlPoints.Length - 1].position = pos;
        Vector3 Fulcrum = StandardPoint.position;
        Fulcrum.y += Length2;
        ControlPoints[2].position = Fulcrum;

        Fulcrum = StandardPoint.position;
        Fulcrum.y -= Length2;
        ControlPoints[4].position = Fulcrum;
    }

    public VisualEffect GetVisualEffect()
    {
        return DrainEffect;
    }
}
