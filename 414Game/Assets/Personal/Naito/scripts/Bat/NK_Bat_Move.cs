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

    public void Update()
    {
        m_fCnt += Time.deltaTime;
        if(m_fCnt > m_fMoveCnt)
        {
            m_fCnt = 0.0f;
            
        }
    }
    public override void UpdateStrategy()
    {
        m_Bat.m_MoveValue = new Vector3(0.0f, 0.0f, 0.0f);

        if(m_Bat.GetSetBatDir == BatDir.Left)
        {
            m_Bat.m_MoveValue.x -= m_fMovePow;
        }
        if (m_Bat.GetSetBatDir == BatDir.Right)
        {
            m_Bat.m_MoveValue.x += m_fMovePow;
        }
    }
}
