/**
 * @file   IS_PlayerParam.cs
 * @brief  Playerのデバッグクラス
 * @author IharaShota
 * @date   2023/06/05
 * @Update 2023/06/05 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerDebug : MonoBehaviour
{
    public bool m_bInvincibleFlg; // 無敵フラグ

    public void UpdateDebug()
    {
        if (m_bInvincibleFlg)
        {
            IS_Player.instance.GetParam().m_bIncinvible = true;
        }
    }
}
