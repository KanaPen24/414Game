/**
 * @file   YK_CameraOut.cs
 * @brief  カメラ外にでないようにする
 * @author 吉田叶聖
 * @date   2023/05/28
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YK_CameraOut : MonoBehaviour
{
   
    private bool m_bCameraOut = false;
    

    // Update is called once per frame
    void Update()
    {
        // -----------------
        // 境界外判定
        // -----------------
        //深度値
        float depth = Mathf.Abs(Camera.main.gameObject.transform.position.z);
        // 画面の左下の座標を取得 (左上じゃないので注意)
        float screen_Left = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, depth)).x;
        // 画面の右上の座標を取得 (右下じゃないので注意)
        float screen_Right = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth)).x;
        // 現在のプレイヤーの位置座標
        Vector3 player_pos = transform.position;
        // 画面左端に達した時
        if (player_pos.x < screen_Left)
        {
            //座標をずらす
            transform.position += new Vector3(0.1f, 0, 0f);
            m_bCameraOut = true;
        }
        // 画面右端に達した時、
        else if (player_pos.x > screen_Right)
        {
            //座標をずらす
            transform.position -= new Vector3(0.1f, 0, 0f);
            m_bCameraOut = true;
        }
        else
        {
            m_bCameraOut = false;
        }
    }
    /**
     * @fn
     * カメラ外フラグのgetter・setter
     * @return m_bCameraOut(bool)
     * @brief カメラ外フラグを返す・セット
     */
    public bool GetSetCameraOut
    {
        get { return m_bCameraOut; }
        set { m_bCameraOut = value; }
    }
}
