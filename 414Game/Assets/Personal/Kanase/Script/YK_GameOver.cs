/**
 * @file   YK_GameOver.cs
 * @brief  ゲームオーバークラス
 * @author YoshidaKanase
 * @date   2023/04/26
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YK_GameOver : MonoBehaviour
{
    [SerializeField] private GameObject GameOver;

    // Start is called before the first frame update
    void Start()
    {
        GameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GetSetGameState == GameState.GameOver)
        {
            Debug.Log("Gameover");
            //Time.timeScale = 0.0f;
            //GameOver.SetActive(true);
        }
    }
}
