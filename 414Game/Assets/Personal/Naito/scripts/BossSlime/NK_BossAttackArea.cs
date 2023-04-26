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

    // Update is called once per frame
    void Update()
    {
        
    }
}
