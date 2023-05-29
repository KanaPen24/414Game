/**
 * @file YK_SkillIcon.cs
 * @brief スキルのアイコン処理
 * @author 吉田叶聖
 * @date 2023/03/16
 * @Update 2023/04/03 フェード処理実装(Ihara)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class YK_SkillIcon : YK_UI
{
    [SerializeField] private Image SkillIcon;
    [SerializeField] private Image SkillInner;      //スキルのインナー
    [SerializeField] private Image OutLine;         //アウトライン
    [SerializeField] private int m_nStuck = 1;      //弾数ストック
    private float m_fCoolTime = 0.0f;               //スキルのクールタイム
    [SerializeField] private float m_fCoolTimeLimit;    //スキルのクールタイム
    [SerializeField] private YK_UseSkill Use;     //スキルを使ったか管理するもの
    private bool m_bSkillUse;                     //スキルが使われたかどうか    
    [SerializeField] private ParticleSystem HealParticle;   //UI回復エフェクト
    [SerializeField] private YK_MoveCursol MoveCursol;
    private bool m_bNowWeapon = false;  //武器化中かどうか

    private void Start()
    {
        m_eUIType = UIType.SkillIcon;   //UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        OutLine.enabled = false;
        //座標取得
        GetSetPos = SkillIcon.GetComponent<RectTransform>().anchoredPosition;
        //スケール取得
        GetSetScale = SkillIcon.transform.localScale;

    }

    private void Update()
    {
        //カーソルが到達するまで
        if (!MoveCursol.GetSetArrivalFlg)
        {
            GetComponent<PointEffector2D>().enabled = false;    //エフェクターを無効にすることで道中吸い寄せられない
            return;
        }
        //残弾数を制限
        m_nStuck = Mathf.Min(m_nStuck, 1);
        // ストック数が0になったら非表示,当たり判定なし
        if (m_nStuck <= 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<PointEffector2D>().enabled = false;
            GetComponent<Image>().enabled = false;
            SkillInner.GetComponent<Image>().enabled = false;
            m_bSkillUse = true;
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<PointEffector2D>().enabled = true;
            GetComponent<Image>().enabled = true;
            SkillInner.GetComponent<Image>().enabled = true;
            if (m_bSkillUse && !m_bNowWeapon)
            {
                m_fCoolTime += Time.deltaTime;
                //float型の値を代入する
                SkillInner.fillAmount = 1.0f - m_fCoolTime / m_fCoolTimeLimit;
                if (SkillInner.fillAmount <= 0.0f)
                {
                    m_bSkillUse = false;
                    HealParticle.Play();
                }
            }
        }
    }

    public override void UIFadeIN()
    {
        m_eFadeState = FadeState.FadeIN;
        // 0秒で後X,Y方向を元の大きさに変更
        SkillIcon.transform.DOScale(GetSetScale, 0f);
        // 0秒でテクスチャをフェードイン
        OutLine.DOFade(1f, 0f);
        SkillInner.DOFade(0.7f, 0f);
        SkillIcon.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            m_bNowWeapon = false;
        });
    }

    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        // m_fDelTime秒でm_MinScaleに変更
        SkillIcon.transform.DOScale(m_MinScale, m_fDelTime);
        // m_fDelTime秒でテクスチャをフェードイン
        OutLine.DOFade(0f, m_fDelTime);
        SkillInner.DOFade(0f, m_fDelTime);
        SkillIcon.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            m_bNowWeapon = true;
        });
    }

    public void UseSkill()
    {
        m_bSkillUse = true;
        SkillInner.fillAmount = 1.0f;
        m_fCoolTime = 0.0f;
    }

    public int GetSetStuck
    {
        get { return m_nStuck; }
        set { m_nStuck = value; }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Cursol")
        {
            OutLine.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        OutLine.enabled = false;
    }
    /**
* @fn
* スキル使用のgetter
* @return m_bSkillUse(bool)
* @brief 使ってるかどうかを返す
*/
    public bool GetUseSkill
    {
        get { return m_bSkillUse; }
    }

}
