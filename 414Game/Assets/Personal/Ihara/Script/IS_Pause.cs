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
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.instance.GetSetGameState = GameState.GamePause;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.instance.GetSetGameState = GameState.GamePlay;
        }

        if (GameManager.instance.GetSetGameState == GameState.GamePause)
        {
            Time.timeScale = 0.0f;
        }
        else Time.timeScale = 1.0f;
    }
}
