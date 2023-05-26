using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_Bat_Move : NK_BatStrategy
{
    [SerializeField] private NK_Bat m_Bat;
    [SerializeField] private float m_fMovePow;
    private float m_fCnt;
    //動き出すまでの時間
    [SerializeField] private float m_fMoveCnt;
    private int m_Rand;
    private float m_BatPosY;
    [SerializeField] private float m_Reng;

    private void Start()
    {
        m_BatPosY = this.gameObject.transform.position.y;
    }
    public override void UpdateStrategy()
    {
        m_fCnt += Time.deltaTime;
        m_Bat.m_MoveValue = new Vector3(0.0f, 0.0f, 0.0f);

        if(m_fCnt > m_fMoveCnt)
        {
            m_fCnt = 0.0f;
            m_Rand = Random.Range(1, 4);
            if(m_Rand <= 1)
            {
                m_Bat.GetSetFlightFlag = true;
                m_Bat.GetSetBatState = BatState.BatFlight;
            }
            else
            {
                // SE再生
                IS_AudioManager.instance.PlaySE(SEType.SE_Sonic);
                m_Bat.GetSetBatState = BatState.BatSonic;
            }
        }

        if (this.gameObject.transform.position.x > m_Bat.m_BPlayer.transform.position.x - m_Reng &&
            this.gameObject.transform.position.x < m_Bat.m_BPlayer.transform.position.x + m_Reng)
        {

        }
        else
        {
            if (m_Bat.GetSetBatDir == BatDir.Left)
            {
                m_Bat.m_MoveValue.x -= m_fMovePow;
            }
            if (m_Bat.GetSetBatDir == BatDir.Right)
            {
                m_Bat.m_MoveValue.x += m_fMovePow;
            }
        }
        transform.position = new Vector3(transform.position.x, m_BatPosY + Mathf.PingPong(Time.time, 0.3f), transform.position.z);
    }
}
