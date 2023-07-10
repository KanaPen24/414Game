/**
 * @file   YK_CursolEvent.cs
 * @brief  カーソルのイベント
 *         UI要素に関するカーソルの動作とイベント処理を管理します。
 *         各UI要素の検出や選択中のUIの保持などを行います。
 * @author 吉田叶聖
 * @date   2023/03/31
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class YK_CursolEvent : MonoBehaviour
{
    [SerializeField] private List<YK_UI> m_Uis;  // ゲーム上にあるUIをすべて格納するリスト
    private YK_UI m_CurrentUI;                   // 現在選択中のUI（カーソルが選択しているUI）
    private int Array;                           // m_Uisリストの要素数

    /**
     * @fn
     * 初期化関数
     * @return なし
     * @brief 初期化関数
     */
    private void Awake()
    {
        // メンバの初期化
        m_CurrentUI = null;
        Array = m_Uis.Capacity - 1; // 配列の要素数を設定するために-1する
    }

    // カーソルがUIに入った時のイベント処理
    private void OnTriggerStay2D(Collider2D col)
    {
        if (GameObject.Find("DamageCanvas(Clone)"))
        {
            m_Uis[Array] = GameObject.Find("DamageCanvas(Clone)").GetComponent<YK_DamageUI>();
        }

        // 格納してあるYK_UIのゲームオブジェクトを探す
        for (int i = 0, size = m_Uis.Count; i < size; ++i)
        {
            if (m_Uis[i].gameObject == col.gameObject)
            {
                // 選択中のUIを事前に格納しておく
                m_CurrentUI = m_Uis[i];
//                Debug.Log(m_CurrentUI.GetSetUIType);

                // ダメージのUIの場合、カーソルの当たり判定で位置を特定する
                if (m_CurrentUI.GetSetUIType == UIType.DamageNumber)
                {
                    m_CurrentUI.GetSetUIPos = this.gameObject.GetComponent<RectTransform>().anchoredPosition;
                }
                
                return;
            }
        }
    }

    // カーソルがUIから出て行った時のイベント処理
    private void OnTriggerExit2D(Collider2D col)
    {
        m_CurrentUI = null;
    }

    /**
     * @fn
     * 現在選択中のUIのgetter・setter
     * このプロパティを使用すると、現在選択中のUIを取得または設定
     * @return m_CurrentUI (YK_UI) 現在選択中のUI
     * @brief 現在選択中のUIを取得または設定
     */
    public YK_UI GetSetCurrentUI
    {
        get { return m_CurrentUI; }
        set { m_CurrentUI = value; }
    }
}
