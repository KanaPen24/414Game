/**
 * @file   YK_GameOver.cs
 * @brief  ゲームオーバークラス
 * @author YoshidaKanase
 * @date   2023/04/26
 * @Update 2023/05/05 ゲームオーバーのBGM実装
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// ===============================================
// GameOverState
// … Gameの状態を管理する列挙体
// ===============================================
public enum GameOverState
{
   BreakHPBar,
   NoHP,
   TimeLimit,

    MaxGameOverState
}

public class YK_GameOver : MonoBehaviour
{
    public static YK_GameOver instance;
    [SerializeField] private GameObject GameOver;
    [SerializeField] private YK_Clock clock;
    [SerializeField] private ON_TextEntry TextEntry;
    private bool m_bGameOverFlg;
    [SerializeField] private GameOverState m_GameOverState;
    [SerializeField] private CanvasGroup TextCanvas;
    [SerializeField] private Text HPBarBroken;
    [SerializeField] private Text NoHP;
    [SerializeField] private Text TimeLimit;
    private Vector2 FirstPos;
    [SerializeField]　private float m_fFadeTime; //フェードの時間

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        GameOver.SetActive(false);
        m_bGameOverFlg = false;
        HPBarBroken.DOFade(0f, 0f);
        NoHP.DOFade(0f, 0f); 
        TimeLimit.DOFade(0f, 0f);
        FirstPos = TextCanvas.GetComponent<RectTransform>().anchoredPosition; 

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GetSetGameState == GameState.GameOver&&!m_bGameOverFlg)
        {
            TextCanvas.gameObject.SetActive(true);
            FadeIN();
            if (!m_bGameOverFlg)
            {
                IS_AudioManager.instance.AllStopSE();
                IS_AudioManager.instance.StopBGM(BGMType.BGM_Game);
                IS_AudioManager.instance.PlayBGM(BGMType.BGM_GAMEOVER);
            }
            m_bGameOverFlg = true;
            //Time.timeScale = 0.0f;
            GameOver.SetActive(true);
        }
    }
    //Ruleを表示
    public void FadeIN()
    {
        switch (m_GameOverState)
        {
            case GameOverState.BreakHPBar:
                HPBarBroken.DOFade(1f, m_fFadeTime);
                break;
            case GameOverState.NoHP:
                NoHP.DOFade(1f, m_fFadeTime);
                break;
            case GameOverState.TimeLimit:
                TimeLimit.DOFade(1f, m_fFadeTime);
                break;
        }
        // 2秒で後X,Y方向を元の大きさに変更
        TextCanvas.GetComponent<RectTransform>().DOAnchorPos(new Vector2(FirstPos.x, 0f), m_fFadeTime).OnComplete(() =>
        {
            TextCanvas.GetComponent<RectTransform>().DOAnchorPos(new Vector2(FirstPos.x, -FirstPos.y), m_fFadeTime);
            switch (m_GameOverState)
            {
                case GameOverState.BreakHPBar:
                    HPBarBroken.DOFade(0f, m_fFadeTime);
                    break;
                case GameOverState.NoHP:
                    NoHP.DOFade(0f, m_fFadeTime);
                    break;
                case GameOverState.TimeLimit:
                    TimeLimit.DOFade(0f, m_fFadeTime);
                    break;
            }
        });

    }
    /**
 * @fn
 * GameOverの状態のgetter・setter
 * @return m_GameState
 * @brief GameOverの状態を返す・セット
 */
    public GameOverState GetSetGameOverState
    {
        get { return m_GameOverState; }
        set { m_GameOverState = value; }
    }

}
