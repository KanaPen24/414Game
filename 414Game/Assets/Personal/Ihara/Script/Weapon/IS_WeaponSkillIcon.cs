/**
 * @file   IS_WeaponSkillIcon.cs
 * @brief  SkillIconの武器クラス
 * @author IharaShota
 * @date   2023/03/18
 * @Update 2023/03/18 作成
 * @Update 2023/05/05 エフェクト実装
 * @Update 2023/05/08 反動時間実装
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IS_WeaponSkillIcon : IS_Weapon
{
    private enum ChargeLevel
    {
        ChargeLevel_0,
        ChargeLevel_1,
        ChargeLevel_2,
        ChargeLevel_3,

        MaxChargeLevel
    }

    [SerializeField] private IS_Player Player;          // Player    
    [SerializeField] private IS_Ball m_Ball;            // 生成Ball
    [SerializeField] private ParticleSystem m_ChargeEffect; // 溜めエフェクト
    [SerializeField] private YK_UICatcher m_UICatcher;  // UIキャッチャー
    [SerializeField] private MeshRenderer m_MeshRender; // メッシュ
    [SerializeField] private float m_fMaxPow;           // 最大攻撃速度
    [SerializeField] private float m_fMinPow;           // 最小攻撃速度
    [SerializeField] private float m_fMaxChargeTime;    // 最大溜め時間
    [SerializeField] private float m_fPlayerMovePow;    // プレイヤーの移動量
    [SerializeField] private float m_fMaxReactionTime;  // 最大反動時間
    [SerializeField] private ChargeLevel m_ChargeLevel; // 溜め段階を管理する

    private int   m_nCnt;               // 表示確認用
    private bool  m_bChargeFlg;         // 溜めフラグ
    private float m_fCurrentChargeTime; // 現在の溜め時間
    private float m_fCurrentPow;        // 現在の力
    private float m_fReactionTime;      // 反動時間         
    private YK_SkillIcon m_YKSkillIcon;

    /**
     * @fn
     * 初期化処理(override前提)
     * @brief 初期化処理(外部参照は避けること)
     */
    protected override void Awake()
    {
        // メンバの初期化
        m_ChargeLevel = ChargeLevel.ChargeLevel_0;

        m_eWeaponType = WeaponType.SkillIcon; // 武器種類はBall
        m_bAttack = false;
        m_bVisible = false;
        m_bDestroy = false;

        m_nCnt = 0;
        m_bChargeFlg = false;
        m_fCurrentChargeTime = 0f;
        m_fCurrentPow = 0f;

        m_YKSkillIcon = null;
    }

    /**
     * @fn
     * 初期化処理(override前提)
     * @brief 初期化処理(外部参照する場合)
     */
    protected override void Start()
    {
        m_ChargeEffect.Stop();
        // 現在の状態に更新
        m_nCnt = Convert.ToInt32(m_bVisible);

        // 表示更新
        UpdateVisible();
    }

    private void Update()
    {
        if(Player.GetSetEquipWeaponState == EquipWeaponState.PlayerSkillIcon)
        {
            if (Input.GetButton("Atk"))
            {
                m_bChargeFlg = true;
            }
            else m_bChargeFlg = false;
        }
        // 前回と状態が違ったら
        if (m_nCnt != Convert.ToInt32(m_bVisible))
        {
            UpdateVisible();
        }

        // 現在の状態に更新
        m_nCnt = Convert.ToInt32(m_bVisible);

        // 反動時間があれば
        if (m_fReactionTime > 0f)
        {
            m_fReactionTime -= Time.deltaTime;
            Player.GetSetReactionFlg = true;
        }
        else
        {
            m_fReactionTime = 0f;
            Player.GetSetReactionFlg = false;
        }
    }

    /**
     * @fn
     * 初期化処理(override前提)
     * @brief 初期化処理
     */
    public override void Init()
    {
        // 選択したUIからYK_SkillIconがあったら…
        if (m_UICatcher.GetSetSelectUI.GetComponent<YK_SkillIcon>() != null)
        {
            // 予めそのUIを保存しておき,弾数をセットする
            m_YKSkillIcon = m_UICatcher.GetSetSelectUI.GetComponent<YK_SkillIcon>();
            m_nHp = m_YKSkillIcon.GetSetStuck;
        }
        else
        {
            // 装備解除
            Player.RemovedWeapon();
            return;
        }
    }

    /**
     * @fn
     * 終了処理(override前提)
     * @brief 終了処理
     */
    public override void Uninit()
    {
        GetSetAttack = false; // 攻撃OFF
        m_ChargeLevel = ChargeLevel.ChargeLevel_0; // 溜め段階を0にする
        m_fCurrentPow = 0f;
        m_fCurrentChargeTime = 0f;

        // ストック数をUIの方にも反映させる
        IS_UIManager.instance.FindUI(m_YKSkillIcon).GetComponent<YK_SkillIcon>().GetSetStuck = m_nHp;
    }

    /**
    * @fn
    * 攻撃初期化処理(override)
    * @brief 攻撃初期化処理
    */
    public override void StartAttack()
    {
        GetSetAttack = true; // 攻撃ON
        IS_AudioManager.instance.PlaySE(SEType.SE_Charge);
        m_ChargeEffect.Play();
        m_ChargeEffect.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    /**
     * @fn
     * 攻撃終了処理(override)
     * @brief 攻撃終了処理
     */
    public override void FinAttack()
    {
        GetSetAttack = false; // 攻撃OFF
        m_ChargeLevel = ChargeLevel.ChargeLevel_0; // 溜め段階を0にする
        m_fCurrentPow = 0f;
        m_fCurrentChargeTime = 0f;
        m_ChargeEffect.Stop();
        IS_AudioManager.instance.StopSE(SEType.SE_Charge);

        // 攻撃終了時に弾数が0だったら
        if (m_nHp <= 0)
        {
            // 装備解除する
            Player.RemovedWeapon();
        }
    }

    /**
     * @fn
     * 攻撃更新処理(override)
     * @brief 攻撃処理
     */
    public override void UpdateAttack()
    {
        // 溜め
        ChargePow();

        // 溜め段階チェック
        CheckChargeLevel();

        // 攻撃チェック
        CheckAttack();

        //溜め中は移動できる!!
        // 右向き
        if (Player.bInputRight)
        {
            Player.m_vMoveAmount.x += m_fPlayerMovePow;
            Player.GetSetPlayerDir = PlayerDir.Right;
        }
        // 左向き
        if (Player.bInputLeft)
        {
            Player.m_vMoveAmount.x -= m_fPlayerMovePow;
            Player.GetSetPlayerDir = PlayerDir.Left;
        }

        // 溜めエフェクトの位置更新
        if(Player.GetSetPlayerDir == PlayerDir.Right)
        {
            m_ChargeEffect.transform.position = Player.transform.position + new Vector3(0.5f, 1.1f, 0f);
        }
        else if (Player.GetSetPlayerDir == PlayerDir.Left)
        {
            m_ChargeEffect.transform.position = Player.transform.position + new Vector3(-0.5f, 1.1f, 0f);
        }
    }

    /**
     * @fn
     * 表示更新処理(override)
     * @brief 表示更新処理
     */
    public override void UpdateVisible()
    {
        // 表示状態だったら
        if (m_bVisible)
        {
            m_MeshRender.enabled = true;
        }
        // 非表示状態だったら
        else
        {
            m_MeshRender.enabled = false;
        }
    }

    /**
     * @fn
     * 攻撃溜め処理
     * @brief 攻撃溜め処理
     */
    private void ChargePow()
    {
        // 溜めフラグが立っていたら…
        if (m_bChargeFlg)
        {
            // 力を溜める
            m_fCurrentPow = m_fMinPow + (m_fMaxPow - m_fMinPow) * (m_fCurrentChargeTime / m_fMaxChargeTime);

            // 最大溜め時間を超えていたら…
            if (m_fCurrentChargeTime >= m_fMaxChargeTime)
            {
                m_fCurrentChargeTime = m_fMaxChargeTime;
            }
            else m_fCurrentChargeTime += Time.deltaTime;
        }
    }

    /**
     * @fn
     * 攻撃溜め処理
     * @brief 攻撃溜め処理
     */
    private void CheckChargeLevel()
    {
        // 3段階目チェック
        if(m_fCurrentChargeTime >= m_fMaxChargeTime)
        {
            if(m_ChargeLevel != ChargeLevel.ChargeLevel_3)
            {
                m_ChargeLevel = ChargeLevel.ChargeLevel_3;
                IS_AudioManager.instance.PlaySE(SEType.SE_ChargeLevel_3);
                IS_AudioManager.instance.StopSE(SEType.SE_Charge);
            }
            return;
        }

        // 2段階目チェック
        if (m_fCurrentChargeTime >= m_fMaxChargeTime / 2.0f)
        {
            if (m_ChargeLevel != ChargeLevel.ChargeLevel_2)
            {
                m_ChargeLevel = ChargeLevel.ChargeLevel_2;
                IS_AudioManager.instance.PlaySE(SEType.SE_ChargeLevel_2);
            }

            return;
        }

        // 1段階目チェック
        if (m_fCurrentChargeTime >= m_fMaxChargeTime / 4.0f)
        {
            if (m_ChargeLevel != ChargeLevel.ChargeLevel_1)
            {
                m_ChargeLevel = ChargeLevel.ChargeLevel_1;
                IS_AudioManager.instance.PlaySE(SEType.SE_ChargeLevel_1);
            }

            return;
        }
    }

    /**
     * @fn
     * 攻撃チェック処理
     * @brief 攻撃チェック処理 … 溜めフラグが立っていなかったら攻撃開始
     */
    private void CheckAttack()
    {
        // 溜めフラグが立っていなかったら…
        if (!m_bChargeFlg)
        {
            // 弾を生成し攻撃開始!!
            IS_AudioManager.instance.PlaySE(SEType.SE_FireSkillIcon);
            GameObject shot = Instantiate(m_Ball.gameObject, this.transform.position, this.transform.rotation); // Ball生成
            IS_Ball Shot = shot.GetComponent<IS_Ball>();   // コンポーネントの取得
            Shot.Fire(m_fCurrentPow, Player.GetSetPlayerDir); // 弾発射

            // 弾数を減らす
            m_nHp--;

            // 反動時間付与
            m_fReactionTime = m_fMaxReactionTime;

            // 攻撃終了
            FinAttack();
        }
    }
}
