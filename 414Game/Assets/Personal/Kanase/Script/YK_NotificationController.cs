using UnityEngine;
using DG.Tweening;

public class YK_NotificationController : YK_UI
{
    private static readonly Vector3 INIT_POS = new Vector3(-700f, -200f, 0f);
    private static readonly float MOVE_TIME = 0.5f;
    private static readonly float INTERVAL_TIME = 2.5f;
    private static readonly float FADE_TIME = 0.7f;

    [SerializeField]
    private CanvasGroup m_CanvasGroup;

    private Sequence m_Sequence;

    private void Start()
    {
        m_eUIType = UIType.Notification;       // UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        GetSetUIPos = this.GetComponent<RectTransform>().anchoredPosition;    // UIの座標取得
        GetSetUIScale = this.transform.localScale;                           // UIのスケール取得
    }

    void Update()
    {
        // キーボードの「スペース」キーを押したら演出再生
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayNotification();
        }
    }

    private void PlayNotification()
    {
        m_Sequence?.Kill();
        
        m_Sequence = DOTween.Sequence()
            .OnStart(() =>
            {
                m_CanvasGroup.transform.localPosition = INIT_POS;
                m_CanvasGroup.alpha = 1f;
            })
            .Append(m_CanvasGroup.transform.DOLocalMoveX(0, MOVE_TIME).SetEase(Ease.OutQuart)) // 左からスライドイン
            .AppendInterval(INTERVAL_TIME) // 待機時間
            .Append(m_CanvasGroup.DOFade(0f, FADE_TIME)); // フェードアウト
        

        m_Sequence.Play();
    }

    /**
     * @fn
     * UIのフェードイン処理を行う関数
     */
    public override void UIFadeIN()
    {
        m_eFadeState = FadeState.FadeIN;
        this.gameObject.transform.DOScale(GetSetUIScale, 0f);    // 0秒で後X,Y方向を元の大きさに変更
        m_CanvasGroup.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeIN終了");
        });
    }

    /**
     * @fn
     * UIのフェードアウト処理を行う関数
     */
    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        this.gameObject.transform.DOScale(m_MinScale, m_fDelTime);    // m_fDelTime秒でm_MinScaleに変更
        m_CanvasGroup.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeOUT終了");
        });
    }
}