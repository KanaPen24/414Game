/**
 * @file   NK_SlimeStrategy.cs
 * @brief  Slimeの挙動基底クラス
 * @author NaitoKoki
 * @date   2023/04/17
 * @Update 2023/04/17 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_SlimeStrategy : MonoBehaviour
{
    public virtual void UpdateStrategy()
    {
        // ここにStateごとに処理を加える
    }
}