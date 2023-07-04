using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyName
{
    Bat,
    BossSlime,
    Slime,
    SlimeBes,

    MaxEnemyName
}

public enum EnemyType
{
    Boss,
    Operator,

    MaxEnemyType
}

public enum EnemyDir
{
    Left,
    Right,

    MaxDir
}

public class NK_Enemy : MonoBehaviour
{
    protected EnemyType m_EnemyType;
    protected EnemyName m_EnemyName;
    [SerializeField] protected int m_nHP;
    [SerializeField] protected int m_nMaxHP;
    protected bool m_DamageFlag;
    [SerializeField] protected float m_InvincibleTime;
    protected EnemyDir m_EnemyDir;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetSetHp
    {
        get { return m_nHP; }
        set { m_nHP = value; }
    }

    public int GetSetMaxHp
    {
        get { return m_nMaxHP; }
        set { m_nMaxHP = value; }
    }

    protected void InvicibleEnd()
    {
        m_DamageFlag = false;
    }

    public bool GetSetDamageFlag
    {
        get { return m_DamageFlag; }
        set { m_DamageFlag = value; }
    }

    public EnemyDir GetSetEnemyDir
    {
        get { return m_EnemyDir; }
        set { m_EnemyDir = value; }
    }
}
