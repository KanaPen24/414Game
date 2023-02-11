using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemnt�̋@�\���g�p
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
            //�g�����W�V�������|���ăV�[���J�ڂ���
            fade.FadeIn(1f, () =>
            {
                SceneManager.LoadScene("TitleScene");
            });
        }
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();             //�Q�[���I������
    }
}
