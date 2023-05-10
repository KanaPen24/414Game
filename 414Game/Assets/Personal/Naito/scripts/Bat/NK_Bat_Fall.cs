using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_Bat_Fall : NK_BatStrategy
{
    //地面の位置
    [SerializeField] private float m_FloorPos;
    //空飛んでいるときのY軸の位置
    //private float m_FlyPosY;
    //リジットボディ
    private Rigidbody m_Rbody;
    //落ちる時の力
    [SerializeField] private float m_FoolPow;
    [SerializeField] private NK_Bat m_Bat;
    [SerializeField] private float m_Reng;
    private bool m_FallFlag;
    [SerializeField] private float m_fMovePow;

    private void Start()
    {
        //m_FlyPosY = this.gameObject.transform.position.y;
        m_Rbody = GetComponent<Rigidbody>();
    }

    public override void UpdateStrategy()
    {
        m_Bat.m_MoveValue = new Vector3(0.0f, 0.0f, 0.0f);

        if ((this.gameObject.transform.position.x>m_Bat.m_BPlayer.transform.position.x-m_Reng)&&
            (this.gameObject.transform.position.x < m_Bat.m_BPlayer.transform.position.x + m_Reng))
        {
            m_FallFlag = true;
        }
        else
        {
            if (!m_FallFlag)
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
        if(this.gameObject.transform.position.y>m_FloorPos)
        {
            if (m_FallFlag)
            {
                m_Bat.m_MoveValue.y -= m_FoolPow;
            }
        }
        else
        {
            m_Bat.GetSetBatState = BatState.BatUp;
            m_FallFlag = false;
        }
    }
}
