/**
 * @file HPBarHP.cs
 * @brief 体力バーの体力
 * @author 吉田叶聖
 * @date 2023/03/06
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YK_HPBerHP : MonoBehaviour
{
    // 体力値を格納する変数（一旦 100）
    public static int nMaxHP = 100;
    // 現在の体力値を格納する変数（初期値は maxHealth）
    public int nCurrentHP = nMaxHP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddLife(1);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            DelLife(1);
        }
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
            // コンソールに"HPBarHPMax!"を表示する
            Debug.Log("HPBarHPMax!");
        }
    }
}
