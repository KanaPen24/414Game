using UnityEngine;
using System;
using DG.Tweening;

public class YK_TargetCamera : MonoBehaviour
{
    [SerializeField]
    [Tooltip("追従させたいターゲット")]
    private IS_Player Player;

    [SerializeField] private Vector3 offset;
    [SerializeField] private float m_fMaxCameraMove;
    [SerializeField] private float m_fMoveCnt;
    [SerializeField] private float m_fMoveCamera=2.0f;   //カメラの動くスピード
    private float m_fCameraMove;
    private float m_fCurrnetMoveCnt;
    private float m_fAccel;
    private const float m_fMaxAccel = 3.0f;
    private int nDir;
    [SerializeField] private NK_BossSlime_Aera m_Area;
    [SerializeField] private GameObject m_BattleCameraPos;
    private float Shakefloat = 0.0f;
    private Vector3 RendaPos;
    private bool m_OneFlg = false;
    private bool m_OneFlg2 = false;
    [SerializeField] private float m_CameraSpeed;
    private Vector3 TargetCameraPos;    //標的を置いた時のカメラの座標    
    // Start is called before the first frame update
    void Start()
    {
        m_fCameraMove = 0;
        m_fCurrnetMoveCnt = 0;
        m_fAccel = m_fMaxAccel;

    }

    /// <summary>
    /// プレイヤーが移動した後にカメラが移動するようにするためにLateUpdateにする。
    /// </summary>
    void FixedUpdate()
    {
        // ゲームがプレイ中以外は更新しない
        if (GameManager.instance.GetSetGameState != GameState.GamePlay)
            return;

        TargetCameraPos= new Vector3(Player.transform.position.x, 0f, Player.transform.position.z) + offset +
                new Vector3(m_fCameraMove, 0f, 0f);

        if (m_OneFlg)
        {
            RendaPos = this.gameObject.transform.position;

            //連打の振動
            Shakefloat = Shakefloat * 0.99f - Shakefloat * 0.01f;

            RendaPos.x = m_BattleCameraPos.transform.position.x + UnityEngine.Random.Range(-Shakefloat, Shakefloat);
            RendaPos.y = m_BattleCameraPos.transform.position.y + UnityEngine.Random.Range(-Shakefloat, Shakefloat);

            this.gameObject.transform.position = RendaPos;
        }
        if (m_Area.GetSetBattleFlag)
        {
            if (!m_OneFlg)
            {
                this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, m_BattleCameraPos.transform.position, m_fMoveCamera * Time.deltaTime);
                //ターゲットに到達したら
                if (transform.position == m_BattleCameraPos.transform.position)
                {
                    m_OneFlg = true;
                }
            }
        }
        else
        {
            if (!m_OneFlg2)
            {
                this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, TargetCameraPos, m_fMoveCamera * Time.deltaTime*4);
                //ターゲットに到達したら
                if (transform.position == TargetCameraPos)
                {
                    m_OneFlg2 = true;
                }
            }
            else
            {
                // カメラの位置をターゲットの位置にオフセットを足した場所にする。
                gameObject.transform.position = TargetCameraPos;
            }
        }
        //リセットフラグが立っているとき
        if (YK_JsonSave.instance && YK_JsonSave.instance.GetSetResetFlg)
        {
            Vector3 pos = transform.position;
            pos.x = TargetCameraPos.x;
            transform.position = pos;
        }
    }

    public void GetShakeFloat(float Shake)
    {
        Shakefloat = Shake;
    }

    public Vector3 GetSetCameraPos
    {
        get { return this.gameObject.transform.position; }
        set { this.gameObject.transform.position = value; }
    }
}