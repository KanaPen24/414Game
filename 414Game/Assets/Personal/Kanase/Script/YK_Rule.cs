/**
 * @file YK_Rule.cs
 * @brief ルール画面表示
 * @author 吉田叶聖
 * @date 2023/05/28
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class YK_Rule : MonoBehaviour
{
    [SerializeField] Image Rule;
    [SerializeField] YK_Book book;
    private Vector2 StartPos;
    private bool m_bRuleVisible = false;
    [SerializeField] IS_Player Player;

    private void Start()
    {
        Rule.DOFade(0f, 0f);
        StartPos = Rule.rectTransform.anchoredPosition;
    }

    private void Update()
    {
        if (book.GetSetItemHit)
        {
            FadeIN();
            book.GetSetItemHit = false;
        }
        if (m_bRuleVisible && Input.GetButton("Decision")) 
            FadeOUT();
    }

    //Ruleを表示
    public void FadeIN()
    {
        GameManager.instance.GetSetGameState = GameState.GameRule;
        Player.GetSetPlayerState = PlayerState.PlayerWait;
        //開くの音再生
        IS_AudioManager.instance.PlaySE(SEType.SE_BookOpen);
        // 2秒で後X,Y方向を元の大きさに変更
        Rule.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 0f), 1f);
        // 2秒でテクスチャをフェードイン
        Rule.DOFade(1f, 1f).OnComplete(() =>
        {
           
        });
        m_bRuleVisible = true;
        
    }
    //Ruleを非表示
    public void FadeOUT()
    {
        //閉じるの音再生
        IS_AudioManager.instance.PlaySE(SEType.SE_BookClose);
        // m_fDelTime秒でm_MinScaleに変更
        Rule.GetComponent<RectTransform>().DOAnchorPos(-StartPos, 0.5f);
        // m_fDelTime秒でテクスチャをフェードアウト
        Rule.DOFade(0f, 0.5f).OnComplete(() =>
        {
            GameManager.instance.GetSetGameState = GameState.GamePlay;
        });
        m_bRuleVisible = false;
    }
}
