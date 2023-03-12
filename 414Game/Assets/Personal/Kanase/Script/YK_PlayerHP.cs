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
    // 体力値を格納する変数（一旦 100）
    public static int nMaxHP = 100;
    // 現在の体力値を格納する変数（初期値は maxHealth）
    public int nCurrentHP = nMaxHP;
    //Sliderを入れる
    [SerializeField] Slider HPBar;
    // Start is called before the first frame update
    void Start()
    {
        //Sliderを満タンにする。
        HPBar.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AddLife(1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            DelLife(1);
        }
        //最大HPにおける現在のHPをSliderに反映。
        //int同士の割り算は小数点以下は0になるので、
        //(float)をつけてfloatの変数として振舞わせる。
        HPBar.value = (float)nCurrentHP / (float)nMaxHP;
    }
    // ダメージ処理
    public void DelLife(int damage)
    {
        // 現在の体力値から 引数 damage の値を引く
        nCurrentHP -= damage;
        // 現在の体力値が 0 以下の場合
        if (nCurrentHP <= 0)
        {
            // 現在の体力値に 0 を代入
            nCurrentHP = 0;
            // コンソールに"Dead!"を表示する
            Debug.Log("Dead!");
        }
    }
    // 回復処理
    public void AddLife(int heal)
    {
        // 現在の体力値から 引数 heal の値を足す
        nCurrentHP += heal;
        // 現在の体力値が maxHealth 以上の場合
        if (nCurrentHP >= nMaxHP)
        {
            // 現在の体力値に 最大値 を代入
            nCurrentHP = nMaxHP;
            // コンソールに"Max!"を表示する
            Debug.Log("Max!");
        }
    }
}
