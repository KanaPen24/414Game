/**
 * @file HPBarVisible.cs
 * @brief HPBarを消したり表示したりする
 * @author 吉田叶聖
 * @date 2023/03/06
 * @date 2023/03/12 HPBarを表示管理するbool型作成(Ihara)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; // 型キャストする際に必要

public class YK_HPBarVisible : MonoBehaviour
{
    [SerializeField] Slider HP;
    [SerializeField] private IS_WeaponHPBar m_WeaponHpBar;
    private bool m_bVisible;
    private int m_nCnt;

    // Start is called before the first frame update
    void Start()
    {
        // メンバの初期化
        m_bVisible = true;
        m_nCnt = Convert.ToInt32(m_bVisible);

        // 表示状態だったら
        if (m_bVisible)
        {
            HPEnableTrue();
        }
        // 非表示状態だったら
        else
        {
            HPEnableFalse();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 前回と状態が違ったら
        if(m_nCnt != Convert.ToInt32(m_bVisible))
        {
            // 表示状態だったら
            if(m_bVisible)
            {
                HPEnableTrue();
            }
            // 非表示状態だったら
            else
            {
                HPEnableFalse();
            }
        }

        // 現在の状態に更新
        m_nCnt = Convert.ToInt32(m_bVisible);
    }

    //HPBarを消す
    public void HPEnableFalse()
    {
        HP.gameObject.SetActive(false);
        m_WeaponHpBar.gameObject.SetActive(true);
    }

    //HPBarを表示
    public void HPEnableTrue()
    {
        HP.gameObject.SetActive(true);
        m_WeaponHpBar.gameObject.SetActive(false);
    }

    /**
     * @fn
     * PlayerHP表示のgetter・setter
     * @return m_bVisible(bool)
     * @brief PlayerHP表示を返す・セット
     */
    public bool GetSetVisible
    {
        get { return m_bVisible; }
        set { m_bVisible = value; }
    }
}
