/**
 * @file   IS_PlayerGameOver.cs
 * @brief  Playerのゲームオーバー状態クラス
 * @author IharaShota
 * @date   2023/05/21
 * @Update 2023/05/21 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerGameOver : IS_PlayerStrategy
{
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Playerの地面判定


    private void Update()
    {
        if (IS_Player.instance.GetSetPlayerState == PlayerState.PlayerGameOver)
        {
            // ゲームオーバー → UI取得ゲームオーバー」
            if ((Input.GetKeyDown(IS_XBoxInput.LB) || Input.GetKeyDown(IS_XBoxInput.RB)) &&
            IS_Player.instance.GetUICatcher().GetSetUICatcherState == UICatcherState.None)
            {
                // カーソルがUIを取得している && カーソルが取得しているUIが現在武器にしているUIではない場合…
                // UI取得に遷移
                if (IS_Player.instance.GetCursolEvent().GetSetCurrentUI.GetSetUIType == UIType.Retry ||
                    IS_Player.instance.GetCursolEvent().GetSetCurrentUI.GetSetUIType == UIType.TitleBack)
                {
                    IS_Player.instance.GetSetPlayerState = PlayerState.PlayerUICatchGameOver;
                    IS_Player.instance.GetFlg().m_bStartUICatchFlg = true;
                    return;
                }
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
        IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.GameOver);
    }
}
