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

    // Start is called before the first frame update_
    void Start()
    {
        m_MoveValue = new Vector3(0.0f, 0.0f, 0.0f);
        m_Rbody = GetComponent<Rigidbody>();
        m_localScalex = this.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(GetSetBesState==SlimeBesState.BesCrawl)
        {
            if (m_BesPlayer.transform.position.x > this.gameObject.transform.position.x) 
            {
                GetSetBesDir = BesDir.Right;
                this.transform.localScale =
                 new Vector3(m_localScalex, this.transform.localScale.y, this.transform.localScale.z);
            }
            else
            {
                GetSetBesDir = BesDir.Left;
                this.transform.localScale =
                 new Vector3(-m_localScalex, this.transform.localScale.y, this.transform.localScale.z);
            }
        }
    }

    private void FixedUpdate()
    {
        if(m_Clock.GetSetStopTime)
        {
            return;
        }
        m_BesStrategy[(int)m_BesState].UpdateStrategy();

        m_Rbody.velocity = m_MoveValue;
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
}
