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
    private string HPBar = "HPBack";
    private string Skill = "Skill";
    private string Skill1 = "Skill1";
    private string Skill2 = "Skill2";
    private string Skill3 = "Skill3";
    private string Skill4 = "Skill4";
    public static int m_nUINumber = 0;

    [SerializeField] private IS_Player m_Player;


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
               switch(itemName)
                {
                    case "HPBack":
                        m_nUINumber = 0;
                        m_Player.GetSetEquipWeaponState = EquipWeaponState.PlayerHpBar;
                        m_Player.GetSetEquip = true;
                        Debug.Log("体力バー");
                        break;
                    case "Skill":
                        m_nUINumber = 1;
                        m_Player.GetSetEquipWeaponState = EquipWeaponState.PlayerBall;
                        m_Player.GetSetEquip = true;
                        Debug.Log("スキルアイコン");
                        break;
                    case "Skill1":
                        m_nUINumber = 2;
                        m_Player.GetSetEquipWeaponState = EquipWeaponState.PlayerBall;
                        m_Player.GetSetEquip = true;
                        Debug.Log("スキルアイコン1");
                        break;
                    case "Skill2":
                        m_nUINumber = 3;
                        m_Player.GetSetEquipWeaponState = EquipWeaponState.PlayerBall;
                        m_Player.GetSetEquip = true;
                        Debug.Log("スキルアイコン2");
                        break;
                    case "Skill3":
                        m_nUINumber = 4;
                        m_Player.GetSetEquipWeaponState = EquipWeaponState.PlayerBall;
                        m_Player.GetSetEquip = true;
                        Debug.Log("スキルアイコン3");
                        break;
                    case "Skill4":
                        m_nUINumber = 5;
                        m_Player.GetSetEquipWeaponState = EquipWeaponState.PlayerBall;
                        m_Player.GetSetEquip = true;
                        Debug.Log("スキルアイコン4");
                        break;
                    default:
                        break;
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
            itemName = "何もない";
            Debug.Log("何もない");
        }
    }

    public int GetUINumber()
    {
        return m_nUINumber;
    }

}