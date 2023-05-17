using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_SlimeBes_Crawl : NK_SlimeBesStrategy
{
    [SerializeField] private NK_SlimeBes m_SlimeBes;
    [SerializeField] private float m_MovePow;
    public override void UpdateStrategy()
    {
        m_SlimeBes.m_MoveValue = new Vector3(0.0f, 0.0f, 0.0f);

        if(m_SlimeBes.GetSetBesDir==BesDir.Left)
        {
            m_SlimeBes.m_MoveValue.x -= m_MovePow;
            m_SlimeBes.GetSetBesState = SlimeBesState.BesWait;
        }
        if(m_SlimeBes.GetSetBesDir==BesDir.Right)
        {
            m_SlimeBes.m_MoveValue.x += m_MovePow;
            m_SlimeBes.GetSetBesState = SlimeBesState.BesWait;
        }
    }
}
