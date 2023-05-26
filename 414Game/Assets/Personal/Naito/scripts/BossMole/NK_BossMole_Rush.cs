using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossMole_Rush : NK_BossMoleStrategy
{
    [SerializeField] private NK_BossMole m_BossMole;
    [SerializeField] private float m_RushSpeed;
    private float m_Cnt;
    [SerializeField] private float m_RushTime;
    private float m_BackCnt;
    [SerializeField] private float m_BackTime;
    [SerializeField] private float m_BackSpeed;

    public override void UpdateStrategy()
    {
        if(m_BossMole.GetSetBossMoleDir == BossMoleDir.Left)
        {
            m_Cnt += Time.deltaTime;
            if (m_Cnt < m_RushTime)
            {
                this.gameObject.transform.position = Vector3.MoveTowards(
                    this.gameObject.transform.position, m_BossMole.m_RushPosL.transform.position, m_RushSpeed * Time.deltaTime);
            }
            else
            {
                m_BackCnt += Time.deltaTime;
                this.transform.localScale =
                    new Vector3(-m_BossMole.m_MLocalScaleX, this.transform.localScale.y, this.transform.localScale.z);
                this.gameObject.transform.position = Vector3.MoveTowards(
                  this.gameObject.transform.position, m_BossMole.m_MolePosL.transform.position, m_BackSpeed * Time.deltaTime);
                if(m_BackCnt>m_BackTime)
                {
                    m_BossMole.GetSetBossMoleDir = BossMoleDir.Right;
                    m_BossMole.GetSetBossMoleState = BossMoleState.MoleWait;
                }
            }
        }

        if (m_BossMole.GetSetBossMoleDir == BossMoleDir.Right)
        {
            m_Cnt += Time.deltaTime;
            if (m_Cnt < m_RushTime)
            {
                this.gameObject.transform.position = Vector3.MoveTowards(
                    this.gameObject.transform.position, m_BossMole.m_RushPosR.transform.position, m_RushSpeed * Time.deltaTime);
            }
            else
            {
                m_BackCnt += Time.deltaTime;
                this.transform.localScale =
                    new Vector3(m_BossMole.m_MLocalScaleX, this.transform.localScale.y, this.transform.localScale.z);
                this.gameObject.transform.position = Vector3.MoveTowards(
                  this.gameObject.transform.position, m_BossMole.m_MolePosR.transform.position, m_BackSpeed * Time.deltaTime);
                if (m_BackCnt > m_BackTime)
                {
                    m_BossMole.GetSetBossMoleDir = BossMoleDir.Left;
                    m_BossMole.GetSetBossMoleState = BossMoleState.MoleWait;
                }
            }
        }
    }
}
