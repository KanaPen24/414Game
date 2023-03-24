/**
 * @file   YK_UI.cs
 * @brief  UIのクラス
 * @author 吉田叶聖
 * @date   2023/03/17
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ================================================
// UIType
// … UI種類の列挙体
// ================================================
public enum UIType
{
    HPBar,      // HPバー
    SkillIcon,  // スキルアイコン

    MaxUIType
}

public class YK_UI : MonoBehaviour
{
    protected UIType m_eUIType; // UIの種類
    protected bool m_bVisible;  // 表示非表示フラグ
    protected int m_nHP;        // 体力
    protected Vector3 m_Pos;    // UIの座標

    /**
     * @fn
     * UI種類のgetter・setter
     * @return m_eUIType(UIType)
     * @brief 武器種類を返す・セット
     */
    public UIType GetSetUIType
    {
        get { return m_eUIType; }
        set { m_eUIType = value; }
    }

    /**
    * @fn
    * 表示非表示のgetter・setter
    * @return m_bVisible(bool)
    * @brief 表示中を返す・セット
    */
    public bool GetSetVisible
    {
        get { return m_bVisible; }
        set { m_bVisible = value; }
    }

    /**
   * @fn
   * 表示非表示のgetter・setter
   * @return m_nHP(int)
   * @brief 表示中を返す・セット
   */
    public int GetSetHP
    {
        get { return m_nHP; }
        set { m_nHP = value; }
    }
    /**
 * @fn
 * 表示非表示のgetter・setter
 * @return m_Pos(Vector3)
 * @brief 表示中を返す・セット
 */
    public Vector3 GetSetPos
    {
        get { return m_Pos; }
        set { m_Pos = value; }
    }

    // ダメージ処理
    public void DelLife(int damage)
    {
        // 現在の体力値から 引数 damage の値を引く
        m_nHP -= damage;
        // 現在の体力値が 0 以下の場合
        if (m_nHP <= 0)
        {
            // 現在の体力値に 0 を代入
            m_nHP = 0;
            // コンソールに"Dead!"を表示する
            Debug.Log("Dead!");
        }
    }
    // 回復処理
    public void AddLife(int heal,int maxHP)
    {
        // 現在の体力値から 引数 heal の値を足す
        m_nHP += heal;
        // 現在の体力値が maxHealth 以上の場合
        if (m_nHP >= maxHP)
        {
            // 現在の体力値に 最大値 を代入
            m_nHP = maxHP;
            // コンソールに"HPBarHPMax!"を表示する
            Debug.Log("HPBarHPMax!");
        }
    }
}
