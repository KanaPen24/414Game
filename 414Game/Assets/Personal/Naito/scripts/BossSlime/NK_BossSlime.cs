/**
 * @file   NK_BossSlime.cs
 * @brief  BossSlimeのクラス
 * @author NaitoKoki
 * @date   2023/04/04
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================================
// BossSlimeState
// … BossSlimeの状態を管理する列挙体
// ※m_PlayerStateはこの順番になるように入れること
// ===============================================
public enum BossSlimeState
{
    //BossSlimeWait,   // 待ち状態
    BossSlimeWalk,   // 移動状態
    //BossSlimeJump,   // 跳躍状態
    //BossSlimeDrop,   // 落下状態
    BossSlimeAttack, // 攻撃状態

    MaxBossSlimeState
}

// ===============================================
// BossSlimeDir
// … BossSlimeの向きを管理する列挙体
// ===============================================
public enum BossSlimeDir
{
    Left, // 左向き
    Right,// 右向き

    MaxDir
}

public class NK_BossSlime : MonoBehaviour
{
    [SerializeField] private List<NK_BossSlimeStrategy> m_BossSlimeStrategy; // BossSlime挙動クラスの動的配列
    [SerializeField] private BossSlimeState m_BossSlimeState;      // BossSlimeの状態を管理する
    [SerializeField] private BossSlimeDir m_BossSlimeDir;        // BossSlimeの向きを管理する

    private void FixedUpdate()
    {
        m_BossSlimeStrategy[(int)m_BossSlimeState].UpdateStrategy();
    }


    /**
 * @fn
 * BossSlimeの状態のgetter・setter
 * @return m_BossSlimeState
 * @brief BossSlimeの状態を返す・セット
 */
    public BossSlimeState GetSetBossSlimeState
    {
        get { return m_BossSlimeState; }
        set { m_BossSlimeState = value; }
    }

    /**
     * @fn
     * BossSlimeの向きのgetter・setter
     * @return m_BossSlimeDir
     * @brief BossSlimeの向きを返す・セット
     */
    public BossSlimeDir GetSetBossSlimeDir
    {
        get { return m_BossSlimeDir; }
        set { m_BossSlimeDir = value; }
    }
}
