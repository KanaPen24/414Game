/**
 * @file   IS_PlayerGroundCollision.cs
 * @brief  Playerの地面判定クラス
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerGroundCollision : MonoBehaviour
{
    [SerializeField] private GameObject[] m_RayPoints; // Rayを飛ばす始点(4つ)
    [SerializeField] private float m_fRayLength;       // Rayの長さ

    /**
     * @fn
     * Playerの地面判定関数
     * @brief　地面にrayが当たっていたらtrueを返す,
     *         当たっていなければfalseを返す
     */
    public bool IsGroundCollision()
    {
        // =========================================== 
        // 変数宣言 
        // ===========================================

        // Rayの初期化
        Ray[] ray = new Ray[m_RayPoints.Length];
        //Rayが当たったオブジェクトの情報を入れる箱
        RaycastHit hit;

        // ===========================================

        // Rayの数だけ生成する
        for (int i = 0; i < m_RayPoints.Length; ++i)
        {
            //Rayの作成　　　　　　　↓Rayを飛ばす原点　　　↓Rayを飛ばす方向
            ray[i] = new Ray(m_RayPoints[i].transform.position, Vector3.down);
            Debug.DrawRay(m_RayPoints[i].transform.position, Vector3.down, Color.red, m_fRayLength);
        }

        // 地面との当たり判定(一回でも通ればtrue)
        for (int i = 0; i < m_RayPoints.Length; i++)
        {
            if (Physics.Raycast(ray[i], out hit, m_fRayLength))
            {
                return true;
            }
        }
        return false;
    }
}
