/**
 * @file YK_Crack.cs
 * @brief ヒビを切り替える処理
 * @author 吉田叶聖
 * @date 2023/04/21
 * @Update 2023/05/12 ヒビレベルの処理を反映(Ihara)
 * @Update 2023/05/21 ヒビエフェクトの処理を反映(Ihara)
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
    [SerializeField] private Sprite Crack_Min;
    [SerializeField] private Sprite Crack_Nor;
    [SerializeField] private Sprite Crack_Max;
    [SerializeField] private IS_WeaponHPBar weaponHpBar;
    [SerializeField] private ParticleSystem Glass;

    private int WeaponHP;
    private bool m_bCrackMinFlag;
    private bool m_bCrackNorFlag;
    private bool m_bCrackMaxFlag;
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
        // ※ここで画像の切替を行う
        if (weaponHpBar.GetSetHp <= WeaponHP * 0.2f)
        {
            if (!m_bCrackMaxFlag)
            {
                IS_AudioManager.instance.PlaySE(SEType.SE_HPBarCrack_2);
                weaponHpBar.ChangeCrackLevel(IS_WeaponHPBar.CrackLevel.Level3);
                ParticleSystem Effect = Instantiate(Glass);
                Effect.Play();
                Effect.transform.position = weaponHpBar.transform.position;
                Destroy(Effect.gameObject, 5.0f);
                m_bCrackMaxFlag = true;
                Debug.Log("残り2割り");
            }
            image.sprite = Crack_Max;
            return;
        }
        else if (weaponHpBar.GetSetHp <= WeaponHP * 0.5f)
        {
            if (!m_bCrackNorFlag)
            {
                IS_AudioManager.instance.PlaySE(SEType.SE_HPBarCrack_1);
                weaponHpBar.ChangeCrackLevel(IS_WeaponHPBar.CrackLevel.Level2);
                ParticleSystem Effect = Instantiate(Glass);
                Effect.Play();
                Effect.transform.position = weaponHpBar.transform.position;
                Destroy(Effect.gameObject, 5.0f);
                m_bCrackNorFlag = true;
                Debug.Log("残り5割り");
            }
            image.sprite = Crack_Nor;
            return;
        }
        else if (weaponHpBar.GetSetHp <= WeaponHP * 0.7f) 
        {
            if (!m_bCrackMinFlag)
            {
                IS_AudioManager.instance.PlaySE(SEType.SE_HPBarCrack_1);
                weaponHpBar.ChangeCrackLevel(IS_WeaponHPBar.CrackLevel.Level1);
                ParticleSystem Effect = Instantiate(Glass);
                Effect.Play();
                Effect.transform.position = weaponHpBar.transform.position;
                Destroy(Effect.gameObject, 5.0f);
                m_bCrackMinFlag = true;
                Debug.Log("残り７割り");
            }
            image.sprite = Crack_Min;
            color.a = 1.0f;
            return;
        }
        else
        {
            weaponHpBar.ChangeCrackLevel(IS_WeaponHPBar.CrackLevel.Level0);
            color.a = 0.0f;
        }
       
        this.GetComponent<Image>().color = color;
    }
}
