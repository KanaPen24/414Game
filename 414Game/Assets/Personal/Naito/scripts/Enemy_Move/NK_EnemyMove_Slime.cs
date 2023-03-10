// ==============================================================
// NK_EnemyMove_Slime.cs
// Auther:Naito
// Update:2023/03/06 cs作成
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_EnemyMove_Slime : MonoBehaviour
{
    //フラグ管理
    //private bool m_bMoveFlag;           //移動フラグ
    //private bool m_bPosRight;           //プレイヤーより右にいるか左にいるか。trueだったら右
    //リジットボディ
    private Rigidbody m_rRdoby;
    //横移動
    [SerializeField] private float m_fMovePower;
    //ジャンプ力
    [SerializeField] private float m_fJumpPower;

    private void Start()
    {
        m_rRdoby = GetComponent<Rigidbody>();
    }

    void Update()
    {
    }

    public void MoveFlagChanger(bool PosRight)
    {
        if (PosRight)
        {
            //斜め飛び
            this.m_rRdoby.AddForce(transform.up * m_fJumpPower, ForceMode.Impulse);
            this.m_rRdoby.AddForce(transform.right * m_fMovePower, ForceMode.Impulse);
        }
        else
        {
            //斜め飛び
            this.m_rRdoby.AddForce(transform.up * m_fJumpPower, ForceMode.Impulse);
            this.m_rRdoby.AddForce(transform.right * -m_fMovePower, ForceMode.Impulse);
        }
    }

    public void KnockBack(bool PosRight)
    {
        if (PosRight)
        {
            //ノックバック
            this.m_rRdoby.AddForce(transform.up * m_fJumpPower, ForceMode.Impulse);
            this.m_rRdoby.AddForce(transform.right * -m_fMovePower, ForceMode.Impulse);
        }
        else
        {
            //ノックバック
            this.m_rRdoby.AddForce(transform.up * m_fJumpPower, ForceMode.Impulse);
            this.m_rRdoby.AddForce(transform.right * m_fMovePower, ForceMode.Impulse);
        }
    }
}


