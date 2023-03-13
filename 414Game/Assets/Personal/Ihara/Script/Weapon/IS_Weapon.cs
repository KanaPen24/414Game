/**
 * @file   IS_Weapon.cs
 * @brief  •Ší‚ÌƒNƒ‰ƒX
 * @author IharaShota
 * @date   2023/03/12
 * @Update 2023/03/12 ì¬
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_Weapon : MonoBehaviour
{
    private bool m_bAttack; // UŒ‚’†‚©‚Ç‚¤‚©

    /**
     * @fn
     * UŒ‚ˆ—(override‘O’ñ)
     * @brief UŒ‚ˆ—
     */
    public virtual void Attack()
    {
        // ‚±‚±‚Éˆ—‚ğ‰Á‚¦‚é
        Debug.Log("‰F•”ˆä");
    }


    /**
     * @fn
     * UŒ‚’†‚ÌgetterEsetter
     * @return m_bAttack(bool)
     * @brief UŒ‚’†‚ğ•Ô‚·EƒZƒbƒg
     */
    public bool GetSetAttack
    {
        get { return m_bAttack; }
        set { m_bAttack = value; }
    }
}
