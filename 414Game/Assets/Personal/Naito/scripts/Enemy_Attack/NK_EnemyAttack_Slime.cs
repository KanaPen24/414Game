// ==============================================================
// NK_EnemyAttack_Slime.cs
// Auther:Naito
// Update:2023/03/10 cs作成
// ==============================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_EnemyAttack_Slime : MonoBehaviour
{
    //攻撃
    [SerializeField] private GameObject m_gAttack;


    public void CreateAttack(bool PosRight)
    {
        //プレイヤーがいる方向に攻撃を出す処理
        if (PosRight)
        {
            Instantiate(m_gAttack, new Vector3(this.transform.position.x + 1.0f, this.transform.position.y, this.transform.position.z), Quaternion.identity);
        }
        else
        {
            Instantiate(m_gAttack, new Vector3(this.transform.position.x - 1.0f, this.transform.position.y, this.transform.position.z), Quaternion.identity);
        }
    }
}
