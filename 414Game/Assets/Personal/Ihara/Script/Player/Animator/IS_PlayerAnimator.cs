/**
 * @file   IS_PlayerAnim.cs
 * @brief  Playerのアニメーション管理クラス
 * @author IharaShota
 * @date   2023/05/19
 * @Update 2023/05/19 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================================
// PlayerAnimState
// … Playerのアニメーション状態を管理する列挙体
// ※Animatorのintの数があっている確認すること
// ===============================================
public enum PlayerAnimState
{
    Wait,           // 待ち状態
    WaitHPBar,      // 待ち状態(HPBar)
    WaitSkillIcon,  // 待ち状態(SkillIcon)
    WaitBossBar,    // 待ち状態(BossBar)
    WaitClock,      // 待ち状態(Clock)

    Walk,           // 移動状態
    WalkHPBar,      // 移動状態(HPBar)
    WalkSkillIcon,  // 移動状態(SkillIcon)
    WalkBossBar,    // 移動状態(BossBar)
    WalkClock,      // 移動状態(Clock)

    Jump,           // 跳躍状態
    JumpHPBar,      // 跳躍状態(HPBar)
    JumpSkillIcon,  // 跳躍状態(SkillIcon)
    JumpBossBar,    // 跳躍状態(BossBar)
    JumpClock,      // 跳躍状態(Clock)

    Drop,           // 落下状態
    DropHPBar,      // 落下状態(HPBar)
    DropSkillIcon,  // 落下状態(SkillIcon)
    DropBossBar,    // 落下状態(BossBar)
    DropClock,      // 落下状態(Clock)

    AttackHPBar,    // 攻撃状態(HPBar)
    AttackSkillIcon,// 攻撃状態(SkillIcon)
    AttackBossBar,  // 攻撃状態(BossBar)
    AttackClock,    // 攻撃状態(Clock)

    ChargeWaitSkillIcon, // 溜め待機状態(SkillIcon)
    ChargeWalkSkillIcon, // 溜め移動状態(SkillIcon)

    Avoid,               // 回避状態

    GameOver,            // ゲームオーバー状態

    UICatch,             // UIキャッチ 

    MaxPlayerAnimState
}

public class IS_PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator m_animator; // Playerのアニメーション
    private const string s_intName = "motionNum";

    public void ChangeAnim(PlayerAnimState anim)
    {
        m_animator.SetInteger(s_intName, (int)anim);
    }

    public bool AnimEnd(PlayerAnimState anim)
    {
        if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f &&
            m_animator.GetCurrentAnimatorStateInfo(0).IsName(anim.ToString()))
            return true;
        else return false;
    }

    public bool GetAnimNormalizeTime(float time)
    {
        if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= time)
            return true;
        else return false;
    }
}
