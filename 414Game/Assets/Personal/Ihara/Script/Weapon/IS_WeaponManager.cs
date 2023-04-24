﻿/**
 * @file   IS_WeaponManager.cs
 * @brief  武器とUIの管理クラス
 * @author IharaShota
 * @date   2023/03/17
 * @Update 2023/03/17 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_WeaponManager : MonoBehaviour
{
    static public IS_WeaponManager instance;            // インスタンス
    [SerializeField] private List<IS_Weapon> m_Weapons; // 武器クラスの動的配列 

    private void Start()
    {
        // インスタンス化する
        //(他のスクリプトから呼び出すためだが、他のシーンには残さない)
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IS_Weapon FindWeapon(IS_Weapon weapon)
    {
        for(int i = 0,size = m_Weapons.Count; i < size; ++i)
        {
            if(weapon == m_Weapons[i])
            {
                return m_Weapons[i];
            }
        }

        return null;
    }
}
