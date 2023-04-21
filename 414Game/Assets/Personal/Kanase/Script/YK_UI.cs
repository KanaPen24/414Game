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
    BossBar,    // Bossバー
    Next,       // Next

    MaxUIType
}

// ================================================
// FadeState
// … Fadeを管理する列挙体
// ================================================
public enum FadeState
{
    FadeNone,
    FadeIN,
    FadeOUT,

    MaxFadeState
}

public class YK_UI : MonoBehaviour
{
    protected UIType m_eUIType;      // UIの種類
    protected FadeState m_eFadeState;// フェードの状態
    protected bool m_bVisible;       // 表示非表示フラグ
    protected Vector3 m_Pos;         // UIの座標
    protected Vector2 m_Scale;       // スケール

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
     * Fade状態のgetter・setter
     * @return m_eFadeState(FadeState)
     * @brief Fade状態を返す・セット
     */
    public FadeState GetSetFadeState
    {
        get { return m_eFadeState; }
        set { m_eFadeState = value; }
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
 * @return m_Pos(Vector3)
 * @brief 表示中を返す・セット
 */
    public Vector3 GetSetPos
    {
        get { return m_Pos; }
        set { m_Pos = value; }
    }

    /**
* @fn
* 表示非表示のgetter・setter
* @return m_Scale(Vector3)
* @brief 表示中を返す・セット
*/
    public Vector2 GetSetScale
    {
        get { return m_Scale; }
        set { m_Scale = value; }
    }

    public virtual void UIFadeIN()
    {

    }

    public virtual void UIFadeOUT()
    {

    }
}
