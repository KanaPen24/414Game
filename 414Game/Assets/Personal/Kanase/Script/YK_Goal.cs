/**
 * @file   YK_Goal.cs
 * @brief  ゴール用のブロックの処理
 * @author 吉田叶聖
 * @date   2023/03/27
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用


public class YK_Goal : MonoBehaviour
{
    [SerializeField] Fade fade;

    //プレイヤーに当たったらタイトルシーンに移動
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //トランジションを掛けてシーン遷移する
            fade.FadeIn(1f, () =>
            {
                IS_AudioManager.instance.StopBGM(BGMType.BGM_Game);
                SceneManager.LoadScene("GameScene");
            });
        }

    }
}
