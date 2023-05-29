using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossBes_Up : NK_BossBesStrategy
{
    [SerializeField] private float m_UpPosY;
    private Rigidbody m_Rbody;
    [SerializeField] private NK_BossBes m_BossSlime;
    [SerializeField] private float m_UpPow;
    [SerializeField] private GameObject m_Danger;

    private void Start()
    {
        m_Rbody = GetComponent<Rigidbody>();
    }

    public override void UpdateStrategy()
    {
        m_BossSlime.m_BBMoveValue = new Vector3(0.0f, 0.0f, 0.0f);
        if(m_UpPosY>this.gameObject.transform.position.y)
        {
            m_BossSlime.m_BBMoveValue.y += m_UpPow;
        }
        else
        {
            m_Danger.SetActive(true);
            m_BossSlime.GetSetBossBesState = BossBesState.BossBesFlight;
        }
    }
}
