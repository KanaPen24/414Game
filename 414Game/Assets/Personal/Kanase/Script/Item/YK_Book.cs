/**
 * @file YK_Book.cs
 * @brief 本の制御を行うスクリプト
 * 本はアイテムとして扱われ、プレイヤーと接触するとアイテム取得フラグが立つ
 * また、接触後は本を破壊。
 * アイテムの種類は「ItemType.Book」として設定。
 * スクリプトの動作には「YK_Item」クラスを継承。
 * - 作成者：吉田叶聖
 * - 作成日：2023/05/28
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YK_Book : YK_Item
{
    [SerializeField] GameObject book; // 本のゲームオブジェクト

    /**
     * @brief スタート時にアイテムの初期化。
     * アイテムのタイプを「ItemType.Book」に設定し、アイテム取得フラグを初期化。
     */
    private void Start()
    {
        m_eItemType = ItemType.Book; // アイテムのタイプ設定
        GetSetItemHit = false; // アイテム取得フラグの初期化
    }

    /**
     * @brief 他のコライダーとの接触時にアイテム取得フラグを立て、本を破壊
     * @param other 接触したコライダー
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetSetItemHit = true; // アイテム取得フラグを立てる
            IS_AudioManager.instance.AllStopSE();
            IS_Player.instance.GetSetPlayerState = PlayerState.PlayerWait;

            switch (IS_Player.instance.GetSetEquipState)
            {
                case EquipState.EquipHpBar:
                    IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WaitHPBar);
                    break;
                case EquipState.EquipSkillIcon:
                    IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WaitSkillIcon);
                    break;
                case EquipState.EquipBossBar:
                    IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WaitBossBar);
                    break;
                case EquipState.EquipClock:
                    IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WaitClock);
                    break;
                case EquipState.EquipStart:
                    IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WaitHPBar);
                    break;
                default:
                    IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.Wait);
                    break;
            }
        }

        Destroy(book); // 本を破壊する
        //C#側のManaged Shellが残り続ける
        //微々たる量であるがメモリを食い続けている
        //なのでnullにすることでManaged Shellを消せる
        book = null;   
    }
}
