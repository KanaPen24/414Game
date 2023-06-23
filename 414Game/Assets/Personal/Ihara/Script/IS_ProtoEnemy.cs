/**
 * @file   IS_ProtoEnemy.cs
 * @brief  敵のプロトクラス
 * @author IharaShota
 * @date   2023/03/13
 * @Update 2023/03/13 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_ProtoEnemy : MonoBehaviour
{
    [SerializeField] private IS_Player m_Player;  // Player
    //[SerializeField] private YK_HPBerHP m_HpBarHP;// HPBarのHP

    private void OnTriggerEnter(Collider collision)
    {
        // 武器だったら
        if (collision.gameObject.tag == "Weapon")
        {
            if (m_Player.GetWeapons((int)m_Player.GetSetEquipState).GetSetAttack)
            {
                Debug.Log("Enemy Damage!!");
                //m_HpBarHP.DelLife(10);
                //m_Player.GetPlayerHp().AddLife(5);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // プレイヤーだったら
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player Damage!!");
            //m_Player.GetPlayerHp().DelLife(10);
        }

        // 武器だったら
        if (collision.gameObject.tag == "Weapon")
        {
            Debug.Log("Enemy Damage!!");
         //   m_HpBarHP.DelLife(10);
        }
    }
}
