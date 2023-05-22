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
    [SerializeField] private IS_Player m_Player; // IS_Playerをアタッチする
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Playerの地面判定
    public PlayerAnimState m_PlayerAnimState;

    private void Update()
    {
        if (m_Player.GetSetPlayerState == PlayerState.PlayerAttack01)
        {
            // 攻撃開始時の処理
            if (m_Player.GetSetAttackFlg)
            {
                m_Player.GetSetAttackFlg = false;
                m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).StartAttack();

                if (m_Player.GetSetPlayerEquipState == PlayerEquipState.Equip)
                {
                    switch (m_Player.GetSetEquipWeaponState)
                    {
                        case EquipWeaponState.PlayerHpBar:
                            m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.AttackHPBar);
                            m_PlayerAnimState = PlayerAnimState.AttackHPBar;
                            break;
                        case EquipWeaponState.PlayerSkillIcon:
                            m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.AttackSkillIcon);
                            m_PlayerAnimState = PlayerAnimState.AttackSkillIcon;
                            break;
                        case EquipWeaponState.PlayerBossBar:
                            m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.AttackBossBar);
                            m_PlayerAnimState = PlayerAnimState.AttackBossBar;
                            break;
                        case EquipWeaponState.PlayerClock:
                            m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.AttackClock);
                            m_PlayerAnimState = PlayerAnimState.AttackClock;
                            break;
                    }
                }
            }

            // =========
            // 状態遷移
            // =========
            // 「攻撃01 → 待機」
            if (m_Player.GetPlayerAnimator().AnimEnd(m_PlayerAnimState) &&
                !m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).GetSetAttack)
            {
                m_Player.GetSetPlayerState = PlayerState.PlayerWait;
                return;
            }
            // 「攻撃01 → 落下」
            if (!m_PlayerGroundColl.IsGroundCollision())
            {
                m_Player.GetSetPlayerState = PlayerState.PlayerDrop;
                m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).FinAttack();
                return;
            }
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
        m_Player.GetSetMoveAmount =
            new Vector3(0f, 0f, 0f);

        // 指定した武器で攻撃処理
        m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).UpdateAttack();
    }

    /**
     * @fn
     * アニメーション更新処理
     * @brief Playerのアニメーション更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateAnim()
    {
        if (m_Player.GetSetPlayerEquipState == PlayerEquipState.Equip)
        {
            switch (m_Player.GetSetEquipWeaponState)
            {
                case EquipWeaponState.PlayerHpBar:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.AttackHPBar);
                    break;
                case EquipWeaponState.PlayerSkillIcon:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.AttackSkillIcon);
                    break;
                case EquipWeaponState.PlayerBossBar:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.AttackBossBar);
                    break;
                case EquipWeaponState.PlayerClock:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.AttackClock);
                    break;
            }
        }
    }
}
