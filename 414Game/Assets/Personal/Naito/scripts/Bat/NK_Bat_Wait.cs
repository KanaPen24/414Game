using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_Bat_Wait : NK_BatStrategy
{
    [SerializeField] private bat m_Bat;
    [SerializeField] private float m_MoveTime;
    private float m_fCnt;
    private float m_BatPosY;

    private void Start()
    {
        m_BatPosY = this.gameObject.transform.position.y;
    }

    public override void UpdateStrategy()
    {
        m_fCnt += Time.deltaTime;
        transform.position = new Vector3(transform.position.x, m_BatPosY + Mathf.PingPong(Time.time, 0.3f), transform.position.z);
        if (m_fCnt > m_MoveTime)
        {
            m_fCnt = 0.0f;
            m_Bat.GetSetBatState = batState.BatMove;
        }
    }
}
