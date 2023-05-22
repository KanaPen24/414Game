﻿using UnityEngine;
using System;

public class YK_TargetCamera : MonoBehaviour
{
    [SerializeField]
    [Tooltip("追従させたいターゲット")]
    private IS_Player Player;

    [SerializeField] private Vector3 offset;
    [SerializeField] private float m_fMaxCameraMove;
    [SerializeField] private float m_fMoveCnt;
    private float m_fCameraMove;
    private float m_fCurrnetMoveCnt;
    private float m_fAccel;
    private const float m_fMaxAccel = 3.0f;
    private int nDir;
    [SerializeField] private NK_BossSlime_Aera m_Area;
    [SerializeField] private GameObject m_BattleCameraPos;
    private float Shakefloat = 0.0f;
    private Vector3 RendaPos;
    private bool m_OneFlag;
    // Start is called before the first frame update
    void Start()
    {
        m_fCameraMove = 0;
        m_fCurrnetMoveCnt = 0;
        m_fAccel = m_fMaxAccel;
    }

    /// <summary>
    /// プレイヤーが移動した後にカメラが移動するようにするためにLateUpdateにする。
    /// </summary>
    void FixedUpdate()
    {
        RendaPos = this.gameObject.transform.position;

        //連打の振動
        Shakefloat = Shakefloat * 0.99f - Shakefloat * 0.01f;

        RendaPos.x = m_BattleCameraPos.transform.position.x + UnityEngine.Random.Range(-Shakefloat, Shakefloat);
        RendaPos.y = m_BattleCameraPos.transform.position.y + UnityEngine.Random.Range(-Shakefloat, Shakefloat);

        this.gameObject.transform.position = RendaPos;
        if (m_Area.GetSetBattleFlag)
        {
            if (!m_OneFlag)
            {
                m_OneFlag = true;
                gameObject.transform.position = m_BattleCameraPos.transform.position;
            }
        }
        else
        {
            // カメラの位置をターゲットの位置にオフセットを足した場所にする。
            gameObject.transform.position = new Vector3(Player.transform.position.x, 0f, Player.transform.position.z) + offset +
                new Vector3(m_fCameraMove, 0f, 0f);
        }
    }

    public void GetShakeFloat(float Shake)
    {
        Shakefloat = Shake;
    }
}