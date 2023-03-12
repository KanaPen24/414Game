/**
 * @file   IS_Player.cs
 * @brief  Playerのクラス
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 作成
 * @Update 2023/03/10 攻撃専用の変数追加
 * @Update 2023/03/12 Animator追加
 * @Update 2023/03/12 向きを追加
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================================
// PlayerState
// … Playerの状態を管理する列挙体
// ※m_PlayerStateはこの順番になるように入れること
// ===============================================
public enum PlayerState
{
    PlayerWait,   // 待ち状態
    PlayerWalk,   // 移動状態
    PlayerJump,   // 跳躍状態
    PlayerDrop,   // 落下状態
    PlayerAttack, // 攻撃状態

    MaxPlayerState
}

// ===============================================
// PlayerDir
// … Playerの向きを管理する列挙体
// ===============================================
public enum PlayerDir
{
    Left, // 左向き
    Right,// 右向き

    MaxDir
}

public class IS_Player : MonoBehaviour
{
    [SerializeField] private Animator m_animator;                         // Playerのアニメーション
    [SerializeField] private Rigidbody m_Rigidbody;                       // PlayerのRigidBody
    [SerializeField] private List<IS_PlayerStrategy> m_PlayerStrategys;   // Player挙動クラスの動的配列
    [SerializeField] private PlayerState m_PlayerState;                   // Playerの状態を管理する
    [SerializeField] private PlayerDir   m_PlayerDir;                     // Playerの向きを管理する
    [SerializeField] private float m_fGravity;                            // 重力

    public Vector3 m_vMoveAmount; // 合計移動量(移動時や重力を加算したものをvelocityに代入する)
    public bool bInputUp;
    public bool bInputRight;
    public bool bInputLeft;
    public bool bInputSpace;

    private bool m_bJumpFlg;      // 跳躍開始フラグ
    private bool m_bAttackFlg;    // 攻撃開始フラグ

    private void Start()
    {
        // 挙動クラスと列挙型の数が違えばログ出力
        if(m_PlayerStrategys.Count != (int)PlayerState.MaxPlayerState)
        {
            Debug.Log("m_PlayerStarategyの要素数とm_PlayerStateの数が同じではありません");
        }
        
        // メンバの初期化
        m_vMoveAmount = new Vector3(0.0f, 0.0f, 0.0f);
        m_bJumpFlg = false;
        bInputUp = false;
        bInputRight = false;
        bInputLeft = false;
        bInputSpace = false;
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

        if (Input.GetKey(KeyCode.Space))
        {
            bInputSpace = true;
        }
        else bInputSpace = false;
    }

    private void FixedUpdate()
    {
        // 現在のPlayerの状態をint型に格納
        int nState = (int)GetSetPlayerState;

        // Playerの状態によって更新処理
        m_PlayerStrategys[nState].UpdateStrategy();

        // 合計移動量をvelocityに加算
        m_Rigidbody.velocity = m_vMoveAmount;

        // 向きによってモデルの角度変更
        // 右向き
        if(GetSetPlayerDir == PlayerDir.Right)
        {
            this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 90.0f, 0f));
        }
        // 左向き
        else if (GetSetPlayerDir == PlayerDir.Left)
        {
            this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, -90.0f, 0f));
        }
    }

    /**
     * @fn
     * PlayerのAnimatorのgetter
     * @return m_Animator(Animator)
     * @brief PlayerのAnimatorを返す
     */
    public Animator GetAnimator()
    {
        return m_animator;
    }

    /**
     * @fn
     * PlayerのRigidbodyのgetter
     * @return m_Rigidbody(Rigidbody)
     * @brief PlayerのRIgidbodyを返す
     */
    public Rigidbody GetRigidbody()
    {
        return m_Rigidbody;
    }

    /**
     * @fn
     * Playerの状態のgetter・setter
     * @return m_PlayerState
     * @brief Playerの状態を返す・セット
     */
    public PlayerState GetSetPlayerState
    {
        get { return m_PlayerState; }
        set { m_PlayerState = value; }
    }

    /**
     * @fn
     * Playerの向きのgetter・setter
     * @return m_PlayerDir
     * @brief Playerの向きを返す・セット
     */
    public PlayerDir GetSetPlayerDir
    {
        get { return m_PlayerDir; }
        set { m_PlayerDir = value; }
    }

    /**
     * @fn
     * 重力のgetter・setter
     * @return m_fGravity(float)
     * @brief 重力を返す・セット
     */
    public float GetSetGravity
    {
        get { return m_fGravity; }
        set { m_fGravity = value; }
    }

    /**
     * @fn
     * 合計移動量のgetter・setter
     * @return m_vAmount(Vector3)
     * @brief 合計移動量を返す・セット
     */
    public Vector3 GetSetMoveAmount
    {
        get { return m_vMoveAmount; }
        set { m_vMoveAmount = value; }
    }

    /**
     * @fn
     * 跳躍開始フラグのgetter・setter
     * @return m_bJumpFlg(bool)
     * @brief 跳躍開始フラグを返す・セット
     */
    public bool GetSetJumpFlg
    {
        get { return m_bJumpFlg; }
        set { m_bJumpFlg = value; }
    }

    /**
     * @fn
     * 攻撃開始フラグのgetter・setter
     * @return m_bAttackFlg(bool)
     * @brief 攻撃開始フラグを返す・セット
     */
    public bool GetSetAttackFlg
    {
        get { return m_bAttackFlg; }
        set { m_bAttackFlg = value; }
    }
}
