﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_But_Sonic : NK_BatStrategy
{
    //超音波
    [SerializeField] private GameObject m_gSonic;
    //発射場所
    [SerializeField] private Vector3 m_SonicPos;
    //NK_Batをアタッチ
    [SerializeField] private NK_Bat m_Bat;
    public override void UpdateStrategy()
    {
        Instantiate(m_gSonic, m_SonicPos, Quaternion.AngleAxis(GetAim(), Vector3.forward));

    }

    private float GetAim()
    {
        Vector2 p1 = m_SonicPos;
        Vector2 p2 = m_Bat.m_BPlayer.transform.position;
        float dx = p2.x - p1.x;
        float dy = p2.y - p1.y;
        float rad = Mathf.Atan2(dy, dx);

        return rad * Mathf.Rad2Deg;
    }
}