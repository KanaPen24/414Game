/**
 * @file PlayerHP.cs
 * @brief プレイヤーの体力
 * @author 吉田叶聖
 * @date 2023/03/06
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YK_PlayerHP : MonoBehaviour
{

    //　最大HP
    [SerializeField]
    private int m_nMaxHP = 10000;
    //　徐々に減らしていくhp計測に使う
    [SerializeField]
    private int m_nHP;
    //　最終的なhp計測に使う
    private int m_nFinalHP;
    //　HPを一度減らしてからの経過時間
    private float m_fCountLime = 0f;
    //　次にHPを減らすまでの時間
    [SerializeField]
    private float m_fNextCountTime = 0f;
    //　HP表示用スライダー
    [SerializeField]
    private Slider HpSlider;
    //　一括HP表示用スライダー
    [SerializeField]
    private Slider BulkHPSlider;
    //　現在のダメージ量
    private int m_nDamage = 0;
    //　一回に減らすダメージ量
    [SerializeField]
    private int m_nAmountOfDamageAtOneTime = 100;
    //　HPを減らしているかどうか
    private bool m_bReducing;
    //　HP用表示スライダーを減らすまでの待機時間
    [SerializeField]
    private float m_fDelayTime = 1f;

    void Start()
    {
        m_nHP = m_nMaxHP;
        m_nFinalHP = m_nMaxHP;
        HpSlider.value = 1;
        BulkHPSlider.value = 1;
    }

    void Update()
    {
        //　ダメージなければ何もしない
        if (!m_bReducing)
        {
            return;
        }
        //　次に減らす時間がきたら
        if (m_fCountLime >= m_fNextCountTime)
        {
            int tempDamage;
            //　決められた量よりも残りダメージ量が小さければ小さい方を1回のダメージに設定
            tempDamage = Mathf.Min(m_nAmountOfDamageAtOneTime, m_nDamage);
            m_nHP -= tempDamage;
            //　全体の比率を求める
            HpSlider.value = (float)m_nHP / m_nMaxHP;
            //　全ダメージ量から1回で減らしたダメージ量を減らす
            m_nDamage -= tempDamage;
            //　全ダメージ量が0より下になったら0を設定
            m_nDamage = Mathf.Max(m_nDamage, 0);

            m_fCountLime = 0f;
            //　ダメージがなくなったらHPバーの変更処理をしないようにする
            if (m_nDamage <= 0)
            {
                m_bReducing = false;
            }

            //　HPが0以下になったら敵を削除
            if (m_nHP <= 0)
            {
                Debug.Log("ゲームオーバー");
            }
        }
        m_fCountLime += Time.deltaTime;
    }

    //　ダメージ値を追加するメソッド
    public void DelLife(int damage)
    {
        //　ダメージを受けた時に一括HP用のバーの値を変更する
        var tempHP = Mathf.Max(m_nFinalHP -= damage, 0);
        BulkHPSlider.value = (float)tempHP / m_nMaxHP;
        this.m_nDamage += damage;
        m_fCountLime = 0f;
        //　一定時間後にHPバーを減らすフラグを設定
        Invoke("StartReduceHP", m_fDelayTime);
    }
    // ダメージ処理
    // 回復処理
    public void AddLife(int damege)
    {
        //　ダメージを受けた時に一括HP用のバーの値を変更する
        var tempHP = Mathf.Max(m_nFinalHP += m_nDamage, 0);
        BulkHPSlider.value = (float)tempHP / m_nMaxHP;
        this.m_nDamage -= m_nDamage;
        m_fCountLime = 0f;
        //　一定時間後にHPバーを減らすフラグを設定
        Invoke("StartReduceHP", m_fDelayTime);
    }
    //　徐々にHPバーを減らすのをスタート
    public void StartReduceHP()
    {
        m_bReducing = true;
    }
}
