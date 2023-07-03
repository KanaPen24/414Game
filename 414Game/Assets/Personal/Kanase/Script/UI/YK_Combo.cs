/**
 * @file YK_Combo.cs
 * @brief コンボUIの処理を行うクラス
 *        YK_UIクラスを継承
 *        コンボの数値表示や色の変更、コンボの加算やリセットなどの処理
 *        DOTweenパッケージを使用
 * @author 吉田叶聖
 * @date   2023/05/15
 * @Update 2023/06/30 一部変数,関数を静的化しました(Ihara)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class YK_Combo : YK_UI
{
    // --- 静的メンバ(AddComboを外部で使用するため) ---
    static private Text ComboNumber;   　      // コンボの数値表示テキスト
    static private int m_Combo;                // 現在のコンボ数
    static private float a_color = 0f;         // コンボ表示のアルファ値
    static private int m_nCountComboTime = 0;  // コンボが表示されている時間
    static private bool m_bHitFlg = false;     // コンボがヒットしたかどうかのフラグ
    // -----------------------------------------------------------------------------------

    // --- 動的メンバ ---
    [SerializeField] private Image ComboTxt;      // コンボの画像
    private float f_colordown = 0.016f;           // コンボ表示のアルファ値減少量
    private int ComboS = 0;                       // 小コンボの閾値
    private int ComboM = 5;                       // 中コンボの閾値
    private int ComboL = 10;                      // 大コンボの閾値
    private int ComboXL = 15;                     // 特大コンボの閾値
    [SerializeField] private int m_nCountDownTime;    // コンボが消えるまでの時間（秒単位）
    private Vector3 Combo_Scale;                  // コンボ表示の初期スケール
    // ------------------------------------------------------------------------------------

    // Start is called before the first frame update
    /**
     * @fn
     * スタート時に呼ばれる関数
     * 初期化処理
     */
    void Start()
    {
        m_eUIType = UIType.Combo;                                 // UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        ComboNumber = GetComponentInChildren<Text>();
        GetSetUIPos = ComboNumber.GetComponent<RectTransform>().anchoredPosition;    // UIの座標取得
        GetSetUIScale = ComboNumber.transform.localScale;                           // UIのスケール取得
        Combo_Scale = ComboTxt.transform.localScale;                                // コンボ表示の初期スケール取得
        ComboNumber = GetComponent<Text>();                                         // Textコンポーネントの取得
        m_nCountDownTime *= 60;                                                     // 60FPSに合わせる
        a_color = 0.0f;                                                              // 最初は消しておく
        ComboNumber.color = new Color(0.5f, 0.5f, 1f, a_color);                       // コンボ表示の色を設定
        ComboTxt.color = new Color(0.5f, 0.5f, 1f, a_color);                          // コンボテキストの色を設定
    }

    // Update is called once per frame
    /**
     * @fn
     * フレームごとに呼ばれる関数
     * コンボの更新や表示の色を変更
     */
    void Update()
    {
        if (GameManager.instance.GetSetGameState != GameState.GamePlay)    // ゲームがプレイ中または武器中以外は更新しない
            return;

        if (Input.GetKeyDown(KeyCode.F2))    // F2キーが押されたらコンボを加算する
        {
            AddCombo();
        }

        if (m_Combo == 0)    // コンボが0の場合、表示のアルファ値を0にする
            a_color = 0.0f;

        if (m_bHitFlg)    // コンボがヒットした場合
        {
            m_nCountComboTime++;
            a_color -= 1f / m_nCountDownTime;    // アルファ値を徐々に減少させる
            if (m_nCountComboTime >= m_nCountDownTime)
            {
                m_bHitFlg = false;
                ResetCombo();    // カウントダウン時間を超えたらコンボをリセットする
            }
        }

        //初期カラー
        ComboNumber.color = new Color(0.5f, 0.5f, 1f, a_color);    // コンボ表示の色を設定
        ComboTxt.color = new Color(0.5f, 0.5f, 1f, a_color);       // コンボテキストの色を設定

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
     * @fn
     * UIのフェードイン処理を行う関数
     */
    public override void UIFadeIN()
    {
        m_eFadeState = FadeState.FadeIN;
        this.gameObject.transform.DOScale(GetSetUIScale, 0f);    // 0秒で後X,Y方向を元の大きさに変更
        ComboTxt.transform.DOScale(Combo_Scale, 0f);
        ComboNumber.DOFade(1f, 0f);    // 0秒でテクスチャをフェードイン
        ComboTxt.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeIN終了");
        });
    }

    /**
     * @fn
     * UIのフェードアウト処理を行う関数
     */
    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        this.gameObject.transform.DOScale(m_MinScale, m_fDelTime);    // m_fDelTime秒でm_MinScaleに変更
        ComboTxt.transform.DOScale(m_MinScale, m_fDelTime);
        ComboNumber.DOFade(0f, m_fDelTime);    // m_fDelTime秒でテクスチャをフェードイン
        ComboTxt.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeOUT終了");
        });
    }

    /**
     * @fn
     * コンボを加算する静的関数
     */
    public static void AddCombo()
    {
        m_nCountComboTime = 0;
        m_bHitFlg = true;
        a_color = 1.0f;
        m_Combo++;
        ComboNumber.text = m_Combo + "";
    }

    /**
     * @fn
     * コンボをリセットする関数
     */
    public void ResetCombo()
    {
        m_Combo = 0;
    }
}
