/**
 * @file   IS_AudioManager.cs
 * @brief  Playerのクラス
 * @author IharaShota
 * @date   2023/04/17
 * @Update 2023/04/17 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================================
// SEType
// … SEの種類を管理する列挙体
// ===============================================
public enum SEType
{
    SE_PlayerWalk,   // Playerの歩行SE
    SE_PlayerJump,   // Playerの跳躍SE
    SE_PlayerLanding,// Playerの着地SE
    SE_FireHPBar,    // HPBarの攻撃SE
    SE_FireSkillIcon,// SkillIconの攻撃SE
    SE_HitHPBar,     // HPBarのヒットSE
    SE_HitSkillIcon, // SkillIconのヒットSE
    SE_GameClear,    // ゲームクリアSE

    MAX_SE
}

// ===============================================
// BGMType
// … BGMの種類を管理する列挙体
// ===============================================
public enum BGMType
{
    BGM_Title, // タイトルBGM
    BGM_Game,  // ゲームBGM
    BGM_END,   // エンドBGM

    MAX_BGM
}
public class IS_AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> SESources; // SEのデータ
    [SerializeField] private List<AudioSource> BGMSources;// BGMのデータ
    [SerializeField] private SEType m_eSeType;            // SEのタイプ
    [SerializeField] private BGMType m_eBGMType;          // BGMのタイプ
    public static IS_AudioManager instance;               // IS_AudioManagerのインスタンス

    /**
     * @fn
     * 初期化処理(外部参照を除く)
     * @brief  メンバ初期化処理
     * @detail 特に無し
     */
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("AudioManagerはすでに存在しています");
            Destroy(gameObject);
        }
    }

    /**
     * @fn
     * 初期化処理(外部参照あり)
     * @brief  メンバ初期化処理
     * @detail SEとBGMの数があっているか確認する
     */
    private void Start()
    {
        // SEと列挙型の数が違えばログ出力
        if (SESources.Count != (int)SEType.MAX_SE)
        {
            Debug.Log("SEとSETypeの数が合いません");
        }

        // BGMと列挙型の数が違えばログ出力
        if (BGMSources.Count != (int)BGMType.MAX_BGM)
        {
            Debug.Log("BGMとBGMTypeの数が合いません");
        }
    }

    /**
     * @fn
     * SE再生
     * @param seType … SEの種類
     * @brief  SEの種類を指定して再生
     * @detail SEとseTypeの数があっていることが前提
     */
    public void PlaySE(SEType seType)
    {
        for (int i = 0, size = (int)SEType.MAX_SE; i < size; ++i)
        {
            if (i == (int)seType)
            {
                SESources[i].Play();
                return;
            }
        }
    }

    /**
     * @fn
     * BGM再生
     * @param bgmType … BGMの種類
     * @brief  BGMの種類を指定して再生
     * @detail BGMとbgmTypeの数があっていることが前提
     */
    public void PlayBGM(BGMType bgmType)
    {
        for (int i = 0, size = (int)BGMType.MAX_BGM; i < size; ++i)
        {
            if (i == (int)bgmType)
            {
                BGMSources[i].Play();
                return;
            }
        }
    }

    /**
     * @fn
     * BGMストップ
     * @param bgmType … BGMの種類
     * @brief  BGMの種類を指定してストップ
     * @detail BGMとbgmTypeの数があっていることが前提
     */
    public void StopBGM(BGMType bgmType)
    {
        for (int i = 0, size = (int)BGMType.MAX_BGM; i < size; ++i)
        {
            if (i == (int)bgmType)
            {
                BGMSources[i].Stop();
                return;
            }
        }
    }

    /**
     * @fn
     * BGM全ストップ
     * @param なし
     * @brief  BGM全ストップ
     * @detail BGMとbgmTypeの数があっていることが前提
     */
    public void AllStopBGM()
    {
        for (int i = 0, size = (int)BGMType.MAX_BGM; i < size; ++i)
        {
            BGMSources[i].Stop();
        }
    }
}
