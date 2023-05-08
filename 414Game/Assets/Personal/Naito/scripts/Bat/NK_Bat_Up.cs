using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_Bat_Up : NK_BatStrategy
{
    //空を飛んでいるときのY軸の位置
    [SerializeField] private float m_FlyPosY;
    //上がる速度
    [SerializeField] private float m_FlyPow; 
    
    // Start is called before the first frame update
    void Start()
    {
        m_FlyPosY = this.gameObject.transform.position.y;
    }

    public override void UpdateStrategy()
    {
        if(this.gameObject.transform.position.y<m_FlyPosY)
        {
            transform.Translate(transform.up * m_FlyPow);
        }
    }
}
