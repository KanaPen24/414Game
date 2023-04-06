/**
 * @file   IS_Ball.cs
 * @brief  生成したBallのクラス
 * @author IharaShota
 * @date   2023/03/19
 * @Update 2023/03/19 作成
 * @Update 2023/04/06 着弾エフェクト実装 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody m_Rigidbody; //RigidBody
    [SerializeField] private ParticleSystem hitEffect; // 着弾エフェクト

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
        // エフェクト再生
        ParticleSystem Effect = Instantiate(hitEffect);
        Effect.Play();
        Effect.transform.position = this.transform.position;
        Effect.transform.localScale = new Vector3(2f, 2f, 2f);
        Destroy(Effect.gameObject, 1.0f); // 1秒後に消える

        // 自身のオブジェクトを削除
        Destroy(this.gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
