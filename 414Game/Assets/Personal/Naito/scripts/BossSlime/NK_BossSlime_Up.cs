using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossSlime_Up : NK_BossSlimeStrategy
{
    [SerializeField] private float m_UpPosY;
    private Rigidbody m_Rbody;
    [SerializeField] private NK_BossSlime m_BossSlime;
    [SerializeField] private float m_UpPow;

    private void Start()
    {
        m_Rbody = GetComponent<Rigidbody>();
    }

    public override void UpdateStrategy()
    {
        m_BossSlime.m_BSMoveValue = new Vector3(0.0f, 0.0f, 0.0f);
        if(m_UpPosY>this.gameObject.transform.position.y)
        {
            m_BossSlime.m_BSMoveValue.y += m_UpPow;
        }
        else
        {
            m_BossSlime.GetSetBossSlimeState = BossSlimeState.BossSlimeFlight;
        }
    }
}
