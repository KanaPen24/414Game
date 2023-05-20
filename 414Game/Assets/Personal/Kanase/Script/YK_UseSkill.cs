/**
 * @file YK_UseSkill.cs
 * @brief スキルが使えるかどうか判定するもの
 * @author 吉田叶聖
 * @date 2023/05/21
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YK_UseSkill : MonoBehaviour
{
    [SerializeField] private List<YK_SkillIcon> SkillIcons;  // ゲーム上にあるスキルアイコンをすべて格納する
    [SerializeField] private int m_nSkillLimit;  //スキルの使用回数
    private int m_nSkillUseStorage;   //使えるスキルのを格納
    private int m_nSkillUse;          //使うスキル
    private int m_nNoSkillUse;        //使えないスキル
    [SerializeField] private bool m_bSkillUseflg = true;  //スキルの使用できるかできないか

    // Start is called before the first frame update
    void Start()
    {
        m_nSkillLimit = SkillIcons.Capacity;
        m_nSkillLimit--;    //配列のため-１をして調整
    }

    //デバック用
    //private void Update()
    //{        
    //    if (Input.GetKeyDown(KeyCode.F3))
    //        UseSkillJudge();
    //}

    //回避使用時に呼び出す関数
    public bool UseSkillJudge()
    {
        for (int i = 0; i <= m_nSkillLimit; i++)
        {
            //使えるスキルかどうか判断する
            if (!SkillIcons[i].GetUseSkill)
                m_nSkillUse = i;    //使えるスキル番号を入れる
            else
                m_nNoSkillUse++;    //使えないスキルを加算する

            //使えないスキルが上限値を超えた場合
            if (m_nNoSkillUse > m_nSkillLimit)
                m_nSkillUse = -1;   //使えないスキル番号を入れる

            //使えるスキル番号以上なら
            if (m_nSkillUse >= 0)
                m_bSkillUseflg = true;  //使えるようにする
            else
                m_bSkillUseflg = false; //何も使えない場合はフラグをfalseにする
        }
        if (m_bSkillUseflg)
            SkillIcons[m_nSkillUse].UseSkill();
        else
            Debug.Log("回避できないよ");
        //リセット
        m_nNoSkillUse = 0;

        return m_bSkillUseflg;
    }
    /**
* @fn
* スキルの上限値のgetter・seter
* @return m_nSkillLimit(int)
* @brief スキル上限を返す・セット
*/
    public int GetSetSkillLimit
    {
        get { return m_nSkillLimit; }
        set { m_nSkillLimit = value; }
    }
}
