/**
 * @file   GameManager.cs
 * @brief  GameManagerクラス
 * @author IharaShota
 * @date   2023/03/27
 * @Update 2023/03/27 作成
 * @Update 2023/07/12 シーンようのステート追加
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

// ===============================================
// GameState
// … Gameの状態を管理する列挙体
// ===============================================
public enum GameState
{
    GameStart,
    GamePlay,
    GamePause,
    GameRule,
    GameGoal,
    GameOver,

    MaxGameState
}

// ===============================================
// SceneState
// … Sceneの状態を管理する列挙体
// ===============================================
public enum SceneState
{
    GameScene,
    GameScene2,
    GameScene3,


    MaxGameState
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameState m_GameState;
    [SerializeField] private SceneState m_SceneState;

    private void Awake()
    {
        //シーンごとのステート管理
        switch (SceneManager.GetActiveScene().name)
        {
            case "GameScene":
                m_SceneState = SceneState.GameScene;
                break;
            case "GameScene 2":
                m_SceneState = SceneState.GameScene2;
                break;
            case "GameScene 3":
                m_SceneState = SceneState.GameScene3;
                break;
            default:
                break;
        }
        if (instance == null)
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

        if (IS_AudioManager.instance.GetSE(SEType.SE_GameClear).isPlaying)
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

    /**
     * @fn
     * GSceneの状態のgetter・setter
     * @return m_GameState
     * @brief Sceneの状態を返す・セット
     */
    public SceneState GetSetSceneState
    {
        get { return m_SceneState; }
        set { m_SceneState = value; }
    }
}
