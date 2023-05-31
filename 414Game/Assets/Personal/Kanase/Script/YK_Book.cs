/**
 * @file YK_Book.cs
 * @brief 本の制御
 * @author 吉田叶聖
 * @date 2023/05/28
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YK_Book : MonoBehaviour
{
    private bool m_Hit;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            m_Hit = true;
        }
        Destroy(gameObject);
    }
  
    /**
* @fn
* 当たったかのgetter・setter
* @return m_bHit(bool)
* @brief 当たり判定
*/
    public bool GetSetHitFlg
    {
        get { return m_Hit; }
        set { m_Hit = value; }
    }
}
