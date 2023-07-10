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
}