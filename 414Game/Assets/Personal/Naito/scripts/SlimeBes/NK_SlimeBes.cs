using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Rendering;

public enum SlimeBesState
{
    BesWait,
    BesCrawl,
    BesAcid,

    MaxBesState
}

public enum BesDir
{
    Left,
    Right,

    MaxDir
}

public class NK_SlimeBes : MonoBehaviour
{
    //敵の体力
    [SerializeField] private int m_nHP;
    [SerializeField] private int m_nMaxHP;//敵の最大体力
    public IS_Player m_BesPlayer;//プレイヤー
    [SerializeField] private List<NK_SlimeBesStrategy> m_BesStrategy; // BossBat挙動クラスの動的配列
    [SerializeField] private SlimeBesState m_BesState;      // BossBatの状態を管理する
    [SerializeField] private BesDir m_BesDir;        // BossBatの向きを管理する
    //時を止めるUIをアタッチ
    [SerializeField] private YK_Clock m_Clock;
    private Rigidbody m_Rbody;
    [HideInInspector] public Vector3 m_MoveValue;
    private float m_localScalex;
    private float m_fViewX;
    [SerializeField] private float m_MoveReng;
    [SerializeField] private int m_PlayerDamage;
    //横移動
    [SerializeField] private float m_fMovePower;
    //ジャンプ力
    [SerializeField] private float m_fJumpPower;
    //死亡時エフェクト
    [SerializeField] private ParticleSystem m_DieEffect;
    private Animator m_Anim;
    private bool m_MoveAnimFlag;
    private bool m_AcidAnimFlag;

    // Start is called before the first frame update_
    void Start()
    {
        m_MoveValue = new Vector3(0.0f, 0.0f, 0.0f);
        m_Rbody = GetComponent<Rigidbody>();
        m_localScalex = this.transform.localScale.x;
        m_Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_fViewX = Camera.main.WorldToViewportPoint(this.transform.position).x;
        if (!m_Clock.GetSetStopTime)
        {
            if (m_BesPlayer.transform.position.x > this.gameObject.transform.position.x) 
            {
                GetSetBesDir = BesDir.Right;
                this.transform.localScale =
                 new Vector3(-m_localScalex, this.transform.localScale.y, this.transform.localScale.z);
            }
            else
            {
                GetSetBesDir = BesDir.Left;
                this.transform.localScale =
                 new Vector3(m_localScalex, this.transform.localScale.y, this.transform.localScale.z);
            }
        }
    }

    private void FixedUpdate()
    {
        if(m_Clock.GetSetStopTime)
        {
            return;
        }
        if (m_fViewX >= m_MoveReng)
        {
            return;
        }
        if (GameManager.instance.GetSetGameState != GameState.GamePlay)
        {
            return;
        }

        m_BesStrategy[(int)m_BesState].UpdateStrategy();

        m_Rbody.velocity = m_MoveValue;

        m_Anim.SetBool("AcidFlag", m_AcidAnimFlag);
        m_Anim.SetBool("IdouFlag", m_MoveAnimFlag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject==m_BesPlayer.gameObject)
        {
            Debug.Log("Player Damage!!");
            m_BesPlayer.Damage(m_PlayerDamage, 1.5f);
        }
    }

    public SlimeBesState GetSetBesState
    {
        get{ return m_BesState; }
        set{ m_BesState = value; }
    }

    public BesDir GetSetBesDir
    {
        get { return m_BesDir; }
        set { m_BesDir = value; }
    }

    public Vector3 GetSetMoveValue
    {
        get { return m_MoveValue; }
        set { m_MoveValue = value; }
    }

    public bool GetSetMoveAnimFlag
    {
        get { return m_MoveAnimFlag; }
        set { m_MoveAnimFlag = value; }
    }

    public bool GetSetAcidAnimFlag
    {
        get { return m_AcidAnimFlag; }
        set { m_AcidAnimFlag = value; }
    }

    public void BesDamage(int Damage)
    {
        m_nHP -= Damage;
        m_Rbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        if (GetSetBesDir == BesDir.Left)
        {
            m_Rbody.AddForce(transform.up * m_fJumpPower, ForceMode.Impulse);
            m_Rbody.AddForce(transform.right * m_fMovePower, ForceMode.Impulse);
        }
        else
        {
            m_Rbody.AddForce(transform.up * m_fJumpPower, ForceMode.Impulse);
            m_Rbody.AddForce(transform.right * -m_fMovePower, ForceMode.Impulse);
        }
        if (m_nHP <= 0)
        {
            // SE再生
            IS_AudioManager.instance.PlaySE(SEType.SE_DeathSlime);
            // エフェクト再生
            ParticleSystem Effect = Instantiate(m_DieEffect);
            Effect.Play();
            Effect.transform.position = this.transform.position;
            Destroy(Effect.gameObject, 2.0f);

            Destroy(this.gameObject);
        }
    }
}
