/**
 * @file   IS_Weapon.cs
 * @brief  武器のクラス
 * @author IharaShota
 * @date   2023/03/12
 * @Update 2023/03/12 作成
 * @Update 2023/03/16 武器種類を判別する列挙体追加
 * @Update 2023/03/28 メンバ,仮想関数追加
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
    None      = 0, // 武器無
    HPBar     = 1, // HPバー
    SkillIcon = 2, // スキルアイコン
    BossBar   = 3, // Bossバー
    Clock     = 4, // 時計
    Start     = 5, // スタート

    MaxWeaponType
}

public class IS_Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponType m_eWeaponType;             // 武器の種類
    [SerializeField] protected bool m_bVisible;                      // 表示するかどうか
    [SerializeField] protected bool m_bDestroy;                      // 破壊されたかどうか
    [SerializeField] protected int  m_nHp;                           // 耐久値
    [SerializeField] protected int  m_nMaxHp;                        // 最大耐久値

    /**
     * @fn
     * ゲーム起動時処理(override前提)
     * @brief ゲーム起動時処理(外部参照は避けること)
     */
    protected virtual void Awake()
    {

    }

    /**
     * @fn
     * ゲーム起動時処理(override前提)
     * @brief ゲーム起動時処理(外部参照する場合)
     */
    protected virtual void Start()
    {

    }

    /**
     * @fn
     * 初期化処理(override前提)
     * @brief 初期化処理
     */
    public virtual void Init()
    {

    }

    /**
     * @fn
     * 終了処理(override前提)
     * @brief 終了処理
     */
    public virtual void Uninit()
    {

    }

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
     * 溜め初期化処理(override前提)
     * @brief 溜め初期化処理
     */
    public virtual void StartCharge()
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
     * 溜め終了処理(override前提)
     * @brief 溜め終了処理
     */
    public virtual void FinCharge()
    {
        // ここに処理を加える
    }

    /**
     * @fn
     * 攻撃更新処理(override前提)
     * @brief 攻撃更新処理
     */
    public virtual void UpdateAttack()
    {
        // ここに処理を加える
    }

    /**
     * @fn
     * 溜め更新処理(override前提)
     * @brief 溜め更新処理
     */
    public virtual void UpdateCharge()
    {
        // ここに処理を加える
    }

    /**
     * @fn
     * 表示更新処理(override前提)
     * @brief 表示更新処理
     */
    public virtual void UpdateVisible()
    {

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
    public bool GetSetVisible
    {
        get { return m_bVisible; }
        set { m_bVisible = value; }
    }

    /**
     * @fn
     * 破壊のgetter・setter
     * @return m_bDestroy(bool)
     * @brief 破壊を返す・セット
     */
    public bool GetSetDestroy
    {
        get { return m_bDestroy; }
        set { m_bDestroy = value; }
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

   /**
    * @fn
    * 最大耐久値のgetter・setter
    * @return m_nMaxHp(int)
    * @brief 最大耐久値を返す・セット
    */
    public int GetSetMaxHp
    {
        get { return m_nMaxHp; }
        set { m_nMaxHp = value; }
    }

    public void AddLife(int heal)
    {
        // 現在の体力値から 引数 heal の値を足す
        m_nHp += heal;
        // 現在の体力値が m_nMaxHp 以上の場合
        if (m_nHp >= m_nMaxHp)
        {
            // 現在の体力値に 最大値 を代入
            m_nHp = m_nMaxHp;
        }
    }

    public void DelLife(int damage)
    {
        // 現在の体力値から 引数 damage の値を引く
        m_nHp -= damage;
        // 現在の体力値が 0 以下の場合
        if (m_nHp <= 0)
        {
            // 現在の体力値に 0 を代入
            m_nMaxHp = 0;
            // コンソールに"Dead!"を表示する
            Debug.Log("Dead!");
        }
    }
}
