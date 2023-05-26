/**
 * @file FpsDisplay.cs
 * @brief FPS値の固定及び表示
 * @author 吉田叶聖
 * @date 2023/03/10
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsDisplay : MonoBehaviour
{

    // 変数
    int frameCount;
    float prevTime;
    float fps;
    public int nFont;

    // 初期化処理
    void Start()
    {
        frameCount = 0;
        prevTime = 0.0f;
    }
    // 更新処理
    void Update()
    {
        Application.targetFrameRate = 60;
        frameCount++;
        float time = Time.realtimeSinceStartup - prevTime;

        if (time >= 0.5f)
        {
            fps = frameCount / time;

            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
        }
    }
    private void OnGUI()
    {
        //GUI.color = Color.black;
        //GUI.skin.label.fontSize = nFont;
        //GUILayout.Label(fps.ToString());
    }
}