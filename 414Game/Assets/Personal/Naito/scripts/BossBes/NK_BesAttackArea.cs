﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BesAttackArea : MonoBehaviour
{
    [SerializeField] private float m_AttackTime;
    [SerializeField] private IS_Player m_Player;
    private float m_Cnt;
    [SerializeField] private int m_PlayerDamage;
    // Start is called before the first frame update
    private void Update()
    {
        m_Cnt += Time.deltaTime;
        if(m_Cnt > m_AttackTime)
        {
            m_Cnt = 0;
            AttackAreaDestroy();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject==m_Player.gameObject)
        {
            Debug.Log("Player Damage!!");
            m_Player.Damage(m_PlayerDamage, 5.0f);
        }
    }

    private void AttackAreaDestroy()
    {
        this.gameObject.SetActive(false);
    }
}