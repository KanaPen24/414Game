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
    [SerializeField] private float m_fMax2NextAttackTime; // 次の攻撃に移れる最大時間
    private PlayerAnimState m_CurrentPlayerAnimState;
    private float f2NextAttackTime;

    private void Update()
    {
        if (m_Player.GetSetPlayerState == PlayerState.PlayerAttack01)
        {
            // 攻撃開始時の処理
            if (m_Player.GetSetAttackFlg)
            {
                m_Player.GetSetAttackFlg = false;
                m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).StartAttack();
                f2NextAttackTime = 0.0f;
            }

            // =========
            // 状態遷移
            // =========
            // 「攻撃01 → 落下」
            if (!m_PlayerGroundColl.IsGroundCollision())
            {
                m_Player.GetSetPlayerState = PlayerState.PlayerDrop;
                m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).FinAttack();
                return;
            }
            // 「攻撃01 → 攻撃02」
            if (f2NextAttackTime <= m_fMax2NextAttackTime &&
                m_Player.bInputAttack)
            {
                if (m_Player.GetSetEquipWeaponState == EquipWeaponState.PlayerHpBar ||
                    m_Player.GetSetEquipWeaponState == EquipWeaponState.PlayerStart)
                {
                    m_Player.GetSetPlayerState = PlayerState.PlayerAttack02;
                    m_Player.GetSetAttackFlg = true;
                    return;
                }
            }
            // 「攻撃01 → 移動」
            if (!m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).GetSetAttack  &&
                (m_Player.bInputRight || m_Player.bInputLeft))
            {
                m_Player.GetSetPlayerState = PlayerState.PlayerWait;
                return;
            }
            // 「攻撃01 → 待機」
            if (f2NextAttackTime >= m_fMax2NextAttackTime)
            {
                m_Player.GetSetPlayerState = PlayerState.PlayerWait;
                return;
            }

            if(!m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).GetSetAttack)
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
        switch (m_Player.GetSetEquipWeaponState)
        {
            case EquipWeaponState.PlayerHpBar:
                m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.Attack01HPBar);
                m_CurrentPlayerAnimState = PlayerAnimState.Attack01HPBar;
                break;
            case EquipWeaponState.PlayerSkillIcon:
                m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.AttackSkillIcon);
                m_CurrentPlayerAnimState = PlayerAnimState.AttackSkillIcon;
                break;
            case EquipWeaponState.PlayerBossBar:
                m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.AttackBossBar);
                m_CurrentPlayerAnimState = PlayerAnimState.AttackBossBar;
                break;
            case EquipWeaponState.PlayerClock:
                m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.AttackClock);
                m_CurrentPlayerAnimState = PlayerAnimState.AttackClock;
                break;
            case EquipWeaponState.PlayerStart:
                m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.Attack01HPBar);
                m_CurrentPlayerAnimState = PlayerAnimState.Attack01HPBar;
                break;
        }
    }
}
