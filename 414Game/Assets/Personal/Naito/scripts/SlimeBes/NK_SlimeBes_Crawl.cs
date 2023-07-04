using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_SlimeBes_Crawl : NK_SlimeBesStrategy
{
    [SerializeField] private bes m_SlimeBes;
    private float m_MovePow;
    [SerializeField] private float m_MoveTime;
    private float m_Cnt;
    [SerializeField] private float m_StartTime;
    private float m_StartCnt;
    public override void UpdateStrategy()
    {
        m_Cnt += Time.deltaTime;
        m_SlimeBes.m_MoveValue = new Vector3(0.0f, 0.0f, 0.0f);
            if (m_Cnt < m_MoveTime)
            {
                m_MovePow += 0.03f;
                if (m_SlimeBes.GetSetEnemyDir == EnemyDir.Left)
                {
                    m_SlimeBes.m_MoveValue.x -= m_MovePow;
                }
                if (m_SlimeBes.GetSetEnemyDir == EnemyDir.Right)
                {
                    m_SlimeBes.m_MoveValue.x += m_MovePow;
                }
            }
            else
            {
            m_MovePow = 0.0f;
            m_Cnt = 0;
            m_SlimeBes.GetSetBesState = BesState.BesWait;
            }
        
    }
}
