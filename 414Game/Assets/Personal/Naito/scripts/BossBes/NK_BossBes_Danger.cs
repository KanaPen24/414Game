using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossBes_Danger : MonoBehaviour
{
    [SerializeField] private GameObject m_BossSlime;
    private float m_fViewX;
    private float m_StartX;
    private Vector3 m_DangerPos;

    private void Start()
    {
        m_StartX = 180.0f;
    }
    // Update is called once per frame
    void Update()
    {
        m_fViewX= Camera.main.WorldToViewportPoint(m_BossSlime.transform.position).x;
        if(m_fViewX < 0.25f)
        {
            m_DangerPos = this.gameObject.transform.position;
            m_DangerPos.x = 182.5f;
            this.gameObject.transform.position = m_DangerPos;
        }else if (m_fViewX < 0.5f)
        {
            m_DangerPos = this.gameObject.transform.position;
            m_DangerPos.x = 187.5f;
            this.gameObject.transform.position = m_DangerPos;
        }
        else if (m_fViewX < 0.75f)
        {
            m_DangerPos = this.gameObject.transform.position;
            m_DangerPos.x = 192.5f;
            this.gameObject.transform.position = m_DangerPos;
        }
        else
        {
            m_DangerPos = this.gameObject.transform.position;
            m_DangerPos.x = 197.5f;
            this.gameObject.transform.position = m_DangerPos;
        }
    }
}
