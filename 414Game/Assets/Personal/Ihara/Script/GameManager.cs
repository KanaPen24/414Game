/**
 * @file   GameManager.cs
 * @brief  GameManagerクラス
 * @author IharaShota
 * @date   2023/03/27
 * @Update 2023/03/27 作成
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================================
// GameState
// … Gameの状態を管理する列挙体
// ===============================================
public enum GameState
{
    GameStart,
    GamePlay,
    GamePause,
    GameGoal,
    GameOver,

    MaxGameState
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;  
    [SerializeField] private GameState m_GameState;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        IS_AudioManager.instance.PlayBGM(BGMType.BGM_Game);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("GameQuit!!");
            Application.Quit();             //ゲーム終了処理
        }

        if(IS_AudioManager.instance.GetSE(SEType.SE_GameClear).isPlaying)
        {
            IS_AudioManager.instance.GetBGM(BGMType.BGM_Game).mute = true;
        }
        else IS_AudioManager.instance.GetBGM(BGMType.BGM_Game).mute = false;
    }

    /**
     * @fn
     * Gameの状態のgetter・setter
     * @return m_GameState
     * @brief Gameの状態を返す・セット
     */
    public GameState GetSetGameState
    {
        get { return m_GameState; }
        set { m_GameState = value; }
    }
}
