/**
 * @file   IS_PlayerFlg.cs
 * @brief  Playerのクラス
 * @author IharaShota
 * @date   2023/06/12
 * @Update 2023/06/12 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerFlg : MonoBehaviour
{
    public bool m_bStartWalkFlg;       // 歩行開始フラグ
    public bool m_bStartJumpFlg;       // 跳躍開始フラグ
    public bool m_bStartAttackFlg;     // 攻撃開始フラグ
    public bool m_bStartJumpAttackFlg; // 跳躍攻撃開始フラグ
    public bool m_bStartChargeWaitFlg; // 溜め待機開始フラグ
    public bool m_bStartChargeWalkFlg; // 溜め移動開始フラグ
    public bool m_bStartAvoidFlg;      // 回避開始フラグ
    public bool m_bStartUICatchFlg;    // UI取得開始フラグ
    public bool m_bStartUIReleaseFlg;  // UI解放開始フラグ
    public bool m_bStartGameOverFlg;   // ゲームオーバーフラグ

    public bool m_bAttack;
    public bool m_bCharge;

    // Start is called before the first frame update
    void Start()
    {
        m_bStartWalkFlg = false;
        m_bStartJumpFlg = false;
        m_bStartAttackFlg = false;
        m_bStartJumpAttackFlg = false;
        m_bStartChargeWaitFlg = false;
        m_bStartChargeWalkFlg = false;
        m_bStartAvoidFlg = false;
        m_bStartUICatchFlg = false;
        m_bStartUIReleaseFlg = false;
        m_bStartGameOverFlg = false;

        m_bAttack = false;
        m_bCharge = false;
    }
}
