/**
 * @file   IS_Player.cs
 * @brief  Playerのクラス
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 作成
 * @Update 2023/03/10 攻撃専用の変数追加
 * @Update 2023/03/12 Animator追加
 * @Update 2023/03/12 向きを追加
 * @Update 2023/03/12 武器を追加
 * @date   2023/03/13 コントローラー対応(YK)
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

// ================================================
// PlayerWeapon
// … Playerの武器を管理する列挙体
// ※m_PlayerWeaponはこの順番になるように入れること
// ================================================
public enum PlayerWeaponState
{
    PlayerHpBar,

    MaxPlayerWeaponState
}

public class IS_Player : MonoBehaviour
{
    [SerializeField] private GameObject              m_PlayerObj;        // Playerのモデル
    [SerializeField] private Animator                m_animator;         // Playerのアニメーション
    [SerializeField] private Rigidbody               m_Rigidbody;        // PlayerのRigidBody
    [SerializeField] private YK_HPBarVisible         m_HpVisible;        // PlayerのHp表示管理
    [SerializeField] private YK_PlayerHP             m_Hp;               // PlayerのHp
    [SerializeField] private List<IS_PlayerStrategy> m_PlayerStrategys;  // Player挙動クラスの動的配列
    [SerializeField] private List<IS_Weapon>         m_PlayerWeapons;          // 武器クラスの動的配列
    [SerializeField] private PlayerState             m_PlayerState;      // Playerの状態を管理する
    [SerializeField] private PlayerDir               m_PlayerDir;        // Playerの向きを管理する
    [SerializeField] private PlayerWeaponState       m_PlayerWeaponState;// Playerの武器状態を管理する
    [SerializeField] private float                   m_fGravity;         // 重力

    public Vector3 m_vMoveAmount; // 合計移動量(移動時や重力を加算したものをvelocityに代入する)
    public bool bInputUp;
    public bool bInputRight;
    public bool bInputLeft;
    public bool bInputSpace;

    public int nWeaponState;     // 武器状態をint型で格納する
    private bool m_bJumpFlg;      // 跳躍開始フラグ
    private bool m_bAttackFlg;    // 攻撃開始フラグ
    private float m_fDeadZone;    //コントローラーのスティックデッドゾーン

    private void Start()
    {
        // 挙動クラスと列挙型の数が違えばログ出力
        if(m_PlayerStrategys.Count != (int)PlayerState.MaxPlayerState)
        {
            Debug.Log("m_PlayerStarategyの要素数とm_PlayerStateの数が同じではありません");
        }

        // 武器クラスと列挙型の数が違えばログ出力
        if (m_PlayerWeapons.Count != (int)PlayerWeaponState.MaxPlayerWeaponState)
        {
            Debug.Log("m_PlayerWeaponsの要素数とm_PlayerWeaponStateの数が同じではありません");
        }

        // メンバの初期化
        m_vMoveAmount = new Vector3(0.0f, 0.0f, 0.0f);
        m_bJumpFlg    = false;
        bInputUp      = false;
        bInputRight   = false;
        bInputLeft    = false;
        bInputSpace   = false;
        m_fDeadZone = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        // 入力管理
        // Jump=Key.w,Joy.B
        if (Input.GetButtonDown("Jump"))
        {
            bInputUp = true;
        }
        else bInputUp = false;

        // 右移動
        if ((Input.GetAxis("Horizontal")) >= m_fDeadZone)
        {
            bInputRight = true;
        }
        else bInputRight = false;

        // 左移動
        if ((Input.GetAxis("Horizontal")) <= -m_fDeadZone)
        {
            bInputLeft = true;
        }
        else bInputLeft = false;

        // Atk=Key.Spsce,Joy.X
        if (Input.GetButtonDown("Atk"))
        {
            bInputSpace = true;
        }
        else bInputSpace = false;

        // Decision=Key.Z,Joy.A
        if (Input.GetButtonDown("Decision"))
        {
            m_HpVisible.GetSetVisible = !m_HpVisible.GetSetVisible;
        }
    }
    private void FixedUpdate()
    {
        // 現在のPlayerの状態をint型に格納
        int nPlayerState = (int)GetSetPlayerState;

        // 現在のPlayerの武器状態をint型に格納
        nWeaponState = (int)GetSetPlayerWeaponState;

        // Playerの状態によって更新処理
        m_PlayerStrategys[nPlayerState].UpdateStrategy();

        // 合計移動量をvelocityに加算
        m_Rigidbody.velocity = m_vMoveAmount;

        // 向きによってモデルの角度変更
        // 右向き
        if(GetSetPlayerDir == PlayerDir.Right)
        {
            m_PlayerObj.transform.rotation = Quaternion.Euler(new Vector3(0f, 90.0f, 0f));
        }
        // 左向き
        else if (GetSetPlayerDir == PlayerDir.Left)
        {
            m_PlayerObj.transform.rotation = Quaternion.Euler(new Vector3(0f, -90.0f, 0f));
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
     * PlayerのHp表示のgetter
     * @return m_HpVisible(YK_HPBarVisible)
     * @brief PlayerのYK_HPBarVisibleを返す
     */
    public YK_HPBarVisible GetHPVisible()
    {
        return m_HpVisible;
    }

    /**
     * @fn
     * PlayerのHp管理のgetter
     * @return m_Hp(YK_PlayerHP)
     * @brief PlayerのYK_PlayerHPを返す
     */
    public YK_PlayerHP GetPlayerHp()
    {
        return m_Hp;
    }

    /**
     * @fn
     * 武器クラスのgetter
     * @return m_Weapons[i]
     * @brief 武器を返す
     */
    public IS_Weapon GetWeapons(int i)
    {
        return m_PlayerWeapons[i];
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
     * Playerの武器状態のgetter・setter
     * @return m_PlayerWeaponState
     * @brief Playerの武器状態を返す・セット
     */
    public PlayerWeaponState GetSetPlayerWeaponState
    {
        get { return m_PlayerWeaponState; }
        set { m_PlayerWeaponState = value; }
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
