/**
 * @file YK_Retry.cs
 * @brief RetryUIの処理
 * @author 吉田叶聖
 * @date 2023/05/02
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using DG.Tweening;

public class YK_Start : YK_UI
{
    [SerializeField] private GameObject GameStart;
    [SerializeField] private Image StartUI;
    [SerializeField] private Image ExitUI;
    [SerializeField] private Image TitleUI;
    [SerializeField] ON_VolumeManager PostEffect; // ポストエフェクト
    private Outline outline;
    private bool m_bVisibleStart = true;
    private float m_rate = 1.0f;
    private float m_fTime;
    [SerializeField] private YK_MoveCursol MoveCursol;

    // Start is called before the first frame update
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
 * @fn
 * 表示非表示のgetter・setter
 * @return m_bVisibleStart(bool)
 * @brief 表示非表示処理
 */
    public bool GetSetVisibleFlg
    {
        get { return m_bVisibleStart; }
        set { m_bVisibleStart = value; }
    }

    public void StartPlay()
    {
        m_bVisibleStart = false;
    }
    


}