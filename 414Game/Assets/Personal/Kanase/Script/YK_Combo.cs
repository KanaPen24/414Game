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


/**
 * @class YK_Combo
 * @brief コンボUIの処理を行うクラス
 */
public class YK_Combo: YK_UI
{
    //---　スクリプト内で使用する変数 ---
    [SerializeField] private Text ComboNumber;    // コンボの数値表示テキスト
    [SerializeField] private Text ComboTxt;       // コンボのテキスト表示
    private int m_Combo;                          // 現在のコンボ数
    private float a_color = 0f;                   // コンボ表示のアルファ値
    private float f_colordown = 0.016f;           // コンボ表示のアルファ値減少量
    private int ComboS = 0;                       // 小コンボの閾値
    private int ComboM = 5;                       // 中コンボの閾値
    private int ComboL = 10;                      // 大コンボの閾値
    private int ComboXL = 15;                     // 特大コンボの閾値
    [SerializeField] private int m_nCountDownTime;    // コンボが消えるまでの時間（秒単位）
    private int m_nCountComboTime = 0;            // コンボが表示されている時間
    private bool m_bHitFlg = false;               // コンボがヒットしたかどうかのフラグ
    private Vector3 Combo_Scale;                  // コンボ表示の初期スケール
    private Vector3 m_MinScale = new Vector3(0.5f, 0.5f, 0.0f); // 最小スケール
    private float m_fDelTime = 0.3f;              // スケール変更にかかる時間

    // Start is called before the first frame update
    /**
     * @brief スタート時に呼ばれる関数
     *        初期化処理を行う
     */
    void Start()
    {
        m_eUIType = UIType.Combo; //UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        GetSetPos = ComboNumber.GetComponent<RectTransform>().anchoredPosition; //UIが動くようならUpdateにかかなかん
        GetSetScale = ComboNumber.transform.localScale; //スケール取得
        Combo_Scale = ComboTxt.transform.localScale;
        ComboNumber = GetComponent<Text>();
        m_nCountDownTime *= 60; //60FPSに合わせる
        a_color = 0.0f; //最初は消しておく
        ComboNumber.color = new Color(0.5f, 0.5f, 1f, a_color);
        ComboTxt.color = new Color(0.5f, 0.5f, 1f, a_color);
    }

    // Update is called once per frame
    /**
     * @brief フレームごとに呼ばれる関数
     *        コンボの更新や表示の色を変更する
     */
    void Update()
    {
        // ゲームがプレイ中または武器中以外は更新しない
        if (GameManager.instance.GetSetGameState != GameState.GamePlay)
            return; 

        if (Input.GetKeyDown(KeyCode.F2))
        {
            AddCombo(); // F2キーが押されたらコンボを加算する
        }

        if (m_Combo == 0)
            a_color = 0.0f;

        if (m_bHitFlg)
        {
            m_nCountComboTime++;
            a_color -= 1f / m_nCountDownTime;
            if (m_nCountComboTime >= m_nCountDownTime)
            {
                m_bHitFlg = false;
                ResetCombo(); // カウントダウン時間を超えたらコンボをリセットする
                m_nCountComboTime = 0;
            }
        }

        ComboNumber.color = new Color(0.5f, 0.5f, 1f, a_color);
        ComboTxt.color = new Color(0.5f, 0.5f, 1f, a_color);

        // コンボがComboM以上ComboL未満の場合、色を変更する
        if (m_Combo >= ComboM && m_Combo < ComboL)
        {
            ComboNumber.color = new Color(1f, 1f, 0.5f, a_color); 
            ComboTxt.color = new Color(1f, 1f, 0.5f, a_color);
        }

        // コンボがComboL以上ComboXL未満の場合、色を変更する
        if (m_Combo >= ComboL && m_Combo < ComboXL)
        {
            ComboNumber.color = new Color(1f, 0.5f, 0.5f, a_color); 
            ComboTxt.color = new Color(1f, 0.5f, 0.5f, a_color);
        }

        // コンボがComboXL以上の場合、色をHSVカラーモードで変更する
        if (m_Combo >= ComboXL)
        {
            ComboNumber.color = Color.HSVToRGB(Time.time % 1, 1, 1); 
            ComboTxt.color = Color.HSVToRGB(Time.time % 1, 1, 1);
        }
    }


    /**
     * @brief UIのフェードイン処理
     */
    public override void UIFadeIN()
    {
        m_eFadeState = FadeState.FadeIN;
        this.gameObject.transform.DOScale(GetSetScale, 0f); // 0秒で後X,Y方向を元の大きさに変更
        ComboTxt.transform.DOScale(Combo_Scale, 0f);
        ComboNumber.DOFade(1f, 0f); //0秒でテクスチャをフェードイン
        ComboTxt.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeIN終了");
        });
    }

    /**
     * @brief UIのフェードアウト処理
     */
    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        this.gameObject.transform.DOScale(m_MinScale, m_fDelTime); // m_fDelTime秒でm_MinScaleに変更
        ComboTxt.transform.DOScale(m_MinScale, m_fDelTime);
        ComboNumber.DOFade(0f, m_fDelTime); // m_fDelTime秒でテクスチャをフェードイン
        ComboTxt.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeOUT終了");
        });
    }

    /**
     * @brief コンボを加算する関数
     */
    public void AddCombo()
    {
        m_bHitFlg = true;
        a_color = 1.0f;
        m_Combo++;
        ComboNumber.text = m_Combo + "";
    }

    /**
     * @brief コンボをリセットする関数
     */
    public void ResetCombo()
    {
        m_Combo = 0;
    }
    }
