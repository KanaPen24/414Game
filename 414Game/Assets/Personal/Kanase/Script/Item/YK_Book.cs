/**
 * @file YK_Book.cs
 * @brief 本の制御
 * @author 吉田叶聖
 * @date 2023/05/28
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YK_Book : YK_Item
{
    [SerializeField] GameObject book;

    private void Start()
    {
        m_eItemType = ItemType.Book; //アイテムのタイプ設定
        GetSetItemHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            GetSetItemHit = true;
        }
        Destroy(book);
        book = null;
    }
 
}
