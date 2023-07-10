/**
 * @file YK_Crack.cs
 * @brief ヒビを切り替える処理
 * 
 * プレイヤーの武器の耐久値に応じてヒビの状態を切り替えるスクリプトです。
 * 
 * - ヒビレベル1: 武器の耐久値が残り70%以下
 * - ヒビレベル2: 武器の耐久値が残り50%以下
 * - ヒビレベル3: 武器の耐久値が残り20%以下
 * 
 * ヒビの状態に応じてヒビ画像とヒビエフェクトを切り替え、指定時間後にエフェクトを破棄。
 * また、ヒビ状態がない場合は透明。
 * 
 * @author 吉田叶聖
 * @date 2023/04/21 初期制作
 * Iharaによってヒビレベルの処理が更新。(2023/05/12)
 * Iharaによってヒビエフェクトの処理が更新。(2023/05/21)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class YK_Crack : MonoBehaviour
{
    private Image image;
    private Color color;
    [SerializeField] private Sprite Crack_Min; // ヒビレベル1の画像
    [SerializeField] private Sprite Crack_Nor; // ヒビレベル2の画像
    [SerializeField] private Sprite Crack_Max; // ヒビレベル3の画像
    [SerializeField] private IS_WeaponHPBar weaponHpBar; // 武器の耐久値バー
    [SerializeField] private ParticleSystem Glass; // ヒビエフェクト

    private int WeaponHP; // 武器の最大耐久値
    private bool m_bCrackMinFlag; // ヒビレベル1の状態フラグ
    private bool m_bCrackNorFlag; // ヒビレベル2の状態フラグ
    private bool m_bCrackMaxFlag; // ヒビレベル3の状態フラグ

    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
        color = this.GetComponent<Image>().color;
        color.a = 0.0f;

        m_bCrackMinFlag = false;
        m_bCrackNorFlag = false;
        m_bCrackMaxFlag = false;
        WeaponHP = weaponHpBar.GetSetHp;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckHP();
    }

    private void CheckHP()
    {
        // 武器の耐久値をチェックしてヒビの状態を切り替える
        if (weaponHpBar.GetSetHp <= WeaponHP * 0.2f)
        {
            // ヒビレベル3に切り替える処理
            if (!m_bCrackMaxFlag)
            {
                // ヒビレベル3の効果音を再生
                IS_AudioManager.instance.PlaySE(SEType.SE_HPBarCrack_2);

                // 武器の耐久値バーのヒビレベルを切り替え
                weaponHpBar.ChangeCrackLevel(IS_WeaponHPBar.CrackLevel.Level3);

                // ヒビエフェクトを生成して再生
                ParticleSystem Effect = Instantiate(Glass);
                Effect.Play();
                Effect.transform.position = weaponHpBar.transform.position;
                Destroy(Effect.gameObject, 5.0f);

                // ヒビエフェクトをnullに設定し、メモリ解放
                Effect = null;

                // ヒビレベル3の状態フラグを立てる
                m_bCrackMaxFlag = true;

                Debug.Log("残り2割り");
            }

            // ヒビレベル3の画像に切り替える
            image.sprite = Crack_Max;
            return;
        }
        else if (weaponHpBar.GetSetHp <= WeaponHP * 0.5f)
        {
            // ヒビレベル2に切り替える処理
            if (!m_bCrackNorFlag)
            {
                // ヒビレベル2の効果音を再生
                IS_AudioManager.instance.PlaySE(SEType.SE_HPBarCrack_1);

                // 武器の耐久値バーのヒビレベルを切り替え
                weaponHpBar.ChangeCrackLevel(IS_WeaponHPBar.CrackLevel.Level2);

                // ヒビエフェクトを生成して再生
                ParticleSystem Effect = Instantiate(Glass);
                Effect.Play();
                Effect.transform.position = weaponHpBar.transform.position;
                Destroy(Effect.gameObject, 5.0f);

                // ヒビエフェクトをnullに設定し、メモリ解放
                Effect = null;

                // ヒビレベル2の状態フラグを立てる
                m_bCrackNorFlag = true;

                Debug.Log("残り5割り");
            }

            // ヒビレベル2の画像に切り替える
            image.sprite = Crack_Nor;
            return;
        }
        else if (weaponHpBar.GetSetHp <= WeaponHP * 0.7f)
        {
            // ヒビレベル1に切り替える処理
            if (!m_bCrackMinFlag)
            {
                // ヒビレベル1の効果音を再生
                IS_AudioManager.instance.PlaySE(SEType.SE_HPBarCrack_1);

                // 武器の耐久値バーのヒビレベルを切り替え
                weaponHpBar.ChangeCrackLevel(IS_WeaponHPBar.CrackLevel.Level1);

                // ヒビエフェクトを生成して再生
                ParticleSystem Effect = Instantiate(Glass);
                Effect.Play();
                Effect.transform.position = weaponHpBar.transform.position;
                Destroy(Effect.gameObject, 5.0f);

                // ヒビエフェクトをnullに設定し、メモリ解放
                Effect = null;

                // ヒビレベル1の状態フラグを立てる
                m_bCrackMinFlag = true;

                Debug.Log("残り７割り");
            }

            // ヒビレベル1の画像に切り替える
            image.sprite = Crack_Min;
            color.a = 1.0f;
            return;
        }
        else
        {
            // ヒビレベル0に切り替える処理
            weaponHpBar.ChangeCrackLevel(IS_WeaponHPBar.CrackLevel.Level0);
            color.a = 0.0f;
        }

        // 画像の透明度を設定
        this.GetComponent<Image>().color = color;
    }
}


