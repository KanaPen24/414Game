/**
 * @file YK_TitleBack.cs
 * @brief TitleBackUIの処理
 * 
 * TitleBackUIの表示と非表示、およびシーン遷移を制御するスクリプト
 * このスクリプトはYK_UIクラスを継承しており、UIの基本機能を利用
 *  
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
        m_eUIType = UIType.TitleBack; // UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        // UIが動くようならUpdateにかからない
        GetSetUIPos = TitleBackUI.GetComponent<RectTransform>().anchoredPosition;
        // スケール取得
        GetSetUIScale = TitleBackUI.transform.localScale;
    }

    /**
     * @brief TitleBackUIを表示
     * 
     * TitleBackUIをフェードインさせて表示
     * 0秒でUIのスケールを元の大きさに変更し、テクスチャを不透明に設定
     * フェードインのアニメーションが完了した後、フェード状態を設定
     */
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

    /**
     * @brief TitleBackUIを非表示にする
     * 
     * TitleBackUIをフェードアウトさせて非表示にします。
     * 指定された時間でUIのスケールを最小化し、テクスチャを透明に設定
     * フェードアウトのアニメーションが完了した後、フェード状態を設定
     * TitleBackPlay()関数が呼ばれ、ゲームのステートをスタート状態に設定し、シーン遷移
     */
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
     * @brief TitleBackUIの表示状態のgetter・setter
     * 
     * TitleBackUIの表示状態を取得または設定
     * 
     * @return m_bVisibleTitleBack(bool) TitleBackUIの表示状態
     */
    public bool GetSetVisibleFlg
    {
        get { return m_bVisibleTitleBack; }
        set { m_bVisibleTitleBack = value; }
    }

    /**
     * @brief TitleBackUIの再生処理
     * 
     * ゲームのステートをスタート状態に設定し、BGMを停止した後、指定のシーンにトランジション効果を掛けて遷移
     * シーン遷移時にはFadeクラスのFadeIn()関数を使用
     */
    public void TitleBackPlay()
    {
        // ゲームのステートをスタート状態にする
        GameManager.instance.GetSetGameState = GameState.GameStart;
        // トランジションを掛けてシーン遷移する
        fade.FadeIn(1f, () =>
        {
            IS_AudioManager.instance.StopBGM(BGMType.BGM_GAMEOVER);
            SceneManager.LoadScene("GameScene");
        });
    }
}


