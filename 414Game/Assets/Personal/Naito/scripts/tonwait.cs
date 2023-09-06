﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tonwait : NK_SlimeBesStrategy
{
    //[SerializeField] private NK_Slime m_Slime;//NK_Slimeをアタッチする
    [SerializeField] private tonkatienemy m_Slime;
    [SerializeField] private float m_fAttackTime;//攻撃間隔
    private float m_fCnt;
    //private Animator anim;

    //private void Start()
    //{
    //    anim = GetComponent<Animator>();
    //}

    public override void UpdateStrategy()
    {

        m_fCnt += Time.deltaTime;
        if (m_fCnt > m_fAttackTime)
        {
            m_fCnt = 0.0f;
            //anim.SetBool("JumpFlag", true);
            m_Slime.GetSetMoveAnimFlag = true;
            //Invoke("AnimeChange", 0.3f);
            m_Slime.GetSetBesState = tonState.tonCrawl;
        }
    }
    //private void AnimeChange()
    //{
    //    anim.SetBool("JampFlag", false);
    //}
}