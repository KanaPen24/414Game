/**
 * @file   IS_PlayerParam.cs
 * @brief  Playerのパラメータクラス
 * @author IharaShota
 * @date   2023/06/05
 * @Update 2023/06/05 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerParam : MonoBehaviour
{
    public int   m_nHP;             // 現在の体力値
    public int   m_nMaxHP;          // 最大体力値
    public int   m_nAtk;            // 攻撃力
    public int   m_nDef;            // 防御力
    public float m_fGravity;        // 重力
    public bool  m_bIncinvible;     // 無敵かどうか 
}
