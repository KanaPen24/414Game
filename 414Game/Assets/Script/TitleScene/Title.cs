using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField] Fade fade;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Decision"))
        {
            //トランジションを掛けてシーン遷移する
            fade.FadeIn(1f, () =>
            {
                SceneManager.LoadScene("GameScene");
            });
        }
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();             //ゲーム終了処理
    }
}
