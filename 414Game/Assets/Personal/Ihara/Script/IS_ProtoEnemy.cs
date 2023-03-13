/**
 * @file   IS_ProtoEnemy.cs
 * @brief  �G�̃v���g�N���X
 * @author IharaShota
 * @date   2023/03/13
 * @Update 2023/03/13 �쐬
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_ProtoEnemy : MonoBehaviour
{
    [SerializeField] private IS_Player m_Player;  // Player
    [SerializeField] private YK_HPBerHP m_HpBarHP;// HPBar��HP

    private void OnTriggerEnter(Collider collision)
    {
        // ���킾������
        if (collision.gameObject.tag == "Weapon")
        {
            if (m_Player.GetWeapons(m_Player.nWeaponState).GetSetAttack)
            {
                Debug.Log("Enemy Damage!!");
                m_HpBarHP.DelLife(10);
                m_Player.GetPlayerHp().AddLife(5);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �v���C���[��������
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player Damage!!");
            m_Player.GetPlayerHp().DelLife(10);
        }
    }
}
