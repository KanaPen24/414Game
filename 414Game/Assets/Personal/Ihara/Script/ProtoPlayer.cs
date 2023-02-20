// ==============================================================
// ProtoPlayer.cs
// Auther:Ihara
// Update:2023/02/20 cs作成
// ==============================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerState
{
    PlayerWait,
    PlayerMove,
    PlayerJump,
    PlayerDrop,
}

public class ProtoPlayer : MonoBehaviour
{
    [SerializeField] private CharacterController m_CharacterController; //キャラコントローラー(仮)
    [SerializeField] private Vector3 m_vMove;                           // 移動する量
    [SerializeField] private Vector3 m_vGravity;                        // 重力
    [SerializeField] private float   m_fJumpPow;                        // 跳躍力
    [SerializeField] private PlayerState m_PlayerState;                 // Playerの状態を管理する
    private Vector3 m_vMoveAmount; // 合計移動量(移動時や重力を加算したもの)        
    private bool bInputUp; 
    private bool bInputRight;
    private bool bInputLeft;

    // Start is called before the first frame update
    void Start()
    {
        // メンバの初期化
        m_PlayerState = PlayerState.PlayerDrop;
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
        if (!m_CharacterController.isGrounded)
        {
            m_vMoveAmount.x += m_vGravity.x;
            m_vMoveAmount.y += m_vGravity.y;
            m_vMoveAmount.z += m_vGravity.z;
        }

        //Debug.Log(m_CharacterController.isGrounded);

        // 合計移動量を加算
        m_CharacterController.Move(m_vMoveAmount);
    }

    // 待ち状態の更新処理
    private void UpdateWait()
    {
        // 合計移動量をリセット
        m_vMoveAmount.x = 0.0f;
        m_vMoveAmount.y = 0.0f;
        m_vMoveAmount.z = 0.0f;

        // 状態遷移
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

        // 状態遷移
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
        // 合計移動量をリセット
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
        // 合計移動量をリセット
        m_vMoveAmount.x = 0.0f;
        m_vMoveAmount.z = 0.0f;

        if (bInputRight)
        {
            m_vMoveAmount.x += m_vMove.x;
        }
        if (bInputLeft)
        {
            m_vMoveAmount.x -= m_vMove.x;
        }

        if(m_CharacterController.isGrounded)
        {
            m_PlayerState = PlayerState.PlayerWait;
        }
    }
}
