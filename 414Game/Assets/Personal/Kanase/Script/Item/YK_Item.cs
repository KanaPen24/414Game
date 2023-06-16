/**
 * @file   YK_Item.cs
 * @brief  Itemのクラス
 * Itemの基本クラスであり、Itemの種類や状態などの情報を保持。
 * - 作成者：吉田叶聖
 * - 作成日：2023/03/17
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ================================================
// ItemType
// … Item種類の列挙体
// ================================================

/**
 * @enum ItemType
 * @brief Itemの種類を表す列挙体
 * 
 * Itemの種類を表すために使用。
 * 「MaxItemType」は列挙体の最大値を表すために使用。
 */
public enum ItemType
{
    Book,       // 本
    Tonkathi,   // トンカチ

    MaxItemType
}

/**
 * @class YK_Item
 * @brief Itemの基本クラス
 * 
 * Itemの基本クラスであり、Itemの種類や状態などの情報を保持。
 * 継承元のクラスで具体的なアイテムの振る舞いを実装。
 */
public class YK_Item : MonoBehaviour
{
    protected ItemType m_eItemType;      // Itemの種類    
    protected bool m_bHit;               // 当たったフラグ
    protected Vector3 m_ItemPos;         // Itemの座標
    protected Vector2 m_ItemScale;       // スケール

    /**
     * @property GetSetItemType
     * @brief Item種類のgetter・setter
     * 
     * Itemの種類を取得または設定。
     * @return m_eItemType(ItemType)
     * @brief Itemの種類を返す・セットするためのプロパティ。
     */
    public ItemType GetSetItemType
    {
        get { return m_eItemType; }
        set { m_eItemType = value; }
    }

    /**
     * @property GetSetItemHit
     * @brief 当たったかどうかのgetter・setter
     * 
     * アイテムがプレイヤーと衝突したかどうかを取得または設定。
     * @return m_bHit(bool)
     * @brief アイテムの衝突状態を返す・セットするためのプロパティ。
     */
    public bool GetSetItemHit
    {
        get { return m_bHit; }
        set { m_bHit = value; }
    }

    /**
     * @property GetSetItemPos
     * @brief Item座標のgetter・setter
     * 
     * Itemの座標を取得または設定。
     * @return m_ItemPos(Vector3)
     * @brief Itemの座標を返す・セットするためのプロパティ。
     */
    public Vector3 GetSetItemPos
    {
        get { return m_ItemPos; }
        set { m_ItemPos = value; }
    }

    /**
     * @property GetSetItemScale
     * @brief Itemスケールのgetter・setter
     * 
     * Itemのスケールを取得または設定。
     * @return m_ItemScale(Vector2)
     * @brief Itemのスケールを返す・セットするためのプロパティ。
     */
    public Vector2 GetSetItemScale
    {
        get { return m_ItemScale; }
        set { m_ItemScale = value; }
    }
}
