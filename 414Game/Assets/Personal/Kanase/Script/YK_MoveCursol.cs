/**
 * @file   YK_MoveCursol.cs
 * @brief  カーソルを動かす
 * @author 吉田叶聖
 * @date   2023/03/31
 * @Update 2023/06/19 Input情報改定(Ihara)
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
    private float m_fCircle_Radius;
    //円運動の半径限界値
    [SerializeField]
    private float m_fCircle_Radius_Limit;
    //円運動の半径保存用
    private float m_fCircle_Radius_Storage;
    //円運動のスピード
    [SerializeField]
    private float m_fCircle_Speed;
    //円運動のスピード保存用
    private float m_fCircle_Speed_Storage;
    //現時点の座標を保存する
    private Vector2 m_fPos;
    //回転方向の正負
    [SerializeField]
    private bool m_bDirection = false;
    //エフェクト用のオブジェクト
    [SerializeField]
    private GameObject Effect;
    //リジットボディ
    Rigidbody2D rb;
    //カーソルイベント
    [SerializeField] private YK_CursolEvent CursolEvent;
    //指定座標に移動
    [SerializeField] RectTransform target;
    //到達したら
    private bool m_bMove = false;
    //　アイコンが1秒間に何ピクセル移動するか
    [SerializeField]
    private float m_fTargetSpeed = 1.0f;    //ターゲットまで移動するスピード
    //キャンバス
    [SerializeField] Canvas canvas;
    private float m_fDeadZone;   //コントローラーのスティックデッドゾーン

    void Start()
    {
        rect = GetComponent<RectTransform>();
        //　オフセット値をアイコンのサイズの半分で設定
        offset = new Vector2(rect.sizeDelta.x / 2f, rect.sizeDelta.y / 2f);
        // 初期値を設定
        m_fPos = rect.anchoredPosition;
        //Rigidbodyを取得
        rb = GetComponent<Rigidbody2D>();

        m_fDeadZone = 0.2f;
        m_fCircle_Radius_Storage = m_fCircle_Radius;
        m_fCircle_Speed_Storage = m_fCircle_Speed;
    }

    void Update()
    {
        //最初にカーソルを演出でターゲットまで動かす
        if (!m_bMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, m_fTargetSpeed * Time.deltaTime);
            if (transform.position == target.position) 
            {
                if(m_fCircle_Radius<=m_fCircle_Radius_Limit)
                //最初のタイミングだけは半径とスピードを変える
                m_fCircle_Radius++;
                m_fCircle_Speed = m_fCircle_Speed_Storage / 2f;
                Circle();
            }
            //ちょっとでも動かしたら
            //if (Mathf.Abs(Input.GetAxis("HorizontalR")) >= m_fDeadZone || Mathf.Abs(Input.GetAxis("VerticalR")) >= m_fDeadZone)
            if (Mathf.Abs(IS_XBoxInput.RStick_H) >= m_fDeadZone || Mathf.Abs(IS_XBoxInput.RStick_V) >= m_fDeadZone)
            {
                m_bMove = true;
                //  元に戻す
                m_fCircle_Radius = m_fCircle_Radius_Storage;
                m_fCircle_Speed = m_fCircle_Speed_Storage;
            }
            return;
        }
        //　移動キーを押していなければ円運動
        if (Mathf.Approximately(IS_XBoxInput.RStick_H, 0f) && Mathf.Approximately(IS_XBoxInput.RStick_V, 0f))
        {
            Circle();
            if (CursolEvent.GetSetCurrentUI == null)
                ZeroVelocity();
            return;
        }
        //　移動先を計算
        var pos = rect.anchoredPosition + new Vector2(IS_XBoxInput.RStick_H * m_fIconSpeed, IS_XBoxInput.RStick_V * m_fIconSpeed) * Time.deltaTime;

       
        //　カーソルが画面内でループ
        //X座標
        if (pos.x >= canvas.GetComponent<RectTransform>().rect.width / 2.0f)
            pos.x = -canvas.GetComponent<RectTransform>().rect.width / 2.0f;
       else if (pos.x <= -canvas.GetComponent<RectTransform>().rect.width / 2.0f)
            pos.x = canvas.GetComponent<RectTransform>().rect.width / 2.0f;
        //Y座標
        if (pos.y >= canvas.GetComponent<RectTransform>().rect.height / 2.0f)
            pos.y = -canvas.GetComponent<RectTransform>().rect.height / 2.0f;
        else if (pos.y <= -canvas.GetComponent<RectTransform>().rect.height / 2.0f)
            pos.y = canvas.GetComponent<RectTransform>().rect.height / 2.0f;
        //　位置を設定
        rect.anchoredPosition = pos;
        m_fPos = pos;

        if (CursolEvent.GetSetCurrentUI == null)
            ZeroVelocity();

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
    public void ZeroVelocity()
    {
        //Rigidbodyを0にする
        //やらないと離れても磁力が発生し続ける
        rb.velocity = Vector3.zero;        
    }
    /**
* @fn
* 到達したかどうかのフラグのgetter・setter
* @return m_bArrival(bool)
* @brief 到達フラグを返す・セット
*/
    public bool GetSetMoveFlg
    {
        get { return m_bMove; }
        set { m_bMove = value; }
    }
}