/**
 * @file   YK_MoveCursol.cs
 * @brief  カーソルを動かす
 * @author 吉田叶聖
 * @date   2023/03/31
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YK_MoveCursol : MonoBehaviour
{
    //　アイコンが1秒間に何ピクセル移動するか
    [SerializeField]
    private float m_fIconSpeed = Screen.width;
    //　アイコンのサイズ取得で使用
    private RectTransform rect;
    //　アイコンが画面内に収まる為のオフセット値
    private Vector2 offset;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        //　オフセット値をアイコンのサイズの半分で設定
        offset = new Vector2(rect.sizeDelta.x / 2f, rect.sizeDelta.y / 2f);
    }

    void Update()
    {
        //　移動キーを押していなければ何もしない
        if (Mathf.Approximately(Input.GetAxis("HorizontalR"), 0f) && Mathf.Approximately(Input.GetAxis("VerticalR"), 0f))
        {   
            return;
        }
        //　移動先を計算
        var pos = rect.anchoredPosition + new Vector2(Input.GetAxis("HorizontalR") * m_fIconSpeed, Input.GetAxis("VerticalR") * -m_fIconSpeed) * Time.deltaTime;

        //　アイコンが画面外に出ないようにする
        pos.x = Mathf.Clamp(pos.x, -Screen.width * 0.5f + offset.x, Screen.width * 0.5f - offset.x);
        pos.y = Mathf.Clamp(pos.y, -Screen.height * 0.5f + offset.y, Screen.height * 0.5f - offset.y);
        //　アイコン位置を設定
        rect.anchoredPosition = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "UI")
        {
            Debug.Log("当たった");
        }
    }
}