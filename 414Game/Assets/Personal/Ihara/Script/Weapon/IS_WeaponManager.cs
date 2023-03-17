/**
 * @file   IS_WeaponManager.cs
 * @brief  武器とUIの管理クラス(今のところは表示の切り替えのみ)
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
    [SerializeField] private IS_Player m_Player;        // Player
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

    /**
     * @fn
     * 武器を入れ替える関数
     * @brief  武器とUIの表示を切り替える
     * @detail 武器とUIの列挙番号を連動させる必要がある
     */
    public void ChangeWeapon(PlayerWeaponState weaponState)
    {
        // 武器とUIの数が合わなければ終了する
        if(m_Weapons.Count != 1)
        {
            Debug.Log("武器とUIの数が合いませんので実行できません");
            return;
        }

        for (int i = 0, size = m_Weapons.Count; i < size; ++i)
        {
            // 引数の武器タイプとUIタイプの番号が一致していたら
            if (i == (int)weaponState)
            {
                // 武器を表示・UIを非表示
                m_Weapons[i].gameObject.SetActive(true);
                // m_UIs[i].gameObject.SetActive(false);
            }
            // 一致していなければ
            else
            {
                // UIを表示・武器を非表示
                m_Weapons[i].gameObject.SetActive(false);
                // m_UIs[i].gameObject.SetActive(true);
            }
        }
    }
}
