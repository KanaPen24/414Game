using UnityEngine;
using System;

public class YK_TargetCamera : MonoBehaviour
{
    [SerializeField]
    [Tooltip("追従させたいターゲット")]
    private IS_Player Player;

    [SerializeField] private Vector3 offset;
    [SerializeField] private float m_fMaxCameraMove;
    [SerializeField] private float m_fMoveCnt;
    private float m_fCameraMove;
    private float m_fCurrnetMoveCnt;
    private float m_fAccel;
    private const float m_fMaxAccel = 3.0f;
    private int nDir;
    // Start is called before the first frame update
    void Start()
    {
        m_fCameraMove = 0;
        m_fCurrnetMoveCnt = 0;
        m_fAccel = m_fMaxAccel;
    }

    /// <summary>
    /// プレイヤーが移動した後にカメラが移動するようにするためにLateUpdateにする。
    /// </summary>
    void FixedUpdate()
    {
        //// 左向きなら…
        //if (Player.GetSetPlayerDir == PlayerDir.Left)
        //{
        //    if (Player.GetSetPlayerState != PlayerState.PlayerWait)
        //    {
        //        if (m_fCurrnetMoveCnt <= -m_fMoveCnt)
        //        {
        //            m_fCurrnetMoveCnt = -m_fMoveCnt;
        //        }
        //        else m_fCurrnetMoveCnt -= Time.deltaTime * m_fAccel;
        //    }

        //    m_fAccel -= 0.1f;
        //}
        //// 右向きなら…
        //else if(Player.GetSetPlayerDir == PlayerDir.Right)
        //{
        //    if (Player.GetSetPlayerState != PlayerState.PlayerWait)
        //    {
        //        if (m_fCurrnetMoveCnt >= m_fMoveCnt)
        //        {
        //            m_fCurrnetMoveCnt = m_fMoveCnt;
        //        }
        //        else m_fCurrnetMoveCnt += Time.deltaTime * m_fAccel;
        //    }

        //    m_fAccel -= 0.1f;
        //}

        //if(m_fAccel <=0f)
        //{
        //    m_fAccel = 0f;
        //}
        //if (m_fAccel >= m_fMaxAccel)
        //{
        //    m_fAccel = m_fMaxAccel;
        //}
        //// 前回と向きが同じであれば…
        //if ((int)Player.GetSetPlayerDir != nDir)
        //{
        //    m_fAccel = m_fMaxAccel;
        //}

        //// 現在の向きを取得
        //nDir = (int)Player.GetSetPlayerDir;

        //m_fCameraMove = m_fMaxCameraMove * (m_fCurrnetMoveCnt / m_fMoveCnt);

        // カメラの位置をターゲットの位置にオフセットを足した場所にする。
        gameObject.transform.position = new Vector3(Player.transform.position.x,0f, Player.transform.position.z) + offset + 
            new Vector3(m_fCameraMove,0f,0f);
    }
}