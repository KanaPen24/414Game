/**
 * @file YK_DamageUI.cs
 * @brief 敵のダメージを表記
 * @author 吉田叶聖
 * @date 2023/05/02
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class YK_DamageUI : YK_UI
{

    private Text damageText;
    //　フェードアウトするスピード
    private float fadeOutSpeed = 1f;
    //　移動値
    [SerializeField] private float moveSpeed = 0.4f;
    [SerializeField] private int m_nDamage;    
    [SerializeField] private Vector3 m_MinScale = new Vector3(0.5f, 0.5f, 0.5f); // 最小サイズ
    [SerializeField] private float m_fDelTime = 0.5f;   // 減算していく時間
    [SerializeField] private int m_nCountDown;          // 消えるまでの時間（秒単位）
    private int m_nCountTime = 0;                       // 表示されている時間
    [SerializeField] YK_Time time;                      // 時間
    void Start()
    {        
        damageText = GetComponentInChildren<Text>();
        m_eUIType = UIType.DamageNumber; //UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        //UIが動くようならUpdateにかかなかん
        GetSetPos = this.GetComponent<RectTransform>().anchoredPosition;
        //スケール取得
        GetSetScale = damageText.transform.localScale;
        m_nCountDown *= 60; //60FPSに合わせる
        time= GameObject.Find("Timer").GetComponent<YK_Time>();
    }

    void LateUpdate()
    {
        m_nCountTime++;
        if (m_nCountTime >= m_nCountDown)
        {
            transform.rotation = Camera.main.transform.rotation;
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;

            damageText.color = Color.Lerp(damageText.color, new Color(1f, 0f, 0f, 0f), fadeOutSpeed * Time.deltaTime);

            if (damageText.color.a <= 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetDamage(int damage)
    {
        damageText = GetComponentInChildren<Text>();
        //damageTextを更新
        damageText.text = "" + damage;
        m_nDamage = damage;
    }
    public override void UIFadeOUT()
    {
        //時間をプラスする
        time.AddTime(m_nDamage);
        m_eFadeState = FadeState.FadeOUT;
        // m_fDelTime秒でm_MinScaleに変更
        damageText.transform.DOScale(m_MinScale, m_fDelTime);
        // m_fDelTime秒でテクスチャをフェードイン
        damageText.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Destroy(gameObject);
        });
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Cursol")
        {
            damageText.color = Color.blue;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        damageText.color = Color.red;
    }
    /**
* @fn
* ダメージのgetter
* @return m_nDamage(int)
* @brief ダメージを受け取る
*/
    public int GetDamage
    {
        get { return m_nDamage; }
    }
}