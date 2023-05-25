using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_Bat_Flight : NK_BatStrategy
{
    [SerializeField] private NK_Bat m_Bat;
    [SerializeField] private float m_Reng;
    [SerializeField] private float m_fMovePow;
    [SerializeField] private float m_MoveTime;
    [SerializeField] private float m_FlightTime;
    private float m_Cnt;
    private bool m_FallFlag;
    private float m_FlightCnt;
    public override void UpdateStrategy()
    {
        m_Cnt += Time.deltaTime;
        m_Bat.m_MoveValue = new Vector3(0.0f, 0.0f, 0.0f);
        if (m_Cnt > m_MoveTime)
        {
            if ((this.gameObject.transform.position.x > m_Bat.m_BPlayer.transform.position.x - m_Reng) &&
                (this.gameObject.transform.position.x < m_Bat.m_BPlayer.transform.position.x + m_Reng)&&
                !m_FallFlag)
            {
                m_FallFlag = true;
            }

            if (m_FallFlag)
            {
                m_FlightCnt += Time.deltaTime;
                if (m_FlightCnt > m_FlightTime)
                {
                    m_Bat.GetSetFlightFlag = false;
                    m_Bat.GetSetFallFlag = true;
                    m_FallFlag = false;
                    m_FlightCnt = 0f;
                    m_Cnt = 0f;
                    m_Bat.GetSetBatState = BatState.BatFall;
                }
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
        }


    }
}
