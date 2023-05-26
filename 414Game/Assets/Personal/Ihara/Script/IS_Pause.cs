/**
 * @file   IS_Pause.cs
 * @brief  ポーズクラス
 * @author IharaShota
 * @date   2023/03/28
 * @Update 2023/03/28 作成
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_Pause : MonoBehaviour
{
    [SerializeField] private GameObject Pause;
    private bool m_bPause;

    private void Start()
    {
        Pause.SetActive(false);
        m_bPause = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)|| Input.GetKeyDown(KeyCode.Joystick1Button7) && !m_bPause)
        {
            GameManager.instance.GetSetGameState = GameState.GamePause;
            m_bPause = true;
        }
        else if (Input.GetKeyDown(KeyCode.P)|| Input.GetKeyDown(KeyCode.Joystick1Button7) && m_bPause)
        {
            GameManager.instance.GetSetGameState = GameState.GamePlay;
            m_bPause = false;
        }

        if (GameManager.instance.GetSetGameState == GameState.GamePause)
        {
            Time.timeScale = 0.0f;
            Pause.SetActive(true);
        }
        else
        {
            Time.timeScale = 1.0f;
            Pause.SetActive(false);
        }
    }
}
