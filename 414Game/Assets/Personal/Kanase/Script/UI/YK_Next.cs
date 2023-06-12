/**
 * @file YK_Next.cs
 * @brief NextUIの処理
 * @author 吉田叶聖
 * @date 2023/03/25
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用

public class YK_Next : YK_UI
{
    [SerializeField] private Image Next;
    [SerializeField] private Fade fade;
    private bool m_bClear = false;
    // Start is called before the first frame update
    void Start()
    {
        m_eUIType = UIType.Next; //UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        //UIが動くようならUpdateにかかなかん
        GetSetUIPos = Next.GetComponent<RectTransform>().anchoredPosition;
        //スケール取得
        GetSetUIScale = Next.transform.localScale;
        UIFadeOUT();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //NextUIを表示
    public override void UIFadeIN()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<PointEffector2D>().enabled = true;
        m_eFadeState = FadeState.FadeIN;
        // 1秒で後X,Y方向を元の大きさに変更
        Next.transform.DOScale(GetSetUIScale, 0f);
        // 1秒でテクスチャをフェードイン
        Next.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            m_bClear = true;
        });
    }

    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        // 1秒で後X,Y方向を0.5倍に変更
        Next.transform.DOScale(m_MinScale, m_fDelTime);
        // 1秒でテクスチャをフェードアウト
        Next.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            //フェードアウトが終わったら
            GetSetFadeState = FadeState.FadeNone;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<PointEffector2D>().enabled = false;
            if (m_bClear)
            {
                IS_AudioManager.instance.StopBGM(BGMType.BGM_Game);
                SceneManager.LoadScene("GameScene");
            }
        });
    }
}