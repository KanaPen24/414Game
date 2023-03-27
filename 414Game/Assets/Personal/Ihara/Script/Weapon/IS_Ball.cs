/**
 * @file   IS_Ball.cs
 * @brief  生成したBallのクラス
 * @author IharaShota
 * @date   2023/03/19
 * @Update 2023/03/19 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody m_Rigidbody; //RigidBody

    /**
     * @fn
     * 弾発射処理
     * @param (InitVel) 初期速度
     * @param (Dir)     弾を発射する向き
     * @brief 初期速度を設定し、発射する(velocityを設定すれば後は勝手に動く)
     */
    public void Fire(float InitVel,PlayerDir Dir)
    {
        // 右向きなら
        if(Dir == PlayerDir.Right)
        {
            m_Rigidbody.velocity = new Vector3(InitVel, 0f, 0f);
        }
        // 左向きなら
        else if (Dir == PlayerDir.Left)
        {
            m_Rigidbody.velocity = new Vector3(-InitVel, 0f, 0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
