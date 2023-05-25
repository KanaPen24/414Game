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
 * @Update 2023/05/08 反動フラグ追加
 * @Update 2023/05/12 武器チェンジ処理追加
 * @Update 2023/05/21 GameOver状態,回避状態処理実装
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
    PlayerWait,           // 待ち状態
    PlayerWalk,           // 移動状態
    PlayerJump,           // 跳躍状態
    PlayerDrop,           // 落下状態
    PlayerAttack01,       // 攻撃01状態
    PlayerAttack02,       // 攻撃02状態
    PlayerChargeWait,     // 溜め待機状態
    PlayerChargeWalk,     // 溜め移動状態
    PlayerAvoidance,      // 回避状態
    PlayerUICatch,        // UI取得状態
    PlayerGameOver,       // ゲームオーバー状態

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
    [SerializeField] private IS_PlayerAnimator         m_PlayerAnimator;   // Playerのアニメーション
    [SerializeField] private Rigidbody                 m_Rigidbody;        // PlayerのRigidBody
    [SerializeField] private List<SkinnedMeshRenderer> m_PlayerMesh;       // Playerのメッシュ
    [SerializeField] private YK_CursolEvent            m_CursolEvent;      // カーソルイベントの情報
    [SerializeField] private YK_UICatcher              m_UICatcher;        // UIキャッチャー
    [SerializeField] private List<IS_PlayerStrategy>   m_PlayerStrategys;  // Player挙動クラスの動的配列
    [SerializeField] private List<IS_Weapon>           m_Weapons;          // 武器クラスの動的配列
    [SerializeField] private int                       m_nHp;              // PlayerのHP
    [SerializeField] private int                       m_nMaxHp;           // Playerの最大HP
    [SerializeField] private float                     m_fGravity;         // 重力
    [SerializeField] private PlayerState               m_PlayerState;      // Playerの状態を管理する
    [SerializeField] private PlayerDir                 m_PlayerDir;        // Playerの向きを管理する
    [SerializeField] private EquipWeaponState          m_EquipWeaponState; // 装備武器を管理する
    [SerializeField] private PlayerEquipState          m_PlayerEquipState; // 装備状態を管理する

    public Vector3 m_vMoveAmount; // 合計移動量(移動時や重力を加算したものをvelocityに代入する)
    public bool bInputUp;
    public bool bInputRight;
    public bool bInputLeft;
    public bool bInputAttack;
    public bool bInputCharge;
    public bool bInputAvoid;

    private C_Invincible m_Invincible; // 無敵かどうか管理する
    private bool m_bWalkFlg;           // 歩行開始フラグ
    private bool m_bJumpFlg;           // 跳躍開始フラグ
    private bool m_bAttackFlg;         // 攻撃開始フラグ
    private bool m_bChargeWaitFlg;     // 溜め待機開始フラグ
    private bool m_bChargeWalkFlg;     // 溜め移動開始フラグ
    private bool m_bAvoidFlg;          // 回避開始フラグ
    private bool m_bReactionFlg;       // 反動フラグ
    private float m_fDeadZone;   //コントローラーのスティックデッドゾーン
    private bool m_bItemHit;    //武器回復アイテムぶつかったら

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
        m_bAvoidFlg   = false;
        m_bReactionFlg = false;
        bInputUp      = false;
        bInputRight   = false;
        bInputLeft    = false;
        bInputAttack   = false;
        bInputAvoid   = false;
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
        // Decision=Key.Z,Joy.A
        if (Input.GetButtonDown("Decision") ||
            Input.GetButtonDown("Decision_Debug"))
        {
            // 装備していないor選択しているUIがあったら…
            if (GetSetPlayerEquipState == PlayerEquipState.NoneEquip ||
                (m_CursolEvent.GetSetCurrentUI != null &&
                 m_UICatcher.GetSetSelectUI != m_CursolEvent.GetSetCurrentUI))
            {
                // 武器装備
                EquipWeapon();
            }
            // 装備していたら…
            else if (GetSetPlayerEquipState == PlayerEquipState.Equip)
            {
                // 装備解除
                RemovedWeapon();
            }
        }

        // ゲームがプレイ中以外は更新しない
        if (GameManager.instance.GetSetGameState != GameState.GamePlay &&
            GameManager.instance.GetSetGameState != GameState.GameGoal)
            return;

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
            bInputAttack = true;
        }
        else bInputAttack = false;

        // Atk=Key.Spsce,Joy.X
        if (Input.GetButton("Atk"))
        {
            bInputCharge = true;
        }
        else bInputCharge = false;

        // Aボタン
        if (Input.GetKeyDown("joystick button 0"))
        {
            bInputAvoid = true;
        }
        else bInputAvoid = false;

        // 無敵チェック
        CheckInvincible();
    }
    private void FixedUpdate()
    {
        m_Rigidbody.velocity = new Vector3(0f, 0f, 0f);

        // Playerの状態によって更新処理
        m_PlayerStrategys[(int)GetSetPlayerState].UpdateStrategy();

        // 合計移動量をvelocityに加算
        m_Rigidbody.velocity = m_vMoveAmount;

        // 向き更新
        UpdatePlayerDir();

        // HPチェック
        CheckHP();
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
            // 現在装備している武器をUIに戻す
            // ※最初の武器化では、選択しているUIがないため
            //   ここで制限をかけておく
            if(m_UICatcher.GetSetSelectUI != null)
            {
                if (m_UICatcher.GetSetSelectUI.GetSetFadeState == FadeState.FadeNone)
                    m_UICatcher.StartWeapon2UIEvent();
                else return;
            }

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

                // UI取得状態に遷移
                GetSetPlayerState = PlayerState.PlayerUICatch;

                Debug.Log("武器装備");
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
            GetSetPlayerState = PlayerState.PlayerGameOver;
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
            for(int i = 0,size = m_PlayerMesh.Count; i < size; ++i)
            {
                m_PlayerMesh[i].enabled = true;
            }

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
                for (int i = 0, size = m_PlayerMesh.Count; i < size; ++i)
                {
                    m_PlayerMesh[i].enabled = !m_PlayerMesh[i].enabled;
                }
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
     * @return m_APlayerAnimator(IS_PlayerAnimator)
     * @brief PlayerのAnimatorを返す
     */
    public IS_PlayerAnimator GetPlayerAnimator()
    {
        return m_PlayerAnimator;
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

    /**
     * @fn
     * 溜め待機開始フラグのgetter・setter
     * @return m_bChargeWaitFlg(bool)
     * @brief 溜め待機開始フラグを返す・セット
     */
    public bool GetSetChargeWaitFlg
    {
        get { return m_bChargeWaitFlg; }
        set { m_bChargeWaitFlg = value; }
    }

    /**
     * @fn
     * 溜め移動開始フラグのgetter・setter
     * @return m_bChargeWalkFlg(bool)
     * @brief 溜め移動開始フラグを返す・セット
     */
    public bool GetSetChargeWalkFlg
    {
        get { return m_bChargeWalkFlg; }
        set { m_bChargeWalkFlg = value; }
    }

    /**
     * @fn
     * 回避開始フラグのgetter・setter
     * @return m_bAvoidFlg(bool)
     * @brief 回避開始フラグを返す・セット
     */
    public bool GetSetAvoidFlg
    {
        get { return m_bAvoidFlg; }
        set { m_bAvoidFlg = value; }
    }

    /**
     * @fn
     * 反動フラグのgetter・setter
     * @return m_bReactionFlg(bool)
     * @brief 反動フラグを返す・セット
     */
    public bool GetSetReactionFlg
    {
        get { return m_bReactionFlg; }
        set { m_bReactionFlg = value; }
    }
    /**
   * @fn
   * 武器アイテムヒットフラグのgetter・setter
   * @return m_bItemHit(bool)
   * @brief 武器アイテムヒットフラグを返す・セット
   */
    public bool GetSetItemHit
    {
        get { return m_bItemHit; }
        set { m_bItemHit = value; }
    }
}
