/**
 * @file YK_Start.cs
 * @brief StartUIの処理
 * @author 吉田叶聖
 * @date 2023/05/02
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class YK_Start : YK_UI
{
    [SerializeField] private GameObject GameStart;   // ゲームスタートオブジェクト
    [SerializeField] private Image StartUI;          // スタートUIイメージ
    [SerializeField] private Image ExitUI;           // 終了UIイメージ
    [SerializeField] private Image TitleUI;          // タイトルUIイメージ
    [SerializeField] ON_VolumeManager PostEffect;    // ポストエフェクト
    private Outline outline;                         // アウトライン
    private bool m_bVisibleStart = true;              // スタートUIの表示フラグ
    private float m_rate = 1.0f;                      // ポストエフェクトの割合
    private float m_fTime;                            // 経過時間
    [SerializeField] private YK_MoveCursol MoveCursol; // カーソル移動制御オブジェクト

    /**
     * @brief Start関数
     *        UIのタイプを設定し、アウトラインを非表示
     *        スタートの座標とスケールを取得
     */
    void Start()
    {
        m_eUIType = UIType.Start; //UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        //UIが動くようならUpdateにかかなかん
        GetSetUIPos = StartUI.GetComponent<RectTransform>().anchoredPosition;
        //スケール取得
        GetSetUIScale = StartUI.transform.localScale;
        //アウトライン取得
        outline = this.GetComponent<Outline>();

        GameStart.SetActive(true);
    }
    /**
     * @brief Update関数
     *        カーソルが動き始めるまで当たり判定を無効にし、エフェクターを無効
     *        ブラウン管のポストエフェクトを減らしていく処理
     */
    private void Update()
    {
        // カーソルが動き始めるまで
        if (!MoveCursol.GetSetMoveFlg)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<PointEffector2D>().enabled = false;    //エフェクターを無効にすることで道中吸い寄せられない
            return;
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<PointEffector2D>().enabled = true;
        }

        //ブラウン管のポストエフェクトを減らしていく処理
        if (!m_bVisibleStart)
        {
            m_fTime += Time.deltaTime;
            m_rate = Mathf.Lerp(1.0f, 0.0f, m_fTime);
            PostEffect.SetBraunRate(m_rate);
        }
    }

    //StartUIを表示
    public override void UIFadeIN()
    {
        m_eFadeState = FadeState.FadeIN;
        // 0秒で後X,Y方向を元の大きさに変更
        StartUI.transform.DOScale(GetSetUIScale, 0f);
        // 0秒でテクスチャをフェードイン
        StartUI.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeIN終了");
        });
    }

    //StartUIを非表示
    public override void UIFadeOUT()
    {
        //ゲームのステートをプレイ状態にする
        GameManager.instance.GetSetGameState = GameState.GamePlay;
        m_eFadeState = FadeState.FadeOUT;
        // m_fDelTime秒でm_MinScaleに変更
        StartUI.transform.DOScale(m_MinScale, m_fDelTime);
        // m_fDelTime秒でテクスチャをフェードアウト
        StartUI.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            //フェード処理終了時に呼ばれる
            GetSetFadeState = FadeState.FadeNone;
            StartPlay();
            ExitUI.DOFade(0f, 1.0f);
            TitleUI.DOFade(0f, 1.0f).OnComplete(() =>
            {
                GameStart.SetActive(false);
            });
            Debug.Log("FadeOUT終了");
        });
    }

    /**
     * @fn GetSetVisibleFlg
     * @brief 表示非表示のgetter・setter
     * @return m_bVisibleStart(bool)
     */
    public bool GetSetVisibleFlg
    {
        get { return m_bVisibleStart; }
        set { m_bVisibleStart = value; }
    }

    // ゲームスタート処理
    public void StartPlay()
    {
        m_bVisibleStart = false;
    }
}
