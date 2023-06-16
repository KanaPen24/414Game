/**
 * @file YK_Tonkathi.cs
 * @brief UI回復アイテムの処理
 * @author 吉田叶聖
 *         このスクリプトの作成者
 * @date   2023/05/25
 *         初版作成日
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YK_Tonkathi : YK_Item
{
    [SerializeField] private IS_Player Player;
    [SerializeField] private List<YK_SkillIcon> SkillIcons;
    [SerializeField] private int m_nSkillLimit;  //スキルの使用回数
    [SerializeField] private IS_WeaponHPBar WeaponHPBar;    //HPバー
    [SerializeField] private IS_WeaponStart WeaponStart;    //スタート
    [SerializeField] private YK_Clock Clock;    //時計
    [SerializeField] private YK_Time time;      //テキストの方の時計
    [SerializeField] private ParticleSystem Effect; //回復エフェクト

    // Start is called before the first frame update
    void Start()
    {
        m_eItemType = ItemType.Tonkathi; //アイテムのタイプ設定
        GetSetItemHit = false;
        m_nSkillLimit = SkillIcons.Capacity - 1;//配列のため-１をして調整
    }
    //回避使用時に呼び出す関数
    public void SkillHeal()
    {
        for (int i = 0; i <= m_nSkillLimit; i++)
        {
            //スキルのストックがないものを探す
            if (SkillIcons[i].GetSetStuck == 0)
            {
                SkillIcons[i].GetSetStuck = 1;    //ストックを増やす
                SkillIcons[i].HealEffect();
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetSetItemHit = true;

            //なに持ってない
            if (Player.GetSetPlayerEquipState == PlayerEquipState.NoneEquip)
                return;
            switch(Player.GetSetEquipWeaponState)
            {
                case EquipWeaponState.PlayerHpBar:
                    // SE再生
                    IS_AudioManager.instance.PlaySE(SEType.SE_Tonkachi);
                    Effect.Play();
                    WeaponHPBar.AddLife(WeaponHPBar.GetSetMaxHp / 2);
                    Destroy(gameObject);
                    break;
                case EquipWeaponState.PlayerStart:
                    IS_AudioManager.instance.PlaySE(SEType.SE_Tonkachi);
                    Effect.Play();
                    WeaponStart.AddLife(WeaponStart.GetSetMaxHp / 2);
                    Destroy(gameObject);
                    break;
                case EquipWeaponState.PlayerSkillIcon:
                    IS_AudioManager.instance.PlaySE(SEType.SE_Tonkachi);
                    SkillHeal();                    
                    Destroy(gameObject);
                    break;
                case EquipWeaponState.PlayerClock:
                    IS_AudioManager.instance.PlaySE(SEType.SE_Tonkachi);
                    time.EffectPlay();
                    Clock.GetSetTimeCount += 1;
                    Destroy(gameObject);
                    break;
            }
        }
        
    }
   
}
