/**
 * @file   IS_WeaponHPBar.cs
 * @brief  HPBarの武器クラス
 * @author IharaShota
 * @date   2023/03/12
 * @Update 2023/03/12 作成
 * @Update 2023/04/17 SE実装
 * @Update 2023/05/11 当たり判定処理をIS_WeaponHPBarCollision.csに移動
 * @Update 2023/05/11 マテリアル切替処理追加
 * @Update 2023/05/15 液体シェーダー実装
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IS_WeaponHPBar : IS_Weapon
{
    public enum CrackLevel
    {
        Level0,
        Level1,
        Level2,
        Level3,

        MaxMaterialLevel
    }
    [System.Serializable]
    private class C_MaterialMesh
    {
        public List<MeshRenderer> m_MeshRender; // メッシュのリスト
        public List<Material> m_Material; // マテリアルのリスト
    }
    [SerializeField] private IS_Player m_Player;               // Player
    [SerializeField] private C_MaterialMesh m_MaterialMesh;    // メッシュとマテリアルのリスト
    [SerializeField] private ON_BottleLiquid m_BottleLiquid;   // 液体シェーダー
    [SerializeField] private CapsuleCollider m_CapsuleCollider;// 当たり判定
    [SerializeField] private CrackLevel m_eCrackLevel;　　　   // ヒビレベル
    private int m_nCnt;

    /**
     * @fn
     * 初期化処理(override前提)
     * @brief 初期化処理(外部参照は避けること)
     */
    protected override void Awake()
    {
        // メンバの初期化
        m_eWeaponType = WeaponType.HPBar; // 武器種類はHPバー
        m_bAttack  = false;
        m_bCharge  = false;
        m_bVisible = false;
        m_bDestroy = false;

        m_eCrackLevel = CrackLevel.Level0;

    }

   /**
    * @fn
    * 初期化処理(override前提)
    * @brief 初期化処理(外部参照する場合)
    */
    protected override void Start()
    {
        // 現在の状態に更新
        m_nCnt = Convert.ToInt32(m_bVisible);

        // 表示更新
        UpdateVisible();
    }

    private void Update()
    {
        // 前回と状態が違ったら
        if (m_nCnt != Convert.ToInt32(m_bVisible))
        {
            UpdateVisible();
        }

        // 現在の状態に更新
        m_nCnt = Convert.ToInt32(m_bVisible);

        //攻撃中だったら当たり判定をON
        if (GetSetAttack)
        {
            m_CapsuleCollider.enabled = true;
        }
        else m_CapsuleCollider.enabled = false;

        // 液体の量をPlayerのHPに依存させる
        m_BottleLiquid.ChangeFillingRate((float)(IS_Player.instance.GetParam().m_nHP / 100.0f));

        if(m_nHp > m_nMaxHp)
        {
            m_nHp = m_nMaxHp;
        }
    }

    /**
     * @fn
     * 初期化処理(override前提)
     * @brief 初期化処理
     */
    public override void Init()
    {

    }

    /**
     * @fn
     * 終了処理(override前提)
     * @brief 終了処理
     */
    public override void Uninit()
    {

    }

    /**
    * @fn
    * 攻撃初期化処理(override)
    * @brief 攻撃初期化処理
    */
    public override void StartAttack()
    {
        // SE再生
        IS_AudioManager.instance.PlaySE(SEType.SE_FireHPBar);

        // 攻撃ON
        GetSetAttack = true;
    }

    /**
     * @fn
     * 攻撃終了処理(override)
     * @brief 攻撃終了処理
     */
    public override void FinAttack()
    {
        GetSetAttack = false; // 攻撃OFF
    }

    /**
     * @fn
     * 攻撃更新処理(override)
     * @brief 攻撃処理
     */
    public override void UpdateAttack()
    {
        switch(IS_Player.instance.GetSetPlayerState)
        {
            case PlayerState.PlayerAttack01:
                if(IS_Player.instance.GetPlayerAnimator().AnimEnd(PlayerAnimState.Attack01HPBar))
                    FinAttack();
                break;
            case PlayerState.PlayerAttack02:
                if (IS_Player.instance.GetPlayerAnimator().AnimEnd(PlayerAnimState.Attack02HPBar))
                    FinAttack();
                break;
            default:
                break;
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
            m_CapsuleCollider.enabled = true;
            for (int i = 0, size = m_MaterialMesh.m_MeshRender.Count; i < size; ++i)
            {
                m_MaterialMesh.m_MeshRender[i].enabled = true;
            }
        }
        // 非表示状態だったら
        else
        {
            m_CapsuleCollider.enabled = false;
            for (int i = 0, size = m_MaterialMesh.m_MeshRender.Count; i < size; ++i)
            {
                m_MaterialMesh.m_MeshRender[i].enabled = false;
            }
        }
    }

    /**
     * @fn
     * ヒビレベルを変更
     * @return なし
     * @brief ヒビレベルを変更し、マテリアルを変える
     */
    public void ChangeCrackLevel(CrackLevel cracklevel)
    {
        m_eCrackLevel = cracklevel;

        // マテリアル切替(耐久度によってHPBarにヒビが入る)
        Material[] mats = m_MaterialMesh.m_MeshRender[0].materials;
        mats[1] = m_MaterialMesh.m_Material[(int)m_eCrackLevel];
        m_MaterialMesh.m_MeshRender[0].materials = mats;

        m_BottleLiquid.SetBrokeTex((int)m_eCrackLevel);
    }
}
