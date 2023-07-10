/**
 * @file YK_SkillIcon.cs
 * @brief スキルのアイコン処理を管理するクラスです。
 *        スキルのアイコンの表示と非表示、スキルのクールダウン処理を行います。
 *        スキルアイコンの使用状態を取得するプロパティがあります。
 *        DOTweenライブラリを使用します。
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
    [SerializeField] private float m_fJumpPower = 10.0f;    //跳ぶ力
    private int m_nJump = 1;    //跳ぶ回数
    [SerializeField] private float m_fJumpTime = 0.3f;      //跳ぶ時間

    /**
     * @brief Start関数
     *        UIのタイプを設定し、アウトラインを非表示
     *        スキルアイコンの座標とスケールを取得
     */
    private void Start()
    {
        m_eUIType = UIType.SkillIcon;   //UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        OutLine.enabled = false;
        //座標取得
        GetSetUIPos = SkillIcon.GetComponent<RectTransform>().anchoredPosition;
        //スケール取得
        GetSetUIScale = SkillIcon.transform.localScale;
    }

    /**
     * @brief Update関数
     *        カーソルが動き始めるまで当たり判定を無効にし、エフェクターを無効
     *        ストック数を制限し、ストック数が0になったらアイコンを非表示
     *        スキルが使われていて、かつ武器化中でない場合はクールダウン処理
     */
    private void Update()
    {
        //カーソルが動き始めるまで
        if (!MoveCursol.GetSetMoveFlg)
        {
            GetComponent<BoxCollider2D>().enabled = false;
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
                    HealEffect();
                }
            }
        }
    }

    /**
     * @brief HealEffect関数は回復エフェクトを再生し、アイコンを跳ねさせる処理
     */
    public void HealEffect()
    {
        HealParticle.Play();
        RectTransform recttran = this.GetComponent<RectTransform>();
        Vector2 originalPos = recttran.anchoredPosition;
        recttran.DOJumpAnchorPos(originalPos, 10f, 1, 0.3f, true);
    }

    /**
     * @brief UIFadeIN関数はUIのフェードイン処理
     *        スキルアイコンのスケールと透明度を元の状態にする
     */
    public override void UIFadeIN()
    {
        m_eFadeState = FadeState.FadeIN;
        // 0秒で後X,Y方向を元の大きさに変更
        SkillIcon.transform.DOScale(GetSetUIScale, 0f);
        // 0秒でテクスチャをフェードイン
        OutLine.DOFade(1f, 0f);
        SkillInner.DOFade(0.7f, 0f);
        SkillIcon.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            m_bNowWeapon = false;
        });
    }

    /**
     * @brief UIFadeOUT関数はUIのフェードアウト処理を行う
     *        スキルアイコンのスケールを最小値に変更し、透明度を0
     */
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

    /**
     * @brief UseSkill関数はスキルを使用し、クールダウンを開始
     */
    public void UseSkill()
    {
        m_bSkillUse = true;
        SkillInner.fillAmount = 1.0f;
        m_fCoolTime = 0.0f;
    }

    /**
     * @brief GetSetStuckプロパティはストック数を取得および設定
     */
    public int GetSetStuck
    {
        get { return m_nStuck; }
        set { m_nStuck = value; }
    }

    /**
     * @brief OnTriggerEnter2D関数は当たり判定が有効になったときに呼び出さる
     *        当たり判定の対象がカーソルである場合、アウトラインを表示
     * @param collision 当たり判定の相手のCollider2D
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Cursol")
        {
            OutLine.enabled = true;
        }
    }

    /**
     * @brief OnTriggerExit2D関数は当たり判定が無効になったときに呼び出される
     *        アウトラインを非表示にします。
     * @param collision 当たり判定の相手のCollider2D
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        OutLine.enabled = false;
    }

    /**
     * @brief GetSetSkillUseプロパティはスキルの使用状態を取得
     */
    public bool GetSkillUse
    {
        get { return m_bSkillUse; }
    }

    /**
     * @brief GetSetNowWeaponプロパティは武器化中かどうかを取得
     */
    public bool GetNowWeapon
    {
        get { return m_bNowWeapon; }
    }
}
