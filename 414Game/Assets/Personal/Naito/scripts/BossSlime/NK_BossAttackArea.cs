using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossAttackArea : MonoBehaviour
{
    [SerializeField] private float m_AttackTime;
    [SerializeField] private IS_Player m_Player;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("AttackAreaDestroy", m_AttackTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject==m_Player.gameObject)
        {
            Debug.Log("Player Damage!!");
            m_Player.Damage(10, 5.0f);
        }
    }

    private void AttackAreaDestroy()
    {
        this.gameObject.SetActive(false);
    }
}
