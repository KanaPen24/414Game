/**
 * @file YK_TitleBack.cs
 * @brief TitleBackUIの処理
 * @author 吉田叶聖
 * @date 2023/05/02
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用

public class YK_TitleBack : YK_UI
{
    [SerializeField] Fade fade;
    [SerializeField] private Image TitleBackUI;
    private bool m_bVisibleTitleBack = false;
    // Start is called before the first frame update
    void Start()
    {
        m_eUIType = UIType.TitleBack; //UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        //UIが動くようならUpdateにかかなかん
        GetSetUIPos = TitleBackUI.GetComponent<RectTransform>().anchoredPosition;
        //スケール取得
        GetSetUIScale = TitleBackUI.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    //TitleBackUIを表示
    public override void UIFadeIN()
    {
        m_eFadeState = FadeState.FadeIN;
        // 0秒で後X,Y方向を元の大きさに変更
        TitleBackUI.transform.DOScale(GetSetUIScale, 0f);
        // 0秒でテクスチャをフェードイン
        TitleBackUI.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeIN終了");
        });
    }

    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        // m_fDelTime秒でm_MinScaleに変更
        TitleBackUI.transform.DOScale(m_MinScale, m_fDelTime);
        // m_fDelTime秒でテクスチャをフェードイン
        TitleBackUI.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeOUT終了");
        });
        TitleBackPlay();
    }
    /**
 * @fn
 * 表示非表示のgetter・setter
 * @return m_bVisibleTitleBack(bool)
 * @brief 表示非表示処理
 */
    public bool GetSetVisibleFlg
    {
        get { return m_bVisibleTitleBack; }
        set { m_bVisibleTitleBack = value; }
    }
    public void TitleBackPlay()
    {
        //ゲームのステートをスタート状態にする
        GameManager.instance.GetSetGameState = GameState.GameStart;
        //トランジションを掛けてシーン遷移する
        fade.FadeIn(1f, () =>
        {
            IS_AudioManager.instance.StopBGM(BGMType.BGM_GAMEOVER);
            SceneManager.LoadScene("GameScene");
        });
    }
}