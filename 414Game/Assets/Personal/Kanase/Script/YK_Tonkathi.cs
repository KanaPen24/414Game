/**
 * @file YK_Tonkathi.cs
 * @brief UI回復アイテムの処理
 * @author 吉田叶聖
 *         このスクリプトの作成者
 * @date   2023/05/25
 *         初版作成日
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YK_Tonkathi : MonoBehaviour
{
    [SerializeField] private IS_Player Player;
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            Player.GetSetItemHit = true;
        }
        Destroy(gameObject);
    }
}
