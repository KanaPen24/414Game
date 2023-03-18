/**
 * @file YK_SkillIcon.cs
 * @brief スキルのアイコン処理
 * @author 吉田叶聖
 * @date 2023/03/16
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class YK_SkillIcon : MonoBehaviour
{

    // フェードさせる時間を設定
    [SerializeField]
    [Tooltip("フェードさせる時間(秒)")]
    private float fadeTime = 1f;
    // 経過時間を取得
    private float timer;

    private int m_SkillPoint;  //現在のスキル数
    public CanvasGroup[] SkillArray;
    public int m_nMaxSkill; //最高スキル数

    private void Start()
    {
        CanvasGroup[] SkillArray = new CanvasGroup[m_SkillPoint];
        m_SkillPoint = m_nMaxSkill;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AddSkill(1);
        }

        else if (Input.GetMouseButtonDown(1))
        {
            DelSkill(1); 
        }
    }
    //スキル数増やす
    public void AddSkill(int Heal)
    {
        if (m_SkillPoint < m_nMaxSkill)
        {
            m_SkillPoint+=Heal;
            SkillArray[m_SkillPoint - 1].alpha = 1;
        }
    }
    //スキル数減らす
    public void DelSkill(int Use)
    {
        if(m_SkillPoint > 0)
        {
            SkillArray[m_SkillPoint - 1].alpha = 0;
            m_SkillPoint-=Use;
        }
    }

}
