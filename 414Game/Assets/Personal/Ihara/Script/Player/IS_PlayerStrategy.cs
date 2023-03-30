/**
 * @file   IS_PlayerStrategy.cs
 * @brief  Playerの挙動基底クラス
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerStrategy : MonoBehaviour
{
    /**
     * @fn
     * 更新処理(override前提)
     * @brief Playerの更新処理
     */
    public virtual void UpdateStrategy()
    {
        // ここにStateごとに処理を加える
    }
}
