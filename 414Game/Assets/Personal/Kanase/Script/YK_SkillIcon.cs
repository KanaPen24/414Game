﻿/**
 * @file YK_SkillIcon.cs
 * @brief スキルのアイコン処理
 * @author 吉田叶聖
 * @date 2023/03/16
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class YK_SkillIcon : YK_UI
{
    private int m_SkillPoint;  //現在のスキル数
    public CanvasGroup[] SkillArray;
    public int m_nMaxSkill; //最高スキル数

    private void Start()
    {
        m_eUIType = UIType.SkillIcon;
        CanvasGroup[] SkillArray = new CanvasGroup[m_SkillPoint];
        m_SkillPoint = m_nMaxSkill;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ResetSkill();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DelSkill(1); 
        }
    }
    //スキル数元に戻す
    public void ResetSkill()
    {
        while (m_SkillPoint < m_nMaxSkill)
        {
            m_SkillPoint++;
            SkillArray[m_SkillPoint - 1].DOFade(1f, 1f);
        }
    }
    //スキル数減らす
    public void DelSkill(int Use)
    {
        if(m_SkillPoint > 0)
        {
            SkillArray[m_SkillPoint - 1].DOFade(0f, 1f);
            m_SkillPoint -=Use;
        }
    }
    //座標引っ張ってくる
    public Vector3 GetPos(int SkillPoint)
    {
        return SkillArray[SkillPoint].GetComponent<RectTransform>().position;
    }

    public void VisibleSkill(int Icon)
    {
        m_SkillPoint--;
        SkillArray[Icon].DOFade(0f, 0.5f);
        if (m_SkillPoint <= 0)
            ResetSkill();
        Debug.Log(m_SkillPoint);
    }

}
