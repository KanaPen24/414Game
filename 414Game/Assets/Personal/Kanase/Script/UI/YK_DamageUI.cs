/**
 * @file YK_DamageUI.cs
 * @brief 敵のダメージを表記するクラス
 *        ダメージのテキストを表示し、フェードアウト
 *        テキストは移動しながら上に上昇し、徐々に透明
 *        テキストが透明になった後はオブジェクトを破棄
 *        ダメージUIの位置はコリジョンと連動し、ダメージを受けたオブジェクトの位置に表示
 *        ダメージUIのフェードアウトには時間をかけ、フェードアウトが完了した後にオブジェクトを破棄
 *        ダメージ値のgetterも提供
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
    private float fadeOutSpeed = 1f;
    [SerializeField] private float moveSpeed = 0.4f;
    [SerializeField] private int m_nDamage;
    [SerializeField] private int m_nCountDown; // 消えるまでの時間（秒単位）
    private int m_nCountTime = 0; // 表示されている時間
    [SerializeField] YK_Time time; // 時間
    private GameObject DamageUI;

    /**
     * @fn
     * ダメージUIの初期化
     * テキストコンポーネントを取得し、UIのタイプやフェード状態を設定
     * カウントダウン時間をフレームに変換
     * タイマーオブジェクトを取得し、ダメージUIオブジェクトを設定
     */
    void Start()
    {
        damageText = GetComponentInChildren<Text>();
        m_eUIType = UIType.DamageNumber; // UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        m_nCountDown *= 60; // 60FPSに合わせる
        time = GameObject.Find("Timer").GetComponent<YK_Time>();
        DamageUI = this.gameObject;
    }

    /**
     * @fn
     * ダメージUIのフェードアウト処理を遅延更新
     * テキストの移動とフェードアウトを行い、アルファ値が一定以下になったらオブジェクトを破棄
     * テキストの位置はカメラの回転に合わせて上に移動
     * テキストの色を徐々に透明に変化
     * アルファ値が一定以下になったらオブジェクトを破棄し、メモリを解放
     */
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
                Destroy(DamageUI);
                DamageUI = null;
            }
        }
    }

    /**
     * @fn
     * ダメージUIのテキストとダメージ値を設定
     * ダメージ値に応じてテキストを更新
     */
    public void SetDamage(int damage)
    {
        damageText = GetComponentInChildren<Text>();
        damageText.text = "" + damage; // damageTextを更新
        m_nDamage = damage;
    }

    /**
     * @fn
     * ダメージUIのフェードアウト処理
     * 指定した時間でテキストのスケールとアルファ値を変化させ、フェードアウト
     * フェードアウトが完了したらオブジェクトを破棄し、メモリを解放
     * 同時に時間オブジェクトにダメージ値を加算
     */
    public override void UIFadeOUT()
    {
        time.AddTime(m_nDamage); // 時間をプラスする
        m_eFadeState = FadeState.FadeOUT;
        damageText.transform.DOScale(m_MinScale, m_fDelTime); // m_fDelTime秒でm_MinScaleに変更
        damageText.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Destroy(DamageUI);
            DamageUI = null;
        });
    }

    /**
     * @fn
     * コリジョンとの衝突時に呼び出される関数
     * 衝突したオブジェクトが「Cursol」という名前の場合、ダメージテキストの色を青に変更
     * ダメージUIの表示位置を衝突したオブジェクトの位置に設定
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Cursol")
        {
            damageText.color = Color.blue;
        }
        GetSetUIPos = collision.transform.position;
    }

    /**
     * @fn
     * コリジョンとの衝突が解消された時に呼び出される関数
     * ダメージテキストの色を赤に変更
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        damageText.color = Color.red;
    }

    /**
     * @fn
     * ダメージ値のgetter関数
     * @return ダメージ値（int）
     * @brief ダメージ値を取得
     */
    public int GetDamage
    {
        get { return m_nDamage; }
    }
}
