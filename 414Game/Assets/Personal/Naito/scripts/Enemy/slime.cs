using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Rendering;

public enum slimeState
{
    SlimeWait,
    SlimeMove,

    MoxSlimeState
}

public class slime : NK_Enemy
{
    [SerializeField] private List<NK_SlimeStrategy> m_SlimeStrategy;
    [SerializeField] private SlimeState m_SlimeState;
    [SerializeField] private ParticleSystem m_DieEffect;
    [SerializeField] private YK_Clock m_Clock;
    [SerializeField] private CubismRenderController renderController;
    private float m_fViewX;
    //影をアタッチする
    [SerializeField] private GameObject Shadow;
    private Animator anim;
    private Rigidbody m_rBody;
    private bool m_MoveAnimFlag;
    //横移動
    [SerializeField] private float m_fMovePower;
    //ジャンプ力
    [SerializeField] private float m_fJumpPower;
    private float m_localScalex;
    [SerializeField] private float m_MoveReng;
    [SerializeField] private int m_PlayerDamage;
    private bool m_ClockFlag;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        m_rBody = GetComponent<Rigidbody>();
        m_localScalex = this.transform.localScale.x;
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
        else
        {
            renderController.Opacity = 1f;
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
        if (m_SlimeState == SlimeState.SlimeMove)
        {
            Shadow.SetActive(false);
        }
        else
        {
            Shadow.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (m_Clock.GetSetStopTime)
        {
            m_ClockFlag = true;
            return;
        }
        else
        {
            m_ClockFlag = false;
        }
        if (m_fViewX >= m_MoveReng)
        {
            return;
        }
        if (GameManager.instance.GetSetGameState != GameState.GamePlay)
        {
            return;
        }

        m_SlimeStrategy[(int)m_SlimeState].UpdateStrategy();
        anim.SetBool("JumpFlag", m_MoveAnimFlag);
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーだったら
        if (other.gameObject == IS_Player.instance.gameObject)
        {
            Debug.Log("Player Damage!!");
            IS_Player.instance.Damage(m_PlayerDamage, 1.5f);
        }
    }

    public SlimeState GetSetSlimeState
    {
        get { return m_SlimeState; }
        set { m_SlimeState = value; }
    }

    public bool GetSetMoveAnimFlag
    {
        get { return m_MoveAnimFlag; }
        set { m_MoveAnimFlag = value; }
    }

    private void InvisbleEnd()
    {
        m_DamageFlag = false;
    }
    public void SlimeDamage(int Damage)
    {
        if (!m_DamageFlag)
        {
            m_nHP -= Damage;
            m_DamageFlag = true;
            m_rBody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            if (GetSetEnemyDir == EnemyDir.Left)
            {
                m_rBody.AddForce(transform.up * m_fJumpPower, ForceMode.Impulse);
                m_rBody.AddForce(transform.right * m_fMovePower, ForceMode.Impulse);
            }
            else
            {
                m_rBody.AddForce(transform.up * m_fJumpPower, ForceMode.Impulse);
                m_rBody.AddForce(transform.right * -m_fMovePower, ForceMode.Impulse);
            }
            Invoke("InvisbleEnd", m_InvincibleTime);
            // HPが0になったら、このオブジェクトを破壊
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
}
