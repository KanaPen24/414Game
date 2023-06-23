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
    public bool m_bWalkFlg;           // 歩行開始フラグ
    public bool m_bJumpFlg;           // 跳躍開始フラグ
    public bool m_bAttackFlg;         // 攻撃開始フラグ
    public bool m_bJumpAttackFlg;     // 跳躍攻撃開始フラグ
    public bool m_bChargeWaitFlg;     // 溜め待機開始フラグ
    public bool m_bChargeWalkFlg;     // 溜め移動開始フラグ
    public bool m_bAvoidFlg;          // 回避開始フラグ
    public bool m_bUICatchFlg;        // UI取得開始フラグ
    public bool m_bUIReleaseFlg;      // UI解放開始フラグ

    // Start is called before the first frame update
    void Start()
    {
        m_bWalkFlg = false;
        m_bJumpFlg = false;
        m_bAttackFlg = false;
        m_bJumpAttackFlg = false;
        m_bChargeWaitFlg = false;
        m_bChargeWalkFlg = false;
        m_bAvoidFlg = false;
        m_bUICatchFlg = false;
        m_bUIReleaseFlg = false;
    }
}
