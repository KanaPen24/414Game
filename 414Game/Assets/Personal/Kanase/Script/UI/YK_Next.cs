/**
 * @file YK_Next.cs
 * @brief NextUIの処理を管理するクラス
 *        ゲームクリア時に呼び出され、UIのフェードインとフェードアウト、シーンの切り替えを行う
 *        ゲームクリアフラグを管理
 *        フェードイン時にBoxCollider2DとPointEffector2Dを有効にし、フェードアウト時に無効
 *        ゲームクリア時にBGMを停止し、ゲームシーンを再読み込み
 * @author 吉田叶聖
 * @date 2023/03/25
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class YK_Next : YK_UI
{
    [SerializeField] private Image Next;
    [SerializeField] private Fade fade;
    private bool m_bClear = false;

    /**
     * @brief Start関数
     *        UIのタイプを設定し、UIの位置とスケールを初期化
     *        UIFadeOUT関数を呼び出し、UIをフェードアウト
     */
    void Start()
    {
        m_eUIType = UIType.Next; // UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        GetSetUIPos = Next.GetComponent<RectTransform>().anchoredPosition; // UIの位置を取得
        GetSetUIScale = Next.transform.localScale; // UIのスケールを取得
        UIFadeOUT(); // UIをフェードアウトさせる
    }

    /**
     * @brief UIFadeIN関数はUIのフェードイン処理
     *        BoxCollider2DとPointEffector2Dを有効にし、UIのスケールとアルファ値を変化させてフェードイン
     *        フェードインが完了したらゲームクリアフラグを設定
     */
    public override void UIFadeIN()
    {
        GetComponent<BoxCollider2D>().enabled = true;   // BoxCollider2Dを有効にする
        GetComponent<PointEffector2D>().enabled = true; // PointEffector2Dを有効にする
        m_eFadeState = FadeState.FadeIN;
        Next.transform.DOScale(GetSetUIScale, 0f);      // UIのスケールを変更して元の大きさに戻す
        Next.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone; // フェード処理終了時にフェード状態をリセット
            m_bClear = true; // ゲームクリアフラグを設定
        });
    }

    /**
     * @brief UIFadeOUT関数はUIのフェードアウト処理を行う
     *        UIのスケールとアルファ値を変化させてフェードアウト
     *        フェードアウトが完了したらBoxCollider2DとPointEffector2Dを無効にし、
     *        ゲームクリアフラグが設定されている場合はBGMを停止してゲームシーンを再読み込み
     */
    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        Next.transform.DOScale(m_MinScale, m_fDelTime);         // UIのスケールを変更して縮小する
        Next.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;               // フェード処理終了時にフェード状態をリセット
            GetComponent<BoxCollider2D>().enabled = false;      // BoxCollider2Dを無効にする
            GetComponent<PointEffector2D>().enabled = false;    // PointEffector2Dを無効にする
            if (m_bClear)
            {
                IS_AudioManager.instance.StopBGM(BGMType.BGM_Game); // BGMを停止する
                SceneManager.LoadScene("GameScene");                // ゲームシーンを再読み込みする
            }
        });
    }
}
