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
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用

public class YK_Retry : YK_UI
{
    [SerializeField] Fade fade;
    [SerializeField] private Image RetryUI;
    [SerializeField] private Vector3 m_MinScale = new Vector3(0.5f, 0.5f, 0.5f); // 最小サイズ
    [SerializeField] private float m_fDelTime = 0.5f; // 減算していく時間
    private bool m_bVisibleRetry = false;
    // Start is called before the first frame update
    void Start()
    {
        m_eUIType = UIType.Retry; //UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        //UIが動くようならUpdateにかかなかん
        GetSetPos = RetryUI.GetComponent<RectTransform>().anchoredPosition;
        //スケール取得
        GetSetScale = RetryUI.transform.localScale;
    }
    
    //RetryUIを表示
    public override void UIFadeIN()
    {
        m_eFadeState = FadeState.FadeIN;
        // 1秒で後X,Y方向を元の大きさに変更
        RetryUI.transform.DOScale(GetSetScale, 0f);
        // 1秒でテクスチャをフェードイン
        RetryUI.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeIN終了");
        });
    }

    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        // 1秒で後X,Y方向を0.5倍に変更
        RetryUI.transform.DOScale(m_MinScale, m_fDelTime);
        // 1秒でテクスチャをフェードアウト
        RetryUI.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            //フェード処理終了時に呼ばれる
            GetSetFadeState = FadeState.FadeNone;
            RetryPlay();
            Debug.Log("FadeOUT終了");
        });
        
    }
    /**
 * @fn
 * 表示非表示のgetter・setter
 * @return m_bVisibleRetry(bool)
 * @brief 表示非表示処理
 */
    public bool GetSetVisibleFlg
    {
        get { return m_bVisibleRetry; }
        set { m_bVisibleRetry = value; }
    }

    public void RetryPlay()
    {
        //トランジションを掛けてシーン遷移する
        fade.FadeIn(1f, () =>
        {
            IS_AudioManager.instance.StopBGM(BGMType.BGM_GAMEOVER);
            SceneManager.LoadScene("GameScene");
        });
    }
}