/**
 * @file   YK_ScaleDownOrUp.cs
 * @brief  UIのスケールを変える
 * @author 吉田叶聖
 * @date   2023/05/24
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YK_ScaleDownOrUp : MonoBehaviour
{
    private float time, changeSpeed;
    private bool enlarge;
    private bool m_bScallDownUp;
    private Vector2 Scale;  //初期値を保存

    void Start()
    {
        enlarge = true;
        //初期値を保存
        Scale = transform.localScale;
    }

    void Update()
    {
        // 拡縮フラグがオフの場合は処理を終了
        if (!m_bScallDownUp)
            return;

        // 拡縮の変化速度を計算
        changeSpeed = Time.deltaTime * 0.8f;

        // 時間に応じて拡大・縮小の切り替えを行う
        if (time < 0)
        {
            enlarge = true;
        }
        if (time > 0.45f)
        {
            enlarge = false;
        }

        // 拡大・縮小の処理を実行
        if (enlarge == true)
        {
            // 時間を加算し、スケールを増加させる
            time += Time.deltaTime;
            transform.localScale += new Vector3(changeSpeed, changeSpeed, changeSpeed);
        }
        else
        {
            // 時間を減算し、スケールを減少させる
            time -= Time.deltaTime;
            transform.localScale -= new Vector3(changeSpeed, changeSpeed, changeSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //カーソルに当たったら
        if (collision.gameObject.name == "Cursol")
        {
            m_bScallDownUp = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //カーソルに当たったら
        if (collision.gameObject.name == "Cursol")
        {
            m_bScallDownUp = false;
            transform.localScale = Scale;
        }
    }
    /**
* @fn
* 拡縮フラグのgetter・setter
* @return m_bArrival(bool)
* @brief 拡縮フラグを返す・セット
*/
    public bool GetSetScalelFlg
    {
        get { return m_bScallDownUp; }
        set { m_bScallDownUp = value; }
    }
}