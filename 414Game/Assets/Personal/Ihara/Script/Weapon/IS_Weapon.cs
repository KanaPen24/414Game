/**
 * @file   IS_Weapon.cs
 * @brief  武器のクラス
 * @author IharaShota
 * @date   2023/03/12
 * @Update 2023/03/12 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_Weapon : MonoBehaviour
{
    private bool m_bAttack; // 攻撃中かどうか

    /**
     * @fn
     * 攻撃処理(override前提)
     * @brief 攻撃処理
     */
    public virtual void Attack()
    {
        // ここに処理を加える
        Debug.Log("宇部井");
    }


    /**
     * @fn
     * 攻撃中のgetter・setter
     * @return m_bAttack(bool)
     * @brief 攻撃中を返す・セット
     */
    public bool GetSetAttack
    {
        get { return m_bAttack; }
        set { m_bAttack = value; }
    }
}
