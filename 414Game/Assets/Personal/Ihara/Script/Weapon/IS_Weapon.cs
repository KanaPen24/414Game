/**
 * @file   IS_Weapon.cs
 * @brief  武器のクラス
 * @author IharaShota
 * @date   2023/03/12
 * @Update 2023/03/12 作成
 * @Update 2023/03/16 武器種類を判別する列挙体追加
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ================================================
// WeaponType
// … 武器種類の列挙体
// ================================================
public enum WeaponType
{
    HPBar,// HPバー
    Ball, // Ball

    MaxWeaponType
}

public class IS_Weapon : MonoBehaviour
{
    protected WeaponType m_eWeaponType; // 武器の種類
    protected bool m_bAttack;           // 攻撃中かどうか
    protected bool m_bVisible;          // 表示
    protected int  m_nHp;               // 耐久値

    /**
     * @fn
     * 攻撃初期化処理(override前提)
     * @brief 攻撃初期化処理
     */
    public virtual void StartAttack()
    {
        // ここに処理を加える
    }

    /**
     * @fn
     * 攻撃終了処理(override前提)
     * @brief 攻撃終了処理
     */
    public virtual void FinAttack()
    {
        // ここに処理を加える
    }

    /**
     * @fn
     * 攻撃更新処理(override前提)
     * @brief 攻撃処理
     */
    public virtual void UpdateAttack()
    {
        // ここに処理を加える
    }

    /**
     * @fn
     * 武器種類のgetter・setter
     * @return m_eWeaponType(WeaponType)
     * @brief 武器種類を返す・セット
     */
    public WeaponType GetSetWeaponType
    {
        get { return m_eWeaponType; }
        set { m_eWeaponType = value; }
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

    /**
     * @fn
     * 攻撃中のgetter・setter
     * @return m_bAttack(bool)
     * @brief 攻撃中を返す・セット
     */
    public bool GetSetVisible
    {
        get { return m_bVisible; }
        set { m_bVisible = value; }
    }

    /**
     * @fn
     * 耐久値のgetter・setter
     * @return m_nHp(int)
     * @brief 耐久値を返す・セット
     */
    public int GetSetHp
    {
        get { return m_nHp; }
        set { m_nHp = value; }
    }
}
