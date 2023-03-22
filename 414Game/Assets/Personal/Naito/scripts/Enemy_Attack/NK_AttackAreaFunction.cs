// ==============================================================
// NK_AttackAreaFunction.cs
// Auther:Naito
// Update:2023/03/10 cs作成
// ==============================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_AttackAreaFunction : MonoBehaviour
{
    [SerializeField] private IS_Player m_Player;  // Player
    private void Start()
    {
        //攻撃範囲が消える処理
        Invoke("AttackAreaDereta", 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //ここにプレイヤーがダメージを食らう処理を書いてね
            Debug.Log("Player Damage!!");
            m_Player.GetPlayerHp().DelLife(10);
        }
    }

    private void AttackAreaDereta()
    {
        Destroy(this.gameObject);
    }

}
