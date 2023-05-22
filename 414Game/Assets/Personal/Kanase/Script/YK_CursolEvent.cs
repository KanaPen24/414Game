/**
 * @file   YK_CursolEvent.cs
 * @brief  カーソルのイベント
 * @author 吉田叶聖
 * @date   2023/03/31
 * @Update 2023/04/02 UIの情報を獲得できるように変更(Ihara)
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class YK_CursolEvent : MonoBehaviour
{
    [SerializeField] private IS_Player m_Player; // Player
    [SerializeField] private List<YK_UI> m_Uis;  // ゲーム上にあるUIをすべて格納する
    private YK_UI m_CurrentUI;                   // 現在選択中のUI(カーソルが選択しているUI)
    private int Array;  //m_Uisの中身

    /**
     * @fn
     * 初期化関数(外部参照はしないように)
     * @return なし
     * @brief 初期化関数
     */
    private void Awake()
    {
        // メンバの初期化
        m_CurrentUI = null;
        Array = m_Uis.Capacity - 1; //配列のため-1する
    }

    //　マウスアイコンが自分のアイコン上に入った時
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(GameObject.Find("DamageCanvas(Clone)"))
        m_Uis[Array] = GameObject.Find("DamageCanvas(Clone)").GetComponent<YK_DamageUI>();
        // 格納してあるYK_UIのゲームオブジェクトを探す
        for (int i = 0, size = m_Uis.Count; i < size; ++i)
        {
            if (m_Uis[i].gameObject == col.gameObject)
            {
                // 予め格納しておく
                m_CurrentUI = m_Uis[i];
                Debug.Log(m_CurrentUI.GetSetUIType);
                return;
            }
        }
    }
    //　マウスアイコンが自分のアイコン上から出て行った時
    void OnTriggerExit2D(Collider2D col)
    {
        m_CurrentUI = null;
        //Debug.Log("UI無");
    }

    /**
     * @fn
     * 現在選択中のUIのgetter・setter
     * @return m_Current(YK_UI)
     * @brief 現在選択中のUIを返す・セット
     */
    public YK_UI GetSetCurrentUI
    {
        get { return m_CurrentUI; }
        set { m_CurrentUI = value; }
    }
}