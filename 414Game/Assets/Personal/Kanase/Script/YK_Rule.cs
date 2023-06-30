/**
 * @file YK_Rule.cs
 * @brief ルール画面の表示と非表示を管理するスクリプト
 * @author 吉田叶聖
 * @date 2023/05/28
 * @Update 2023/06/23 入力変更(Ihara)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class YK_Rule : MonoBehaviour
{
    [SerializeField] Image Rule; // ルール画面のイメージ
    [SerializeField] YK_Book book; // ブックオブジェクト参照
    private Vector2 StartPos; // ルール画面の初期位置
    private bool m_bRuleVisible = false; // ルール画面の表示状態

    /**
     * @brief 初期化処理
     */
    private void Start()
    {
        Rule.DOFade(0f, 0f);
        StartPos = Rule.rectTransform.anchoredPosition;
    }

    /**
     * @brief フレームごとの更新処理
     */
    private void Update()
    {
        // ブックのアイテムヒットフラグが立っていた場合、ルール画面を表示する
        if (book.GetSetItemHit)
        {
            FadeIN();
            book.GetSetItemHit = false;
        }

        // ルール画面が表示されている状態でLBかRBが押された場合、ルール画面を非表示にする
        if (m_bRuleVisible && (Input.GetKeyDown(IS_XBoxInput.LB) || Input.GetKeyDown(IS_XBoxInput.RB))) 
            FadeOUT();
    }

    /**
     * @brief ルール画面を表示する処理
     */
    public void FadeIN()
    {
        GameManager.instance.GetSetGameState = GameState.GameRule;
        IS_Player.instance.GetSetPlayerState = PlayerState.PlayerWait;
        // 開く音を再生する
        IS_AudioManager.instance.PlaySE(SEType.SE_BookOpen);
        // 2秒でルール画面の位置を目標位置に移動させる
        Rule.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 0f), 1f);
        // 2秒でルール画面の透明度をフェードインさせる
        Rule.DOFade(1f, 1f).OnComplete(() =>
        {

        });
        m_bRuleVisible = true;
    }

    /**
     * @brief ルール画面を非表示にする処理
     */
    public void FadeOUT()
    {
        // 閉じる音を再生する
        IS_AudioManager.instance.PlaySE(SEType.SE_BookClose);
        // 0.5秒でルール画面を元の位置に戻す
        Rule.GetComponent<RectTransform>().DOAnchorPos(-StartPos, 0.5f);
        // 0.5秒でルール画面の透明度をフェードアウトさせる
        Rule.DOFade(0f, 0.5f).OnComplete(() =>
        {
            GameManager.instance.GetSetGameState = GameState.GamePlay;
        });
        m_bRuleVisible = false;
    }
}
