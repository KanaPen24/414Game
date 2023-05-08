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
 * @Update 2023/03/13 コントローラー対応(YK)
 * @Update 2023/03/19 武器種類追加(Ball),装備フラグのbool型追加
 * @Update 2023/03/20 武器チェンジ処理(仮)追加
 * @Update 2023/04/12 向き更新関数を追加
 * @Update 2023/04/21 無敵処理追加
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
    PlayerWait,       // 待ち状態
    PlayerWalk,       // 移動状態
    PlayerJump,       // 跳躍状態
    PlayerDrop,       // 落下状態
    PlayerAttack,     // 攻撃状態
    //PlayerJumpAttack, // 跳躍攻撃状態

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
// EquipWeaponState
// … 装備武器を管理する列挙体
// ※m_Weaponsはこの順番になるように入れること
// ================================================
public enum EquipWeaponState
{
    PlayerHpBar,     // HPバー
    PlayerSkillIcon, // Ball
    PlayerBossBar,   // Bossバー
    PlayerClock,     // 時計

    MaxEquipWeaponState
}

// ================================================
// PlayerEquipState
// … プレイヤーの装備状態を管理する列挙体
// ================================================
public enum PlayerEquipState
{
    NoneEquip, // 装備を外している状態
    Equip,     // 装備している状態

    MaxPlayerEquipState
}

// ================================================
// C_Invincible
// … 無敵を管理するクラス
// ================================================
public class C_Invincible
{
    private bool m_bInvincible;     // 無敵フラグ
    private float m_fInvincibleCnt; // 無敵時間
    public float m_fmeshCnt;
    public float m_fMaxMeshCnt;

    public bool GetSetInvincible
    {
        get { return m_bInvincible; }
        set { m_bInvincible = value; }
    }
    public float GetSetInvincibleCnt
    {
        get { return m_fInvincibleCnt; }
        set { m_fInvincibleCnt = value; }
    }
}

public class IS_Player : MonoBehaviour
{
    [SerializeField] private Animator                m_animator;         // Playerのアニメーション
    [SerializeField] private Rigidbody               m_Rigidbody;        // PlayerのRigidBody
    [SerializeField] private SkinnedMeshRenderer     m_PlayerMesh;       // Playerのメッシュ
    [SerializeField] private YK_CursolEvent          m_CursolEvent;      // カーソルイベントの情報
    [SerializeField] private YK_UICatcher            m_UICatcher;        // UIキャッチャー
    [SerializeField] private List<IS_PlayerStrategy> m_PlayerStrategys;  // Player挙動クラスの動的配列
    [SerializeField] private List<IS_Weapon>         m_Weapons;          // 武器クラスの動的配列
    [SerializeField] private int                     m_nHp;              // PlayerのHP
    [SerializeField] private int                     m_nMaxHp;           // Playerの最大HP
    [SerializeField] private float                   m_fGravity;         // 重力
    [SerializeField] private PlayerState             m_PlayerState;      // Playerの状態を管理する
    [SerializeField] private PlayerDir               m_PlayerDir;        // Playerの向きを管理する
    [SerializeField] private EquipWeaponState        m_EquipWeaponState; // 装備武器を管理する
    [SerializeField] private PlayerEquipState        m_PlayerEquipState; // 装備状態を管理する

    public Vector3 m_vMoveAmount; // 合計移動量(移動時や重力を加算したものをvelocityに代入する)
    public bool bInputUp;
    public bool bInputRight;
    public bool bInputLeft;
    public bool bInputSpace;

    private C_Invincible m_Invincible; // 無敵かどうか管理する
    private bool m_bWalkFlg;           // 歩行開始フラグ
    private bool m_bJumpFlg;           // 跳躍開始フラグ
    private bool m_bAttackFlg;         // 攻撃開始フラグ
    private float m_fDeadZone;   //コントローラーのスティックデッドゾーン

    private void Start()
    {
        // 挙動クラスと列挙型の数が違えばログ出力
        if(m_PlayerStrategys.Count != (int)PlayerState.MaxPlayerState)
        {
            Debug.Log("m_PlayerStarategyの要素数とm_PlayerStateの数が同じではありません");
        }

        // 武器クラスと列挙型の数が違えばログ出力
        if (m_Weapons.Count != (int)EquipWeaponState.MaxEquipWeaponState)
        {
            Debug.Log("m_PlayerWeaponsの要素数とm_PlayerWeaponStateの数が同じではありません");
        }

        // メンバの初期化
        m_vMoveAmount = new Vector3(0.0f, 0.0f, 0.0f);
        m_bWalkFlg    = false;
        m_bJumpFlg    = false;
        m_bAttackFlg  = false;
        bInputUp      = false;
        bInputRight   = false;
        bInputLeft    = false;
        bInputSpace   = false;
        m_fDeadZone = 0.2f;

        m_Invincible = new C_Invincible();
        m_Invincible.GetSetInvincible = false;
        m_Invincible.GetSetInvincibleCnt = 0f;
        m_Invincible.m_fMaxMeshCnt = 0.05f;
        m_Invincible.m_fmeshCnt = 0f;
    }

    // Update is called once per frame
    private void Update()
    {
        // 入力管理
        // Jump=Key.w,Joy.B
        if (Input.GetButtonDown("Jump"))
        {
            bInputUp = true;
        }
        else bInputUp = false;

        // 右移動
        if ((Input.GetAxis("HorizontalL")) >= m_fDeadZone)
        {
            bInputRight = true;
        }
        else bInputRight = false;

        // 左移動
        if ((Input.GetAxis("HorizontalL")) <= -m_fDeadZone)
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
            // 装備していたら…
            if(GetSetPlayerEquipState == PlayerEquipState.Equip)
            {
                // 装備解除
                RemovedWeapon();
            }
            // 装備していなかったら…
            else if(GetSetPlayerEquipState == PlayerEquipState.NoneEquip)
            {
                // 武器装備
                EquipWeapon();
            }
        }
    }
    private void FixedUpdate()
    {
        m_Rigidbody.velocity = new Vector3(0f, 0f, 0f);

        // ゲームがプレイ中以外は更新しない
        if (GameManager.instance.GetSetGameState != GameState.GamePlay)
            return;

        // Playerの状態によって更新処理
        m_PlayerStrategys[(int)GetSetPlayerState].UpdateStrategy();

        // 合計移動量をvelocityに加算
        m_Rigidbody.velocity = m_vMoveAmount;

        // 向き更新
        UpdatePlayerDir();

        // HPチェック
        CheckHP();

        // 無敵チェック
        CheckInvincible();
    }

    /**
     * @fn
     * Playerの向き更新
     * @return なし
     * @brief Playerの向き更新
     */
    private void UpdatePlayerDir()
    {
        // 向きによってモデルの角度変更
        // 右向き
        if (GetSetPlayerDir == PlayerDir.Right)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 90.0f, 0f));
        }
        // 左向き
        else if (GetSetPlayerDir == PlayerDir.Left)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, -90.0f, 0f));
        }
    }

    /**
     * @fn
     * Playerの武器装備
     * @return なし
     * @brief UICatcherに参照している
     */
    public void EquipWeapon()
    {
        // UICatcherのイベント中は処理しない
        if (m_UICatcher.GetSetUICatcherState == UICatcherState.None)
        {
            // 装備状態の場合
            if (GetSetPlayerEquipState == PlayerEquipState.NoneEquip)
            {
                // UIを武器化する
                // カーソルがUIに当たっていたら
                if (m_CursolEvent.GetSetCurrentUI != null)
                {
                    // UIを武器にするイベント開始
                    m_UICatcher.StartUI2WeaponEvent();

                    // 装備武器の初期化処理
                    m_Weapons[(int)GetSetEquipWeaponState].Init();

                    // 装備状態にする
                    GetSetPlayerEquipState = PlayerEquipState.Equip;

                    Debug.Log("武器装備");
                }
            }
        }
    }
    /**
     * @fn
     * Playerの武器解除
     * @return なし
     * @brief UICatcherに参照している
     */
    public void RemovedWeapon()
    {
        // UICatcherのイベント中は処理しない
        if (m_UICatcher.GetSetUICatcherState == UICatcherState.None)
        {
            // 装備状態の場合
            if (GetSetPlayerEquipState == PlayerEquipState.Equip)
            {
                // 装備武器の終了処理
                m_Weapons[(int)GetSetEquipWeaponState].Uninit();

                // 武器をUIにするイベント開始
                m_UICatcher.StartWeapon2UIEvent();

                // 装備を外す
                GetSetPlayerEquipState = PlayerEquipState.NoneEquip;

                Debug.Log("武器解除");
            }
        }
    }

    /**
     * @fn
     * PlayerのHPチェック(最大HP以上になっていないかなど)
     * @return なし
     * @brief なし
     */
    private void CheckHP()
    {
        // 最大HPより大きかったら…
        if (m_nHp > m_nMaxHp)
        {
            // HPを最大HPにセットする
            m_nHp = m_nMaxHp;
        }

        // HPが0以下になったら…
        if (m_nHp <= 0)
        {
            // GameOverに移行
            GameManager.instance.GetSetGameState = GameState.GameOver;
        }
    }

    /**
     * @fn
     * Playerの無敵チェック
     * @return なし
     * @brief なし
     */
    private void CheckInvincible()
    {
        // 無敵カウントが0以下だった場合…
        if(m_Invincible.GetSetInvincibleCnt <= 0f)
        {
            // メッシュを表示
            m_PlayerMesh.enabled = true;

            // 無敵状態を解除,無敵カウントを0にする
            m_Invincible.GetSetInvincible = false;
            m_Invincible.GetSetInvincibleCnt = 0f;
        }
        // 無敵カウントが0以上だった場合…
        else
        {
            // 無敵状態になる,無敵カウントを減らす
            m_Invincible.GetSetInvincible = true;
            m_Invincible.GetSetInvincibleCnt 
                = m_Invincible.GetSetInvincibleCnt - Time.deltaTime;

            // メッシュカウントを秒数加算
            m_Invincible.m_fmeshCnt += Time.deltaTime;

            // メッシュカウントが最大値を超えたら…
            if (m_Invincible.m_fmeshCnt >= m_Invincible.m_fMaxMeshCnt)
            {
                // メッシュ表示の切り替え,メッシュカウントをリセット
                m_PlayerMesh.enabled = !m_PlayerMesh.enabled;
                m_Invincible.m_fmeshCnt = 0f;
            }
        }
    }

    /**
     * @fn
     * Playerのダメージ処理
     * @param int damage       … ダメージ量
     * @param float invincible … Playerの無敵時間
     * @return なし
     * @brief Playerのダメージ処理
     */
    public void Damage(int damage,float invincibleCnt)
    {
        // 無敵状態ならダメージを受けない
        if (!m_Invincible.GetSetInvincible)
        {
            // ダメージを受ける,無敵カウントをセット
            GetSetHp -= damage;
            m_Invincible.GetSetInvincibleCnt = invincibleCnt;
        }
        else Debug.Log("NoDamage!!");
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
     * 武器クラスのgetter
     * @return m_Weapons[i]
     * @brief 武器を返す
     */
    public IS_Weapon GetWeapons(int i)
    {
        return m_Weapons[i];
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
     * 装備武器のgetter・setter
     * @return m_EquipWeaponState(EquipWeaponState)
     * @brief 装備武器を返す・セット
     */
    public EquipWeaponState GetSetEquipWeaponState
    {
        get { return m_EquipWeaponState; }
        set { m_EquipWeaponState = value; }
    }

    /**
 * @fn
 * 装備状態のgetter・setter
 * @return m_PlayerEquipState(PlayerEquipState)
 * @brief 装備状態を返す・セット
 */
    public PlayerEquipState GetSetPlayerEquipState
    {
        get { return m_PlayerEquipState; }
        set { m_PlayerEquipState = value; }
    }

    /**
     * @fn
     * HPのgetter・setter
     * @return m_nHp(int)
     * @brief HPを返す・セット
     */
    public int GetSetHp
    {
        get { return m_nHp; }
        set { m_nHp = value; }
    }

    /**
     * @fn
     * 最大HPのgetter・setter
     * @return m_nMaxHp(int)
     * @brief 最大HPを返す・セット
     */
    public int GetSetMaxHp
    {
        get { return m_nMaxHp; }
        set { m_nMaxHp = value; }
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
     * Playerの無敵クラスのgetter・setter
     * @return m_bWalkFlg(C_PlayerInvincible)
     * @brief Playerの無敵クラスを返す・セット
     */
    public C_Invincible GetSetPlayerInvincible
    {
        get { return m_Invincible; }
        set { m_Invincible = value; }
    }

    /**
     * @fn
     * 歩行開始フラグのgetter・setter
     * @return m_bWalkFlg(bool)
     * @brief 歩行開始フラグを返す・セット
     */
    public bool GetSetWalkFlg
    {
        get { return m_bWalkFlg; }
        set { m_bWalkFlg = value; }
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
