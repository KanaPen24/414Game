using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_Bat_Fall : NK_BatStrategy
{
    [SerializeField] private float m_FloorPos;
    //落ちる時の力
    [SerializeField] private float m_FallPow;
    [SerializeField] private NK_Bat m_Bat;
    [SerializeField] private float m_UpTime;
    private float m_Cnt;

    private void Start()
    {
    }

    public override void UpdateStrategy()
    {
        m_Bat.m_MoveValue = new Vector3(0.0f, 0.0f, 0.0f);

        if(this.gameObject.transform.position.y>m_FloorPos)
        {
                m_Bat.m_MoveValue.y -= m_FallPow;
        }
        else
        {
            m_Cnt += Time.deltaTime;
            if (m_Cnt > m_UpTime)
            {
                m_Cnt = 0f;
                m_Bat.GetSetFallFlag = false;
                m_Bat.GetSetBatState = BatState.BatUp;
            }
        }
    }
}
