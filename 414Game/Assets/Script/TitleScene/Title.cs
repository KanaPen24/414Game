using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemnt�̋@�\���g�p
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
        if (Input.GetKey(KeyCode.Return))
        {
            //�g�����W�V�������|���ăV�[���J�ڂ���
            fade.FadeIn(1f, () =>
            {
                SceneManager.LoadScene("GameScene");
            });
        }
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();             //�Q�[���I������
    }
}
