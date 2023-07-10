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
    HPBar        = 1, // HPバー
    SkillIcon    = 2, // スキルアイコン
    BossBar      = 3, // Bossバー
    Clock        = 4, // 時計
    Start        = 5, // スタート
    Next         = 6, // Next
    Retry        = 7, // リトライ
    TitleBack    = 8, // タイトルへ戻る
    Exit         = 9, // 終了
    Combo        = 10,// コンボ
    DamageNumber = 11,// ダメージ
    Clear        = 12,// クリア
    Notification = 13,// 通知

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
    protected Vector3 m_UIPos;         // UIの座標
    protected Vector2 m_UIScale;       // スケール
    protected Vector3 m_MinScale = new Vector3(0.5f, 0.5f, 0.0f); // 最小サイズ
    protected float m_fDelTime = 0.4f;            // 減算していく時間


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
 * UI座標のgetter・setter
 * @return m_Pos(Vector3)
 * @brief UI座標を返す・セット
 */
    public Vector3 GetSetUIPos
    {
        get { return m_UIPos; }
        set { m_UIPos = value; }
    }

    /**
* @fn
* UIスケールのgetter・setter
* @return m_Scale(Vector3)
* @brief UIスケールを返す・セット
*/
    public Vector2 GetSetUIScale
    {
        get { return m_UIScale; }
        set { m_UIScale = value; }
    }

    public virtual void UIFadeIN()
    {

    }

    public virtual void UIFadeOUT()
    {

    }
}
