/**
 * @file YK_Combo.cs
 * @brief コンボUIの処理
 * @author 吉田叶聖
 * @date 2023/05/15
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//↓このスクリプトで得点の計算を行うのではなく、「得点の表示」を行う。
using UnityEngine.UI;
using DG.Tweening;


public class YK_Combo: YK_UI
{
    //---　スクリプト内で使用する変数
    [SerializeField] private Text ComboNumber;
    [SerializeField] private Text ComboTxt;
    private YK_Combo instance;
    private int m_Combo;
    private float a_color = 0f;
    private float f_colordown = 0.016f;
    private int ComboS = 0;
    private int ComboM = 5;
    private int ComboL = 10;
    private int ComboXL = 15;
    [SerializeField] private float m_fCountDownTime;    //何秒で消すか
    private int m_nCountComboTime = 0;
    private bool m_bHitFlg = false;
    private Vector3 Combo_Scale;
    private Vector3 m_MinScale = new Vector3(0.5f, 0.5f, 0.0f); // 最小サイズ
    private float m_fDelTime = 0.3f; // 減算していく時間

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_eUIType = UIType.Combo; //UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        //UIが動くようならUpdateにかかなかん
        GetSetPos = ComboNumber.GetComponent<RectTransform>().anchoredPosition;
        //スケール取得
        GetSetScale = ComboNumber.transform.localScale;
        Combo_Scale = ComboTxt.transform.localScale;
        ComboNumber = GetComponent<Text>();
        //60FPSに合わせる
        m_fCountDownTime *= 60f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            AddCombo();
            m_bHitFlg = true;
        }
        if (m_Combo == 0)
            a_color = 0.0f;
        if (m_bHitFlg)
        {
            m_nCountComboTime++;
            a_color -= 1f / m_fCountDownTime;
            if (m_nCountComboTime >= m_fCountDownTime)
            {
                m_bHitFlg = false;
                ResetCombo();
                m_nCountComboTime = 0;
            }
        }

        ComboNumber.color = new Color(0.5f, 0.5f, 1f, a_color);
        ComboTxt.color = new Color(0.5f, 0.5f, 1f, a_color);
        if (m_Combo >= ComboM && m_Combo < ComboL)
        {
            ComboNumber.color = new Color(1, 1f, 0.5f, a_color);
            ComboTxt.color = new Color(1, 1f, 0.5f, a_color);
        }
        if (m_Combo >= ComboL && m_Combo < ComboXL)
        {
            ComboNumber.color = new Color(1f, 0.5f, 0.5f, a_color);
            ComboTxt.color = new Color(1f, 0.5f, 0.5f, a_color);
        }
        if (m_Combo >= ComboXL)
        {
            ComboNumber.color = Color.HSVToRGB(Time.time % 1, 1, 1);
            ComboTxt.color = Color.HSVToRGB(Time.time % 1, 1, 1);
        }
    }

    public override void UIFadeIN()
    {
        m_eFadeState = FadeState.FadeIN;
        // 1秒で後X,Y方向を元の大きさに変更
        this.gameObject.transform.DOScale(GetSetScale, 0f);
        ComboTxt.transform.DOScale(Combo_Scale, 0f);
        // 1秒でテクスチャをフェードイン
        ComboNumber.DOFade(1f, 0f);
        ComboTxt.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeIN終了");
        });
    }

    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        // 1秒で後X,Y方向を0.5倍に変更
        this.gameObject.transform.DOScale(m_MinScale, m_fDelTime);
        ComboTxt.transform.DOScale(m_MinScale, m_fDelTime);
        // 1秒でテクスチャをフェードアウト
        ComboNumber.DOFade(0f, m_fDelTime);
        ComboTxt.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeOUT終了");
        });
    }


    //コンボ加算
    public void AddCombo()
    {
        a_color = 1.0f;
        m_Combo ++ ;
        ComboNumber.text = m_Combo +"";
    }
    //コンボリセット
    public void ResetCombo()
    {
        m_Combo = 0;
    }
    /**
* @fn
* 表示非表示のgetter・setter
* @return m_bTimeCount(int)
* @brief 時止めフラグを返す・セット
*/
    public int GetCombo
    {
        get { return m_Combo; }        
    }
}