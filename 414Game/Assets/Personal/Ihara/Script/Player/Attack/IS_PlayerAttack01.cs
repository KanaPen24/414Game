/**
 * @file   IS_PlayerAttack01.cs
 * @brief  Playerの攻撃01クラス
 * @author IharaShota
 * @date   2023/03/10
 * @Update 2023/03/10 作成
 * @Update 2023/03/12 アニメーション処理追加
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerAttack01 : IS_PlayerStrategy
{
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Playerの地面判定
    [SerializeField] private float m_fMax2NextAttackTime; // 次の攻撃に移れる最大時間
    private PlayerAnimState m_CurrentPlayerAnimState;
    private float f2NextAttackTime;

    private void Update()
    {
        if (IS_Player.instance.GetSetPlayerState == PlayerState.PlayerAttack01)
        {
            // 攻撃開始時の処理
            if (IS_Player.instance.GetFlg().m_bStartAttackFlg)
            {
                IS_Player.instance.GetFlg().m_bStartAttackFlg = false;
                IS_Player.instance.GetWeapons((int)IS_Player.instance.GetSetEquipState).StartAttack();
                f2NextAttackTime = 0.0f;
            }

            // =========
            // 状態遷移
            // =========
            // 「攻撃01 → 落下」
            if (!m_PlayerGroundColl.IsGroundCollision())
            {
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerDrop;
                IS_Player.instance.GetWeapons((int)IS_Player.instance.GetSetEquipState).FinAttack();
                return;
            }
            // 「攻撃01 → 攻撃02」
            if (f2NextAttackTime <= m_fMax2NextAttackTime &&
                Input.GetKeyDown(IS_XBoxInput.X))
            {
                if (IS_Player.instance.GetSetEquipState == EquipState.EquipHpBar ||
                    IS_Player.instance.GetSetEquipState == EquipState.EquipStart)
                {
                    IS_Player.instance.GetSetPlayerState = PlayerState.PlayerAttack02;
                    IS_Player.instance.GetWeapons((int)IS_Player.instance.GetSetEquipState).FinAttack();
                    IS_Player.instance.GetFlg().m_bStartAttackFlg = true;
                    return;
                }
            }
            // 「攻撃01 → 移動」
            if (!IS_Player.instance.GetFlg().m_bAttack &&
                (IS_XBoxInput.LStick_H >= 0.2 || IS_XBoxInput.LStick_H <= -0.2))
            {
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerWait;
                return;
            }
            // 「攻撃01 → 待機」
            if (f2NextAttackTime >= m_fMax2NextAttackTime)
            {
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerWait;
                return;
            }

            if (!IS_Player.instance.GetFlg().m_bAttack)
                f2NextAttackTime += Time.deltaTime;
        }
    }
    /**
     * @fn
     * 更新処理
     * @brief  Playerの攻撃更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateStrategy()
    {
        // アニメーション更新
        UpdateAnim();

        // 合計移動量をリセット
        IS_Player.instance.GetSetMoveAmount =
            new Vector3(0f, 0f, 0f);

        // 指定した武器で攻撃処理
        IS_Player.instance.GetWeapons((int)IS_Player.instance.GetSetEquipState).UpdateAttack();
    }

    /**
     * @fn
     * アニメーション更新処理
     * @brief Playerのアニメーション更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateAnim()
    {
        switch (IS_Player.instance.GetSetEquipState)
        {
            case EquipState.EquipHpBar:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.Attack01HPBar);
                break;
            case EquipState.EquipSkillIcon:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.AttackSkillIcon);
                break;
            case EquipState.EquipBossBar:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.AttackBossBar);
                break;
            case EquipState.EquipClock:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.AttackClock);
                break;
            case EquipState.EquipStart:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.Attack01HPBar);
                break;
        }
    }
}
