/**
 * @file   YK_CursolEvent.cs
 * @brief  カーソルのイベント
 * @author 吉田叶聖
 * @date   2023/03/31
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class YK_CursolEvent : MonoBehaviour
{

    //　このスロットのアイテム名
    private string itemName;
   

    void Start()
    {
        
    }

    //　マウスアイコンが自分のアイコン上に入った時
    private void OnTriggerEnter2D(Collider2D col)
    {
        CheckEvent(col);
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        CheckEvent(col);
    }
    void CheckEvent(Collider2D col)
    {
        //　アイコンを検知する
        if (col.tag == "UI")
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {

                //　このUIをフォーカスする
                EventSystem.current.SetSelectedGameObject(gameObject);
                itemName = col.name;

                //　アイテムに応じて表示する情報を変える
                if (itemName == "HPBack")
                {
                    Debug.Log("HPバーに当たってる");
                }
                else if (itemName == "armor")
                {

                }
            }
        }
    }

    //　マウスアイコンが自分のアイコン上から出て行った時
    void OnTriggerExit2D(Collider2D col)
    {

        if (col.tag == "UI")
        {
            //　フォーカスを解除する
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}