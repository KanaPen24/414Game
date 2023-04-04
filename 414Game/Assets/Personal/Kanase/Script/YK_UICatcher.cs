/**
 * @file   YK_UICatcher.cs
 * @brief  UIをとるための関数たち
 * @author 吉田叶聖
 * @date   2023/03/18
 * @Update 2023/04/03 UIを武器化する際のエフェクト処理修正(Ihara)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UICatcherState
{
    None,
    UI2Weapon,
    Weapon2UI,

    MaxUICatcherState
}

public class YK_UICatcher : MonoBehaviour
{
    [SerializeField] private List<YK_UI> m_Uis;
    [SerializeField] private ParticleSystem particleUI; //UI用エフェクトオブジェクト
    [SerializeField] private ParticleSystem particlePL; //プレイヤー用エフェクトオブジェクト
    [SerializeField] private GameObject PortalObjUI;    //UIの歪みのオブジェクト
    [SerializeField] private GameObject PortalObjPL;    //プレイヤーの歪みのオブジェクト
    [SerializeField] private GameObject Hand;           //手のオブジェクト
    [SerializeField] private IS_Player Player;          //プレイヤーの情報
    [SerializeField] private YK_CursolEvent CursolEvent;//カーソルイベント

    private bool m_bParticleFlg;                        //パーティクルエフェクト用のフラグ
    private UICatcherState m_UICatcherState;            // UICatcherの状態
    private YK_UI m_SelectUI;                           // 選択中のUI(現在武器化しているUI)

    // Start is called before the first frame update
    void Start()
    {
        ParticleStop();
        m_bParticleFlg = false;
        m_SelectUI = null;
    }

    private void Update()
    {
        //プレイヤーの向き比較
        if(Player.GetSetPlayerDir == PlayerDir.Left)
        {
            particlePL.transform.position = Player.transform.position + new Vector3(-0.5f, 1.0f, 0.0f);
        }
        else if(Player.GetSetPlayerDir == PlayerDir.Right)
        {
            particlePL.transform.position = Player.transform.position + new Vector3(0.5f, 1.0f, 0.0f);
        }
    }

    // 1. 再生
    public void ParticlePlay()
    {
        // プレイしてなかったら再生する
        if (!m_bParticleFlg)
        {
            Hand.GetComponent<Animator>().SetBool("Hand", true);
            particleUI.Play();
            particlePL.Play();
            PortalObjUI.SetActive(true);
            PortalObjPL.SetActive(true);
            m_bParticleFlg = true;
            Debug.Log(Hand.transform.position);
        }
    }

    // 2. 一時停止
    public void ParticlePause()
    {
        particleUI.Pause();
    }

    // 3. 停止
    public void ParticleStop()
    {
        Hand.GetComponent<Animator>().SetBool("Hand", false);
        particleUI.Stop();
        particlePL.Stop();
        PortalObjUI.SetActive(false);
        PortalObjPL.SetActive(false);
        m_bParticleFlg = false;
        m_UICatcherState = UICatcherState.None;
    }

    //アニメーション動かす
    public void AnimationPlay()
    {
        // アニメーターで動かす
        //
    }

    /**
     * @fn
     * UICatcherの状態のgetter・setter
     * @return m_UICatcherState(UICatcherState)
     * @brief UICatcherの状態を返す・セット
     */
    public UICatcherState GetSetUICatcherState
    {
        get { return m_UICatcherState; }
        set { m_UICatcherState = value; }
    }

    /**
     * @fn
     * UIを武器化するイベントの発生
     * @return なし
     * @brief UIを武器化するイベント発生
     */
    public void StartUI2WeaponEvent()
    {
        m_UICatcherState = UICatcherState.UI2Weapon; // UIから武器化するイベント状態にする
        ParticlePlay(); // エフェクト再生


        // 選択したUIを探す
        for(int i = 0,size = m_Uis.Count; i < size;++i)
        {
            if(CursolEvent.GetSetCurrentUI.gameObject == m_Uis[i].gameObject)
            {
                m_SelectUI = m_Uis[i];
                particleUI.GetComponent<Transform>().position = m_SelectUI.GetSetPos;
                Hand.transform.position = m_SelectUI.GetSetPos;
                m_SelectUI.UIFadeOUT();
                break;
            }
        }

        // 武器を装備する(武器チェンジ)
        for(int i = 0,size = (int)EquipWeaponState.MaxEquipWeaponState; i < size;++i)
        {
            if((int)m_SelectUI.GetSetUIType == i)
            {
                Player.GetSetEquipWeaponState = (EquipWeaponState)i;
                break;
            }

        }
    }

    /**
     * @fn
     * 武器をUIにするイベントの発生
     * @return なし
     * @brief 武器をUIにするイベント発生(特に今のところ処理はなし)
     */
    public void StartWeapon2UIEvent()
    {
        //m_UICatcherState = UICatcherState.Weapon2UI;
        m_SelectUI.UIFadeIN();
        m_SelectUI = null;
    }
}
