using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] Fade fade;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Backspace) || Input.GetKeyDown("joystick button 1"))
        {
            //トランジションを掛けてシーン遷移する
            fade.FadeIn(1f, () =>
            {
                SceneManager.LoadScene("TitleScene");
            });
        }
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();             //ゲーム終了処理
    }
}
