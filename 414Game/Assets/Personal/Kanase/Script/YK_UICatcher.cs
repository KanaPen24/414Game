/**
 * @file   YK_UICatcher.cs
 * @brief  UIをとるための関数たち
 * @author 吉田叶聖
 * @date   2023/03/18
 * @Update 2023/04/03 UIを武器化する際のエフェクト処理修正(Ihara)
 * @Update 2023/04/06 新規エフェクトによる調整(Yoshida)
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
    [SerializeField] private GameObject BlackHoleUI;    //UIの大元オブジェクト
    [SerializeField] private GameObject BlackHolePL;    //プレイヤー大元オブジェクト
    [SerializeField] private GameObject PortalObjUI;    //UIのポータルオブジェクト
    [SerializeField] private GameObject PortalObjPL;    //プレイヤーのポータルオブジェクト
    [SerializeField] private GameObject Hand;           //手のオブジェクト
    [SerializeField] private IS_Player Player;          //プレイヤーの情報
    [SerializeField] private YK_CursolEvent CursolEvent;//カーソルイベント

    private bool m_bParticleFlg;                        //パーティクルエフェクト用のフラグ
    private UICatcherState m_UICatcherState;            // UICatcherの状態
    private YK_UI m_SelectUI;                           // 選択中のUI(現在武器化しているUI)

    // Start is called before the first frame update
    void Start()
    {
        //停止の処理を呼び出す
        ParticleStop();

        //最初は消しておく
        BlackHoleUI.SetActive(false);
        BlackHolePL.SetActive(false);

        m_bParticleFlg = false;
        m_SelectUI = null;
    }

    private void Update()
    {
        //プレイヤーの向き比較
        if(Player.GetSetPlayerDir == PlayerDir.Left)
        {
            BlackHolePL.transform.position = Player.transform.position + new Vector3(-0.5f, 1.0f, 0.0f);
        }
        else if(Player.GetSetPlayerDir == PlayerDir.Right)
        {
            BlackHolePL.transform.position = Player.transform.position + new Vector3(0.5f, 1.0f, 0.0f);
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
            BlackHoleUI.SetActive(true);
            BlackHolePL.SetActive(true);
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
            // 探し出せたら…
            if(CursolEvent.GetSetCurrentUI.gameObject == m_Uis[i].gameObject)
            {
                // 予め格納する
                m_SelectUI = m_Uis[i];

                // エフェクトの位置を設定
                BlackHoleUI.GetComponent<Transform>().position = m_SelectUI.GetSetPos;
                Hand.transform.position = m_SelectUI.GetSetPos;

                // 選択したUIのフェードアウト開始
                m_SelectUI.UIFadeOUT();

                // for文から抜ける
                break;
            }
        }

        // 武器を装備する(武器チェンジ)
        for(int i = 0,size = (int)EquipWeaponState.MaxEquipWeaponState; i < size;++i)
        {
            // 番号が一致したら…
            if((int)m_SelectUI.GetSetUIType == i)
            {
                // その番号の武器を装備する
                // ※ 装備武器の列挙数とUIの種類の列挙数は一致していることが条件
                Player.GetSetEquipWeaponState = (EquipWeaponState)i;

                // for文から抜ける
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

        // 選択したUIのフェードイン開始
        m_SelectUI.UIFadeIN();

        // 選択したUIを空にする
        m_SelectUI = null;
    }
}
