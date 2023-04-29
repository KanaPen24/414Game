/**
 * @file   IS_PlayerGroundCollision.cs
 * @brief  Playerの地面判定クラス
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 作成
 * @Update 2023/04/17 SE実装
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerGroundCollision : MonoBehaviour
{
    [SerializeField] private GameObject[] m_RayPoints; // Rayを飛ばす始点(4つ)
    [SerializeField] private float m_fRayLength;       // Rayの長さ
                                                       // Rayの初期化
    private Ray[] ray;
    //Rayが当たったオブジェクトの情報を入れる箱
    private RaycastHit hit;

    private void Awake()
    {
        ray = new Ray[m_RayPoints.Length];
    }

    /**
     * @fn
     * Playerの地面判定関数
     * @brief　地面にrayが当たっていたらtrueを返す,
     *         当たっていなければfalseを返す
     */
    public bool IsGroundCollision()
    {
        // Rayの数だけ生成する
        for (int i = 0; i < m_RayPoints.Length; ++i)
        {
            //Rayの作成　　　　　　　↓Rayを飛ばす原点　　　↓Rayを飛ばす方向
            ray[i] = new Ray(m_RayPoints[i].transform.position, Vector3.down);
            Debug.DrawRay(m_RayPoints[i].transform.position, Vector3.down, Color.red, m_fRayLength);

            // 地面との当たり判定(一回でも通ればtrue)
            if (Physics.Raycast(ray[i], out hit, m_fRayLength))
            {
           
                if (hit.collider.gameObject.tag == "Floor")
                    return true;
            }
        }
        return false;
    }
}
