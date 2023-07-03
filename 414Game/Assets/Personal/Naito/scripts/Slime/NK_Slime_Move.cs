﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_Slime_Move : NK_SlimeStrategy
{
    //リジットボディ
    private Rigidbody m_rBody;
    //横移動
    [SerializeField] private float m_fMovePower;
    //ジャンプ力
    [SerializeField] private float m_fJumpPower;
    //NK_Slimeをアタッチする
    //[SerializeField] private NK_Slime m_Slime;
    //slimeをアタッチ
    [SerializeField] private slime m_Slime;

    private void Start()
    {
        m_rBody = GetComponent<Rigidbody>();
    }

    public override void UpdateStrategy()
    {
        if(m_Slime.GetSetEnemyDir==EnemyDir.Left)
        {
            m_rBody.AddForce(transform.up * m_fJumpPower, ForceMode.Impulse);
            m_rBody.AddForce(transform.right * -m_fMovePower, ForceMode.Impulse);
        }
        else
        {
            m_rBody.AddForce(transform.up * m_fJumpPower, ForceMode.Impulse);
            m_rBody.AddForce(transform.right * m_fMovePower, ForceMode.Impulse);
        }
        m_Slime.GetSetMoveAnimFlag = false;
        m_Slime.GetSetSlimeState = SlimeState.SlimeWait;
    }
}
