/**
 * @file YK_PLPlayEff
 * @brief プレイヤーの状態でどのエフェクトを流すか
 * @author 吉田叶聖
 * @date 2023/05/31
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YK_PLPlayEff : MonoBehaviour
{
    [SerializeField] private ParticleSystem AtkEff; //攻撃エフェクト
    [SerializeField] private ParticleSystem AvoEff; //回避エフェクト
    [SerializeField] private IS_Player Player;

    // Update is called once per frame
    void Update()
    {
        //プレイヤーがどういう時にどのエフェクトを流すかのやつ
        switch(Player.GetSetPlayerState)
        {
            case PlayerState.PlayerAttack01:
            case PlayerState.PlayerAttack02:
                AtkEff.Play();
                break;
            case PlayerState.PlayerAvoidance:
                AvoEff.Play();
                break;
            default:
                AtkEff.Stop();
                AvoEff.Stop();
                break;
        }
    }
}
