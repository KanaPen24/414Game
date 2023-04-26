using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_Bat_Fall : NK_BatStrategy
{
    //地面の位置
    [SerializeField] private Vector3 m_FloorPos;
    //空飛んでいるときのY軸の位置
    //private float m_FlyPosY;
    //リジットボディ
    private Rigidbody m_Rbody;
    //落ちる時の力
    [SerializeField] private float m_FoolPow;

    private void Start()
    {
        //m_FlyPosY = this.gameObject.transform.position.y;
        m_Rbody = GetComponent<Rigidbody>();
    }

    public override void UpdateStrategy()
    {
        if(this.gameObject.transform.position.y>m_FloorPos.y)
        {
            m_Rbody.AddForce(transform.up * m_FoolPow);
        }
        else
        {

        }
    }
}
