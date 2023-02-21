// ==============================================================
// ProtoPlayer.cs
// Auther:Ihara
// Update:2023/02/20 cs作成
// Update:2023/02/21 移動法を「CharacterController」から
//                   「RigidBody」に変更
// Update:2023/02/21 地面判定関数を作成(仮)
// ==============================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ================================
// PlayerState
// … Playerの状態を管理する列挙体
// ================================
enum PlayerState
{
    PlayerWait, // 待ち状態
    PlayerMove, // 移動状態
    PlayerJump, // 跳躍状態
    PlayerDrop, // 落下状態
}

public class ProtoPlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody m_Rigidbody;                     // PlayerのRigidBody
    [SerializeField] private GameObject m_RayPoint;                     // Rayを飛ばす始点
    [SerializeField] private Vector3 m_vMove;                           // 移動する量
    [SerializeField] private Vector3 m_vGravity;                        // 重力
    [SerializeField] private float   m_fJumpPow;                        // 跳躍力
    [SerializeField] private PlayerState m_PlayerState;                 // Playerの状態を管理する
    private Vector3 m_vMoveAmount; // 合計移動量(移動時や重力を加算したものをvelocityに代入する)        
    private bool bInputUp; 
    private bool bInputRight;
    private bool bInputLeft;

    // Start is called before the first frame update
    void Start()
    {
        // メンバの初期化
        bInputUp = false;
        bInputRight = false;
        bInputLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 入力管理
        if (Input.GetKey(KeyCode.W))
        {
            bInputUp = true;
        }
        else bInputUp = false;

        if (Input.GetKey(KeyCode.D))
        {
            bInputRight = true;
        }
        else bInputRight = false;

        if (Input.GetKey(KeyCode.A))
        {
            bInputLeft = true;
        }
        else bInputLeft = false;
    }

    private void FixedUpdate()
    {
        // プレイヤーの状態によって更新処理
        switch (m_PlayerState)
        {
            case PlayerState.PlayerWait:
                UpdateWait();
                break;
            case PlayerState.PlayerMove:
                UpdateMove();
                break;
            case PlayerState.PlayerJump:
                UpdateJump();
                break;
            case PlayerState.PlayerDrop:
                UpdateDrop();
                break;
        }

        // 空中にいる場合、重力を与える
        if (!IsGroundCollision())
        {
            // 重力を合計移動量に加算
            m_vMoveAmount += m_vGravity;

            if(m_PlayerState != PlayerState.PlayerJump)
            {
                m_PlayerState = PlayerState.PlayerDrop;
            }
        }
        else
        {
            if (m_PlayerState == PlayerState.PlayerDrop)
            {
                m_PlayerState = PlayerState.PlayerWait;
            }
        }

        // 合計移動量をvelocityに加算
        m_Rigidbody.velocity = m_vMoveAmount;
    }

    // 待ち状態の更新処理
    private void UpdateWait()
    {
        // 合計移動量をリセット
        m_vMoveAmount.x = 0.0f;
        m_vMoveAmount.y = 0.0f;
        m_vMoveAmount.z = 0.0f;

        // =========
        // 状態遷移
        // =========
        // 「待ち → 跳躍」
        if (bInputUp)
        {
            m_PlayerState = PlayerState.PlayerJump;
            m_vMoveAmount.y = m_fJumpPow;
            return;
        }
        // 「待ち → 移動」
        if (bInputRight || bInputLeft)
        {
            m_PlayerState = PlayerState.PlayerMove;
            return;
        }
    }

    // 移動状態の更新処理
    private void UpdateMove()
    {
        // 合計移動量をリセット
        m_vMoveAmount.x = 0.0f;
        m_vMoveAmount.y = 0.0f;
        m_vMoveAmount.z = 0.0f;

        // DAキーで移動する
        if (bInputRight)
        {
            m_vMoveAmount.x += m_vMove.x;
        }
        if (bInputLeft)
        {
            m_vMoveAmount.x -= m_vMove.x;
        }

        // =========
        // 状態遷移
        // =========
        // 「移動 → 跳躍」
        //  Wキーで跳躍する
        if (bInputUp)
        {
            m_PlayerState = PlayerState.PlayerJump;
            m_vMoveAmount.y = m_fJumpPow;
            return;
        }
        // 「移動 → 待ち」
        if (!bInputRight && !bInputLeft)
        {
            m_PlayerState = PlayerState.PlayerWait;
            return;
        }
    }

    // 跳躍状態の更新処理
    private void UpdateJump()
    {
        // 合計移動量をリセット(y成分はリセットしない)
        m_vMoveAmount.x = 0.0f;
        m_vMoveAmount.z = 0.0f;

        // DAキーで移動する
        if (bInputRight)
        {
            m_vMoveAmount.x += m_vMove.x;
        }
        if (bInputLeft)
        {
            m_vMoveAmount.x -= m_vMove.x;
        }

        // 状態遷移
        // 「跳躍 → 落下」
        if (m_vMoveAmount.y <= 0.0f)
        {
            m_PlayerState = PlayerState.PlayerDrop;
        }
    }

    // 落下状態の更新処理
    private void UpdateDrop()
    {
        // 合計移動量をリセット(y成分はリセットしない)
        m_vMoveAmount.x = 0.0f;
        m_vMoveAmount.z = 0.0f;

        // DAキーで移動する
        if (bInputRight)
        {
            m_vMoveAmount.x += m_vMove.x;
        }
        if (bInputLeft)
        {
            m_vMoveAmount.x -= m_vMove.x;
        }
    }

    // ===========================================================
    // 地面判定関数
    // 戻り値: bool型
    // … 地面にrayが当たっていたらtrueを返す,
    //    当たっていなければfalseを返す
    // ===========================================================
    private bool IsGroundCollision()
    {
        //Rayの作成　　　　　　　↓Rayを飛ばす原点　　　↓Rayを飛ばす方向
        Ray ray = new Ray(m_RayPoint.transform.position, new Vector3(0, -1, 0));

        //Rayが当たったオブジェクトの情報を入れる箱
        RaycastHit hit;

        //Rayの飛ばせる距離
        float distance = 1.0f;

        //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　　　　　↓Rayの色
        Debug.DrawLine(ray.origin, ray.direction * distance, Color.red);

        //もしRayにオブジェクトが衝突したら
        //                  ↓Ray  ↓Rayが当たったオブジェクト ↓距離
        if (Physics.Raycast(ray, out hit, distance))
        {
            return true;
        }
        else return false;
    }
}

