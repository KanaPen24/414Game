﻿/**
 * @file   YK_GameOver.cs
 * @brief  ゲームオーバークラス
 * @author YoshidaKanase
 * @date   2023/04/26
 * @Update 2023/05/05 ゲームオーバーのBGM実装
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YK_GameOver : MonoBehaviour
{
    [SerializeField] private GameObject GameOver;
    private bool m_bGameOverFlag;

    // Start is called before the first frame update
    void Start()
    {
        GameOver.SetActive(false);
        m_bGameOverFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GetSetGameState == GameState.GameOver)
        {
            if (!m_bGameOverFlag)
            {
                IS_AudioManager.instance.StopBGM(BGMType.BGM_Game);
                IS_AudioManager.instance.PlayBGM(BGMType.BGM_GAMEOVER);
            }
            m_bGameOverFlag = true;
            Debug.Log("Gameover");
            //Time.timeScale = 0.0f;
            GameOver.SetActive(true);
        }
    }
}
