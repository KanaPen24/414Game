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


public class YK_Exit : YK_UI
{
    [SerializeField] private Image Exit;    
    private bool m_bVisibleRetry = false;
    [SerializeField] private YK_MoveCursol MoveCursol;

    // Start is called before the first frame update
    void Start()
    {
        m_eUIType = UIType.Exit; //UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        //UIが動くようならUpdateにかかなかん
        GetSetPos = Exit.GetComponent<RectTransform>().anchoredPosition;
        //スケール取得
        GetSetScale = Exit.transform.localScale;
    }

    private void Update()
    {
        // カーソルが動き始めるまでとゲームがプレイ中以外は更新しない
        if (!MoveCursol.GetSetMoveFlg || GameManager.instance.GetSetGameState != GameState.GameStart) 
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
    }

    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        // m_fDelTime秒でm_MinScaleに変更
        Exit.transform.DOScale(m_MinScale, m_fDelTime);
        // m_fDelTime秒でテクスチャをフェードアウト
        Exit.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            //フェード処理終了時に呼ばれる
            GetSetFadeState = FadeState.FadeNone;
            ExitPlay();
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
        get { return m_bVisibleRetry; }
        set { m_bVisibleRetry = value; }
    }

    public void ExitPlay()
    {
        //  終了処理
        Application.Quit();             //ゲーム終了処理
    }

}