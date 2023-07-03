/**
 * @file   IS_PlayerUICatchGameOver.cs
 * @brief  PlayerのUI取得ゲームオーバー状態クラス
 * @author IharaShota
 * @date   2023/06/26
 * @Update 2023/06/26 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerUICatchGameOver : IS_PlayerStrategy
{
    private void Update()
    {
        if (IS_Player.instance.GetSetPlayerState == PlayerState.PlayerUICatchGameOver)
        {
            // UI取得開始時だったら
            if (IS_Player.instance.GetFlg().m_bStartUICatchFlg)
            {
                IS_Player.instance.GetPlayerAnimator().ParticlePlay();
                IS_Player.instance.GetCursolEvent().GetSetCurrentUI.UIFadeOUT();

                IS_Player.instance.GetFlg().m_bStartUICatchFlg = false;
            }
        }
    }

    /**
     * @fn
     * 更新処理
     * @brief  Playerの待機更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateStrategy()
    {
        // アニメーション更新
        UpdateAnim();

        // 合計移動量をリセット
        IS_Player.instance.m_vMoveAmount = new Vector3(0f, 0f, 0f);
    }

    /**
     * @fn
     * アニメーション更新処理
     * @brief Playerのアニメーション更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateAnim()
    {
        // 何もしない
    }
}
