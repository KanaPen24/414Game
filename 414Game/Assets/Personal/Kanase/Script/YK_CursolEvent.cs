using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class YK_CursolEvent : MonoBehaviour
{

    //　このスロットのアイテム名
    //[SerializeField]
    //private string itemName;
   

    void Start()
    {
        
    }

    //　マウスアイコンが自分のアイコン上に入った時
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("UI"))
        {
            Debug.Log("当たった");
        }
        CheckEvent(col);

    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("UI"))
            Debug.Log("当たった");
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

                Debug.Log(gameObject);

                //　アイテムに応じて表示する情報を変える
                //if (itemName == "axe")
                //{
                    
                //}
                //else if (itemName == "armor")
                //{
                    
                //}
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