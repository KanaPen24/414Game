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
 * @Update 2023/05/28 画面外にでないようにした(YK)
 * @Update 2023/06/05 スクリプト全改修
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
    PlayerUIRelease,      // UI解放状態
    PlayerGameOver,       // ゲームオーバー状態
    PlayerJumpAttack,     // 跳躍攻撃状態

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
// EquipState
// … 装備状態を管理する列挙体
// ※m_Weaponsはこの順番になるように入れること
// ================================================
public enum EquipState
{
    EquipNone      = -1,// 無(殴るモード)
    EquipHpBar     = 0, // HPバー
    EquipSkillIcon = 1, // Ball
    EquipBossBar   = 2, // Bossバー
    EquipClock     = 3, // 時計
    EquipStart     = 4, // スタート

    MaxEquipState
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

// ===== Playerに必要なコンポーネント =========
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(IS_PlayerParam))]
[RequireComponent(typeof(IS_PlayerDebug))]
[RequireComponent(typeof(IS_PlayerFlg))]

[RequireComponent(typeof(IS_PlayerWait))]
[RequireComponent(typeof(IS_PlayerWalk))]
[RequireComponent(typeof(IS_PlayerJump))]
[RequireComponent(typeof(IS_PlayerDrop))]
[RequireComponent(typeof(IS_PlayerAttack01))]
[RequireComponent(typeof(IS_PlayerAttack02))]
[RequireComponent(typeof(IS_PlayerChargeWait))]
[RequireComponent(typeof(IS_PlayerChargeWalk))]
[RequireComponent(typeof(IS_PlayerAvoidance))]
[RequireComponent(typeof(IS_PlayerUICatch))]
[RequireComponent(typeof(IS_PlayerUIRelease))]
[RequireComponent(typeof(IS_PlayerGameOver))]
[RequireComponent(typeof(IS_PlayerJumpAttack))]

[RequireComponent(typeof(IS_PlayerGroundCollision))]
// ============================================

public class IS_Player : MonoBehaviour
{
    // Playerをインスタンス化
    static public IS_Player instance;

    // PlayerのRigidBody
    private Rigidbody m_Rigidbody;

    // Playerのアニメーション
    [SerializeField] private IS_PlayerAnimator m_PlayerAnimator;

    // Playerのパラメータ
    private IS_PlayerParam m_PlayerParam;

    // Playerのフラグ
    private IS_PlayerFlg m_PlayerFlg;

    // Playerのパラメータ
    private IS_PlayerDebug m_PlayerDebug;

    // Playerの状態を管理する
    [SerializeField] private PlayerState m_PlayerState;

    // Playerの向きを管理する
    [SerializeField] private PlayerDir m_PlayerDir;

    // 装備状態を管理する
    [SerializeField] private EquipState m_EquipState;

    // Player挙動クラスの動的配列
    [SerializeField] private List<IS_PlayerStrategy> m_PlayerStrategys;

    // 武器クラスの動的配列
    [SerializeField ]private List<IS_Weapon> m_Weapons;

    // 無敵かどうか管理する
    private C_Invincible m_Invincible;

    [SerializeField] private List<SkinnedMeshRenderer> m_PlayerMesh;       // Playerのメッシュ
    [SerializeField] private YK_CursolEvent m_CursolEvent;      // カーソルイベントの情報
    [SerializeField] private YK_UICatcher m_UICatcher;        // UIキャッチャー
    [SerializeField] private YK_CameraOut m_CameraOut;        // カメラ外判定
    public Vector3 m_vMoveAmount;  // 合計移動量(移動時や重力を加算したものをvelocityに代入する)

    // Start is called before the first frame update
    void Start()
    {
        // インスタンス化
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        // --- コンポーネントの初期化 ---
        // Playerの部分
        m_Rigidbody = GetComponent<Rigidbody>();
        m_PlayerParam = GetComponent<IS_PlayerParam>();
        m_PlayerDebug = GetComponent<IS_PlayerDebug>();
        m_PlayerFlg = GetComponent<IS_PlayerFlg>();

        // Playerの遷移コンポ―ネント
        m_PlayerStrategys.Add(GetComponent<IS_PlayerWait>());
        m_PlayerStrategys.Add(GetComponent<IS_PlayerWalk>());
        m_PlayerStrategys.Add(GetComponent<IS_PlayerJump>());
        m_PlayerStrategys.Add(GetComponent<IS_PlayerDrop>());
        m_PlayerStrategys.Add(GetComponent<IS_PlayerAttack01>());
        m_PlayerStrategys.Add(GetComponent<IS_PlayerAttack02>());
        m_PlayerStrategys.Add(GetComponent<IS_PlayerChargeWait>());
        m_PlayerStrategys.Add(GetComponent<IS_PlayerChargeWalk>());
        m_PlayerStrategys.Add(GetComponent<IS_PlayerAvoidance>());
        m_PlayerStrategys.Add(GetComponent<IS_PlayerUICatch>());
        m_PlayerStrategys.Add(GetComponent<IS_PlayerUIRelease>());
        m_PlayerStrategys.Add(GetComponent<IS_PlayerGameOver>());
        m_PlayerStrategys.Add(GetComponent<IS_PlayerJumpAttack>());

        // 武器のコンポーネント
        m_Weapons.Add(GetComponentInChildren<IS_WeaponHPBar>());
        m_Weapons.Add(GetComponentInChildren<IS_WeaponSkillIcon>());
        m_Weapons.Add(GetComponentInChildren<IS_WeaponBossBar>());
        m_Weapons.Add(GetComponentInChildren<IS_WeaponClock>());
        m_Weapons.Add(GetComponentInChildren<IS_WeaponStart>());

        m_Invincible = new C_Invincible();
        m_Invincible.GetSetInvincible = false;
        m_Invincible.GetSetInvincibleCnt = 0f;
        m_Invincible.m_fMaxMeshCnt = 0.05f;
        m_Invincible.m_fmeshCnt = 0f;
    }

    void FixedUpdate()
    {
        // 移動量を初期化
        m_Rigidbody.velocity = new Vector3(0f, 0f, 0f);

        // ゲームがプレイ中以外は更新しない
        if (GameManager.instance.GetSetGameState != GameState.GamePlay &&
            GameManager.instance.GetSetGameState != GameState.GameGoal &&
            GameManager.instance.GetSetGameState != GameState.GameOver &&
            GameManager.instance.GetSetGameState != GameState.GameStart)
            return;

        // Playerの状態によって更新処理
        m_PlayerStrategys[(int)m_PlayerState].UpdateStrategy();

        // 移動量によって移動する
        m_Rigidbody.velocity = m_vMoveAmount;

        // Playerの向きを更新・反映
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
        m_UICatcher.StartUI2WeaponEvent();

        GetWeapons((int)GetSetEquipState).Init();
    }
    /**
     * @fn
     * Playerの武器変更
     * @return なし
     * @brief UICatcherに参照している
     */
    public void ChangeWeapon()
    {
        m_UICatcher.StartChangeWeaponEvent();
        GetWeapons((int)GetSetEquipState).Init();
    }

    /**
     * @fn
     * Playerの武器解除
     * @return なし
     * @brief UICatcherに参照している
     */
    public void RemovedWeapon()
    {
        m_UICatcher.StartWeapon2UIEvent();
        GetSetEquipState = EquipState.EquipNone;
    }

    /**
     * @fn
     * Playerのダメージ処理
     * @param int damage       … ダメージ量
     * @param float invincible … Playerの無敵時間
     * @return なし
     * @brief Playerのダメージ処理
     */
    public void Damage(int damage, float invincibleCnt)
    {
        // 無敵状態ならダメージを受けない
        if (!m_Invincible.GetSetInvincible)
        {
            // ダメージを受ける,無敵カウントをセット
            GetParam().m_nHP -= damage;
            m_Invincible.GetSetInvincibleCnt = invincibleCnt;
        }
        else Debug.Log("NoDamage!!");
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
        if (GetParam().m_nHP > GetParam().m_nMaxHP)
        {
            // HPを最大HPにセットする
            GetParam().m_nHP = GetParam().m_nMaxHP;
        }

        // HPが0以下になったら…
        if (GetParam().m_nHP <= 0)
        {
            //ゲームオーバーを体力がなくなったにする
            YK_GameOver.instance.GetSetGameOverState = GameOverState.NoHP;
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
        // デバッグ用無敵
        if (GetDebug().m_bInvincibleFlg && GetSetPlayerState != PlayerState.PlayerAvoidance)
        {
            m_Invincible.GetSetInvincible = true;
            return;
        }

        // 無敵カウントが0以下だった場合…
        if (m_Invincible.GetSetInvincibleCnt <= 0f)
        {
            // メッシュを表示
            for (int i = 0, size = m_PlayerMesh.Count; i < size; ++i)
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

    public IS_PlayerParam GetParam()
    {
        return m_PlayerParam;
    }

    public IS_PlayerDebug GetDebug()
    {
        return m_PlayerDebug;
    }

    public IS_PlayerFlg GetFlg()
    {
        return m_PlayerFlg;
    }

    public YK_CursolEvent GetCursolEvent()
    {
        return m_CursolEvent;
    }

    public YK_UICatcher GetUICatcher()
    {
        return m_UICatcher;
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
     * @return m_EquipState(EquipState)
     * @brief 装備武器を返す・セット
     */
    public EquipState GetSetEquipState
    {
        get { return m_EquipState; }
        set { m_EquipState = value; }
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
     * 合計移動量のgetter・setter
     * @return m_vAmount(Vector3)
     * @brief 合計移動量を返す・セット
     */
    public Vector3 GetSetMoveAmount
    {
        get { return m_vMoveAmount; }
        set { m_vMoveAmount = value; }
    }
}
