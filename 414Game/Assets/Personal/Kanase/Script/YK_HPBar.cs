/**
 * @file   YK_HPBar.cs
 * @brief  体力バーを制御するためのスクリプト
 *         UIのフェードイン・フェードアウトや衝突判定などの機能を提供
 * @author 吉田叶聖
 *         スクリプトの作成者
 * @date   2023/03/17
 *         初版作成日
 * @date   2023/04/21
 *         画像追加に伴う処理の追加が行われた日付
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/**
 * @class YK_HPBar
 * @brief 体力バーを制御するクラス
 */
public class YK_HPBar : YK_UI
{
    [SerializeField] Slider HP;                                  // 体力バーのSliderコンポーネント
    [SerializeField] private Image FrontFill;                    // バーの表面のテクスチャ
    [SerializeField] private Image BackFill;                     // 後ろのバーの表面のテクスチャ
    [SerializeField] private Image Frame;                        // フレーム
    [SerializeField] private Image Crack;                        // ヒビの画像
    [SerializeField] private Image Refraction;                   // 反射光
    [SerializeField] private Image OutLine;                      // アウトライン    
    [SerializeField] private YK_MoveCursol MoveCursol;

    /**
     * @brief スタート時に呼ばれる関数
     *        初期化処理を行う
     */
    void Start()
    {
        m_eUIType = UIType.HPBar;                                 // UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        GetSetVisible = false;
        OutLine.enabled = false;
        // UIが動くようならUpdateに書かない
        GetSetPos = HP.GetComponent<RectTransform>().anchoredPosition;  // 位置取得
        // スケール取得
        GetSetScale = HP.transform.localScale;                    // スケール取得
    }

    private void Update()
    {
        //カーソルが動き始めるまで
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
    }
    /**
     * @brief UIのフェードイン処理
     *        バーとテクスチャのフェードインを行う
     */
    public override void UIFadeIN()
    {
        m_eFadeState = FadeState.FadeIN;
        // 0秒で後X,Y方向を元の大きさに変更
        HP.transform.DOScale(GetSetScale, 0f);
        // 0秒でテクスチャをフェードイン
        FrontFill.DOFade(1f, 0f);
        BackFill.DOFade(1f, 0f);
        Crack.DOFade(1f, 0f);
        Refraction.DOFade(1f, 0f);
        OutLine.DOFade(1f, 0f);
        Frame.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeIN終了");
        });
    }

    /**
     * @brief UIのフェードアウト処理
     *        バーとテクスチャのフェードアウトを行う
     */
    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        // m_fDelTime秒でm_MinScaleに変更
        HP.transform.DOScale(m_MinScale, m_fDelTime);
        // m_fDelTime秒でテクスチャをフェードイン
        FrontFill.DOFade(0f, m_fDelTime);
        BackFill.DOFade(0f, m_fDelTime);
        Crack.DOFade(0f, m_fDelTime);
        Refraction.DOFade(0f, m_fDelTime);
        OutLine.DOFade(0f, m_fDelTime);
        Frame.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeOUT終了");
        });
    }

    /**
     * @brief 衝突判定時に呼ばれる関数
     *        衝突したオブジェクトがカーソルの場合、アウトラインを有効にする
     * @param collision 衝突したオブジェクトのコライダー情報
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Cursol")
        {
            OutLine.enabled = true;
        }
    }

    /**
     * @brief 衝突判定から離れた時に呼ばれる関数
     *        アウトラインを無効にする
     * @param collision 衝突したオブジェクトのコライダー情報
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        OutLine.enabled = false;
    }
}
