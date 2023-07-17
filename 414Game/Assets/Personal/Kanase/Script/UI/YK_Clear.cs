/**
 * @file YK_Clear.cs
 * @brief クリアUIの処理を行うクラス
 *        YK_UIクラスを継。
 *        フェードイン・フェードアウトのアニメーションや画像の表示・非表示の制御
 *        クリア時にゲームをリロードする処理
 *        DOTweenパッケージを使用しています 
 * @author 吉田叶聖
 * @date   2023/03/25
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class YK_Clear : YK_UI
{
    [SerializeField] private Image Clear;    // クリア画像
    [SerializeField] private Fade fade;      // フェード
    private bool m_bClear = false;           // クリアフラグ

    // Start is called before the first frame update
    void Start()
    {
        m_eUIType = UIType.Clear;                     // UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        GetSetUIPos = Clear.GetComponent<RectTransform>().anchoredPosition;   // UIの座標取得
        GetSetUIScale = Clear.transform.localScale;                           // UIのスケール取得
        UIFadeOUT();
    }

    /**
     * @fn
     * Clear画像をフェードインさせる処理
     * @brief Clear画像のフェードイン処理
     *        フェードイン完了後にクリアフラグを立てる
     */
    public override void UIFadeIN()
    {
        GetComponent<BoxCollider2D>().enabled = true;             // ボックスコライダーを有効にする
        GetComponent<PointEffector2D>().enabled = true;           // ポイントエフェクターを有効にする
        m_eFadeState = FadeState.FadeIN;                          // フェード状態をフェードインに設定

        // 1秒で後X,Y方向を元の大きさに変更
        Clear.transform.DOScale(GetSetUIScale, 0f);

        // 1秒でテクスチャをフェードイン
        Clear.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;                 // フェード状態をフェードなしに設定
            m_bClear = true;                                      // クリアフラグを立てる
        });
    }

    /**
     * @fn
     * Clear画像をフェードアウトさせる処理
     * @brief Clear画像のフェードアウト処理
     *        フェードアウト完了後にゲームをリロード
     */
    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;                         // フェード状態をフェードアウトに設定

        // 1秒で後X,Y方向を0.5倍に変更
        Clear.transform.DOScale(m_MinScale, m_fDelTime);

        // 1秒でテクスチャをフェードアウト
        Clear.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;                  // フェード状態をフェードなしに設定
            GetComponent<BoxCollider2D>().enabled = false;         // ボックスコライダーを無効にする
            GetComponent<PointEffector2D>().enabled = false;       // ポイントエフェクターを無効にする

            if (m_bClear)
            {
                IS_AudioManager.instance.StopBGM(BGMType.BGM_Game);   // BGMを停止
                SceneManager.LoadScene("GameScene");                  // ゲームシーンをリロード
                YK_JsonSave.instance.DelFile();                       // セーブデータの削除
            }
        });
    }
}
