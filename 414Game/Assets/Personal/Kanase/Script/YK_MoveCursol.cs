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
    //円運動の半径
    [SerializeField]
    private float m_fCircle_Radius = 20.0f;
    //円運動の半径
    [SerializeField]
    private float m_fCircle_Speed = 0.1f;
    //現時点の座標を保存する
    private Vector2 m_fPos;
    //回転方向の正負
    [SerializeField]
    private bool m_bDirection = false;
    //エフェクト用のオブジェクト
    [SerializeField]
    private GameObject Effect;


    void Start()
    {
        rect = GetComponent<RectTransform>();
        //　オフセット値をアイコンのサイズの半分で設定
        offset = new Vector2(rect.sizeDelta.x / 2f, rect.sizeDelta.y / 2f);
        // 初期値を設定
        m_fPos = rect.anchoredPosition;
    }

    void Update()
    {
        //　移動キーを押していなければ円運動
        if (Mathf.Approximately(Input.GetAxis("HorizontalR"), 0f) && Mathf.Approximately(Input.GetAxis("VerticalR"), 0f))
        {
            Circle();
            return;
        }
        //　移動先を計算
        var pos = rect.anchoredPosition + new Vector2(Input.GetAxis("HorizontalR") * m_fIconSpeed, Input.GetAxis("VerticalR") * -m_fIconSpeed) * Time.deltaTime;

        //　カーソルが画面外に出ないようにする
        pos.x = Mathf.Clamp(pos.x, -Screen.width * 0.5f + offset.x, Screen.width * 0.5f - offset.x);
        pos.y = Mathf.Clamp(pos.y, -Screen.height * 0.5f + offset.y, Screen.height * 0.5f - offset.y);
        //　位置を設定
        rect.anchoredPosition = pos;
        m_fPos = pos;
    }

    //円運動
    void Circle()
    {
        Vector2 pos = rect.anchoredPosition;

        float rad = m_fCircle_Speed * Mathf.Rad2Deg * Time.time;

        if (m_bDirection)
            rad *= -1.0f;   //時計回りにする
        pos.x = Mathf.Cos(rad) * m_fCircle_Radius;
        pos.y = Mathf.Sin(rad) * m_fCircle_Radius;
        
        Effect.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.transform.position = collision.transform.position;
    }

}