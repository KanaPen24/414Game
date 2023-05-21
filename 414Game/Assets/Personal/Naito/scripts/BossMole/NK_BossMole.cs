using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Rendering;

public enum BossMoleState
{
    MoleWait,
    MoleFire,
    MoleRush,
    MolePounce,
    MoleGround,

    MaxMolState
}

public enum BossMoleDir
{
    Left,
    Right,

    MaxDir
}

public class NK_BossMole : MonoBehaviour
{
    //敵の体力
    [SerializeField] private int m_nHP;
    [SerializeField] private int m_nMaxHP;//敵の最大体力
    public IS_Player m_BMPlayer;//プレイヤー
    [SerializeField] private IS_GoalEffect goalEffect;//倒されたときに発生するエフェクト
    [SerializeField] private List<NK_BossMoleStrategy> m_BossMoleStrategy;
    [SerializeField] private BossMoleState m_BossMoleState;
    [SerializeField] private BossMoleDir m_BossMoleDir;
    //時を止めるUIをアタッチ
    [SerializeField] private YK_Clock m_Clock;
    private bool m_DamageFlag;
    [SerializeField] private float m_InvincibleTime;
    private float m_fViewX;
    [SerializeField] private YK_Goal goal;
    private Rigidbody m_Rbody;
    [HideInInspector] public Vector3 m_BSMoveValue;
    private float m_localScalex;
    [SerializeField] private CubismRenderController renderController;
    [HideInInspector] public Vector3 m_BMMoveValue;
    public GameObject m_RushPosR;
    public GameObject m_RushPosL;
    public GameObject m_MolePosR;
    public GameObject m_MolePosL;

    // Start is called before the first frame update
    void Start()
    {
        m_BMMoveValue = new Vector3(0.0f, 0.0f, 0.0f);
        m_Rbody = GetComponent<Rigidbody>();
        m_localScalex = this.transform.localScale.x;
    }

    private void Update()
    {
        if (m_DamageFlag)
        {
            //Mathf.Absは絶対値を返す、Mathf.Sinは＋なら１，－なら0を返す
            float level = Mathf.Abs(Mathf.Sin(Time.time * 10));
            renderController.Opacity = level;
        }
        else renderController.Opacity = 1f;
    }

    private void FixedUpdate()
    {
        if(m_Clock.GetSetStopTime)
        {
            return;
        }
        m_BossMoleStrategy[(int)m_BossMoleState].UpdateStrategy();

        m_Rbody.velocity = m_BMMoveValue;
    }

    public BossMoleState GetSetBossMoleState
    {
        get { return m_BossMoleState; }
        set { m_BossMoleState = value; }
    }

    public BossMoleDir GetSetBossMoleDir
    {
        get { return m_BossMoleDir; }
        set { m_BossMoleDir = value; }
    }

    private void InvicibleEnd()
    {
        m_DamageFlag = false;
    }

    public void BossMoleDamage(int Damage)
    {
        if(!m_DamageFlag)
        {
            m_nHP -= Damage;
            m_DamageFlag = true;
            Invoke("InvicibleEnd", m_InvincibleTime);
            if(m_nHP<=0)
            {
                IS_AudioManager.instance.PlaySE(SEType.SE_GameClear);
                goalEffect.StartEffect();
                goal.gameObject.SetActive(true);
                Destroy(this.gameObject);
            }
        }
    }
}
