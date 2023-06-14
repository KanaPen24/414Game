/**
 * @file   YK_Item.cs
 * @brief  Itemのクラス
 * @author 吉田叶聖
 * @date   2023/03/17
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ================================================
// ItemType
// … Item種類の列挙体
// ================================================
public enum ItemType
{
    Book,       // 本
    Tonkathi,   // トンカチ
   
    MaxItemType
}

public class YK_Item : MonoBehaviour
{
    protected ItemType m_eItemType;      // Itemの種類    
    protected bool m_bHit;               // 当たったフラグ
    protected Vector3 m_ItemPos;         // Itemの座標
    protected Vector2 m_ItemScale;       // スケール

    /**
     * @fn
     * Item種類のgetter・setter
     * @return m_eItemType(ItemType)
     * @brief 武器種類を返す・セット
     */
    public ItemType GetSetItemType
    {
        get { return m_eItemType; }
        set { m_eItemType = value; }
    }    

    /**
    * @fn
    * 当たったかどうかのgetter・setter
    * @return m_bVisible(bool)
    * @brief 当たったかを返す・セット
    */
    public bool GetSetItemHit
    {
        get { return m_bHit; }
        set { m_bHit = value; }
    }

    /**
 * @fn
 * Item座標のgetter・setter
 * @return m_Pos(Vector3)
 * @brief Item座標を返す・セット
 */
    public Vector3 GetSetItemPos
    {
        get { return m_ItemPos; }
        set { m_ItemPos = value; }
    }

    /**
* @fn
* Itemスケールのgetter・setter
* @return m_Scale(Vector3)
* @brief Itemスケールを返す・セット
*/
    public Vector2 GetSetItemScale
    {
        get { return m_ItemScale; }
        set { m_ItemScale = value; }
    }

    public virtual void ItemFadeIN()
    {

    }

    public virtual void ItemFadeOUT()
    {

    }
}
