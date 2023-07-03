/**
 * @file YK_PLPlayEff.cs
 * @brief プレイヤーの状態でどのエフェクトを流すか管理するスクリプト
 * @author 吉田叶聖
 * @date 2023/05/31
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YK_PLPlayEff : MonoBehaviour
{
    [SerializeField] private ParticleSystem AtkEff; // 攻撃エフェクト
    [SerializeField] private ParticleSystem AvoEff; // 回避エフェクト

    /**
     * @brief フレームごとの更新処理
     */
    void Update()
    {
        switch (IS_Player.instance.GetSetEquipState)
        {
            // 近接武器の場合のエフェクト再生判定
            case EquipState.EquipHpBar:
            case EquipState.EquipStart:

                // プレイヤーの状態に応じてエフェクトを再生するか判定する
                switch (IS_Player.instance.GetSetPlayerState)
                {
                    case PlayerState.PlayerAttack01:
                    case PlayerState.PlayerAttack02:
                        AtkEff.Play(); // 攻撃エフェクトを再生する
                        break;
                    default:
                        AtkEff.Stop(); // 攻撃エフェクトを停止する
                        break;
                }
                break;
            default:
                AtkEff.Stop(); // 攻撃エフェクトを停止する
                break;
        }

        // 回避状態の場合、回避エフェクトを再生する
        if (IS_Player.instance.GetSetPlayerState == PlayerState.PlayerAvoidance)
            AvoEff.Play();
        else
            AvoEff.Stop(); // 回避エフェクトを停止する
    }
}
