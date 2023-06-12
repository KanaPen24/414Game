/**
 * @file   IS_Ball.cs
 * @brief  生成したBallのクラス
 * @author IharaShota
 * @date   2023/03/19
 * @Update 2023/03/19 作成
 * @Update 2023/04/06 着弾エフェクト実装 
 * @Update 2023/04/17 SE実装
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody m_Rigidbody;    //RigidBody
    [SerializeField] private ParticleSystem hitEffect; // 着弾エフェクト
    [SerializeField] private float fGravity;           // 重力
    [SerializeField] private float fUpPow;             // 上への力
    [SerializeField] private int   nAttackPow;         // 攻撃力
    [SerializeField] private NK_BossSlime bossSlime;

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
            m_Rigidbody.velocity = new Vector3(InitVel, fUpPow, 0f);
        }
        // 左向きなら
        else if (Dir == PlayerDir.Left)
        {
            m_Rigidbody.velocity = new Vector3(-InitVel, fUpPow, 0f);
        }
    }

    private void FixedUpdate()
    {
        // 生成された後、重力をかけ続ける
        m_Rigidbody.velocity = m_Rigidbody.velocity + new Vector3(0f,fGravity,0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Playerと武器だったらスルーする
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Weapon")
            return;

        // SE再生
        IS_AudioManager.instance.PlaySE(SEType.SE_HitSkillIcon);
        // エフェクト再生
        ParticleSystem Effect = Instantiate(hitEffect);
        Effect.Play();
        Effect.transform.position = this.transform.position;
        Effect.transform.localScale = new Vector3(20f, 20f, 10f);
        Destroy(Effect.gameObject, 1.0f); // 1秒後に消える

        RaycastHit[] hits = Physics.SphereCastAll(
    transform.position,
    2.0f,
    Vector3.forward);

        foreach (var hit in hits)
        {
            if(hit.collider.gameObject.GetComponent<NK_BossSlime>() != null &&
                !hit.collider.gameObject.GetComponent<NK_BossSlime>().GetSetDamageFlag)
            {
                hit.collider.gameObject.GetComponent<NK_BossSlime>().BossSlimeDamage(nAttackPow);
                hit.collider.transform.GetComponent<YK_TakeDamage>().Damage(hit.collider, nAttackPow);
                continue;
            }

            if (hit.collider.gameObject.GetComponent<NK_Slime>() != null &&
                !hit.collider.gameObject.GetComponent<NK_Slime>().GetSetDamageFlag)
            {
                hit.collider.gameObject.GetComponent<NK_Slime>().SlimeDamage(nAttackPow);
                hit.collider.transform.GetComponent<YK_TakeDamage>().Damage(hit.collider, nAttackPow);
                continue;
            }

            if (hit.collider.gameObject.GetComponent<NK_Bat>() != null &&
                !hit.collider.gameObject.GetComponent<NK_Bat>().GetSetDamageFlag)
            {
                hit.collider.gameObject.GetComponent<NK_Bat>().BatDamage(nAttackPow);
                hit.collider.transform.GetComponent<YK_TakeDamage>().Damage(hit.collider, nAttackPow);
                continue;
            }

            if (hit.collider.gameObject.GetComponent<NK_SlimeBes>() != null)
            {
                hit.collider.gameObject.GetComponent<NK_SlimeBes>().BesDamage(nAttackPow);
                hit.collider.transform.GetComponent<YK_TakeDamage>().Damage(hit.collider, nAttackPow);
                continue;
            }
        }

        // 自身のオブジェクトを削除
        Destroy(this.gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
