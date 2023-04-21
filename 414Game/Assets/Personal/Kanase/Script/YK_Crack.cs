/**
 * @file YK_Crack.cs
 * @brief ヒビを切り替える処理
 * @author 吉田叶聖
 * @date 2023/04/21
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
    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
        color = this.GetComponent<Image>().color;
        color.a = 0.0f;
    }

    // Update is called once per frame
    private void Update()
    {
        // ※ここで画像の切替を行う
        if (weaponHpBar.GetSetHp <= 25)
        {
            image.sprite = Crack_Max;
            return;
        }
        else if (weaponHpBar.GetSetHp <= 50)
        {
            image.sprite = Crack_Nor;
            return;
        }
        else if (weaponHpBar.GetSetHp <= 75)
        {
            image.sprite = Crack_Min;
            color.a = 1.0f;
            return;
        }
        else
        {
            color.a = 0.0f;
        }
        this.GetComponent<Image>().color = color;
    }
}
