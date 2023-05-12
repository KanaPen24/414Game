﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossSlime_Fall : NK_BossSlimeStrategy
{
    //ボスすらいむをアタッチ
    [SerializeField] private NK_BossSlime m_BossSlime;
    private float m_FloorPos;
    [SerializeField] private float m_FallPow;

    private void Start()
    {
        m_FloorPos = this.transform.position.y;
    }

    public override void UpdateStrategy()
    {
        m_BossSlime.m_BSMoveValue = new Vector3(0.0f, 0.0f, 0.0f);
        if(this.gameObject.transform.position.y>m_FloorPos)
        {
            m_BossSlime.m_BSMoveValue.y -= m_FallPow;
        }
        else
        {
            m_BossSlime.GetSetBossSlimeState = BossSlimeState.BossSlimeWait;
        }
    }

}