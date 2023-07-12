using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Rendering;

// ===============================================
// BossBatState
// … BossBatの状態を管理する列挙体
// ※m_PlayerStateはこの順番になるように入れること
// ===============================================
public enum batState
{
    BatWait,     //待機状態
    BatMove,     //移動状態
    BatSonic,    //超音波攻撃状態
    BatFlight,   //
    BatFall,     //急降下攻撃
    BatUp,       //上昇状態

    MaxBatState
}

public class bat : NK_Enemy
{
    [SerializeField] private List<NK_BatStrategy> m_BatStrategy; // BossBat挙動クラスの動的配列
    [SerializeField] private batState m_BatState;      // BossBatの状態を管理する
    //時を止めるUIをアタッチ
    [SerializeField] private YK_Clock m_Clock;
    private Rigidbody m_Rbody;
    [HideInInspector] public Vector3 m_MoveValue;
    private bool m_DamageFlag;
    private CubismRenderController renderController;
    private float m_localScalex;
    private bool m_FallAnimFlag;
    private bool m_FlightAnimFlag;
    private Animator m_Anim;
    [SerializeField] private float m_MoveReng;
    private float m_fViewX;
    //死亡時エフェクト
    [SerializeField] private ParticleSystem m_DieEffect;
    [SerializeField] private int m_PlayerDamage;
    [SerializeField] private ParticleSystem m_FallEffact;
    private bool m_ClockFlag;
    [SerializeField] private NK_BossSlime_Aera m_Area;


    // Start is called before the first frame update
    void Start()
    {
        m_MoveValue = new Vector3(0.0f, 0.0f, 0.0f);
        m_Rbody = GetComponent<Rigidbody>();
        m_DamageFlag = false;
        m_localScalex = this.transform.localScale.x;
        m_Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_fViewX = Camera.main.WorldToViewportPoint(this.transform.position).x;
        if (m_DamageFlag)
        {
            //Mathf.Absは絶対値を返す、Mathf.Sinは＋なら１，－なら0を返す
            float level = Mathf.Abs(Mathf.Sin(Time.time * 10));
            renderController.Opacity = level;
        }
        if (!m_ClockFlag)
        {
            if (IS_Player.instance.transform.position.x > this.gameObject.transform.position.x)
            {
                GetSetEnemyDir = EnemyDir.Right;
                this.transform.localScale =
                    new Vector3(-m_localScalex, this.transform.localScale.y, this.transform.localScale.z);
            }
            else
            {
                GetSetEnemyDir = EnemyDir.Left;
                this.transform.localScale =
                    new Vector3(m_localScalex, this.transform.localScale.y, this.transform.localScale.z);
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_Clock.GetSetStopTime)
        {
            m_Rbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            m_FallEffact.Stop();
            m_Anim.SetFloat("Moving", 0.0f);
            m_ClockFlag = true;
            return;
        }
        else
        {
            m_FallEffact.Play();
            m_Anim.SetFloat("Moving", 1.0f);
            m_ClockFlag = false;
        }
        if (m_fViewX >= m_MoveReng)
        {
            return;
        }
        if (GameManager.instance.GetSetGameState != GameState.GamePlay)
        {
            if (m_BatState == batState.BatMove)
            {
                m_Rbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
                return;
            }
        }
        if (m_Area.GetSetBattleFlag)
        {
            m_Rbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            return;
        }

        m_BatStrategy[(int)m_BatState].UpdateStrategy();

        m_Rbody.velocity = m_MoveValue;

        m_Anim.SetBool("FallFlag", m_FallAnimFlag);
        m_Anim.SetBool("FlightFlag", m_FlightAnimFlag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == IS_Player.instance.gameObject)
        {
            Debug.Log("Player Damage!!");
            IS_Player.instance.Damage(m_PlayerDamage, 2.0f);
        }
    }

    public batState GetSetBatState
    {
        get { return m_BatState; }
        set { m_BatState = value; }
    }
    public Vector3 GetSetMoveValue
    {
        get { return m_MoveValue; }
        set { m_MoveValue = value; }
    }

    private void InvisbleEnd()
    {
        m_DamageFlag = false;
    }

    public bool GetSetFallFlag
    {
        get { return m_FallAnimFlag; }
        set { m_FallAnimFlag = value; }
    }

    public bool GetSetFlightFlag
    {
        get { return m_FlightAnimFlag; }
        set { m_FlightAnimFlag = value; }
    }

    public void BatDamage(int Damage)
    {
        if (!m_DamageFlag)
        {
            m_nHP -= Damage;
            m_DamageFlag = true;
            Invoke("InvincibleEnd", m_InvincibleTime);
            if (m_nHP <= 0)
            {
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
}
