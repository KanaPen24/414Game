/**
 * @file PlayerHP.cs
 * @brief �v���C���[�̗̑�
 * @author �g�c����
 * @date 2023/03/06
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YK_PlayerHP : MonoBehaviour
{
    // �̗͒l���i�[����ϐ��i��U 100�j
    public static int nMaxHP = 100;
    // ���݂̗̑͒l���i�[����ϐ��i�����l�� maxHealth�j
    public int nCurrentHP = nMaxHP;
    //Slider������
    [SerializeField] Slider HPBar;
    // Start is called before the first frame update
    void Start()
    {
        //Slider�𖞃^���ɂ���B
        HPBar.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AddLife(1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            DelLife(1);
        }
        //�ő�HP�ɂ����錻�݂�HP��Slider�ɔ��f�B
        //int���m�̊���Z�͏����_�ȉ���0�ɂȂ�̂ŁA
        //(float)������float�̕ϐ��Ƃ��ĐU���킹��B
        HPBar.value = (float)nCurrentHP / (float)nMaxHP;
    }
    // �_���[�W����
    public void DelLife(int damage)
    {
        // ���݂̗̑͒l���� ���� damage �̒l������
        nCurrentHP -= damage;
        // ���݂̗̑͒l�� 0 �ȉ��̏ꍇ
        if (nCurrentHP <= 0)
        {
            // ���݂̗̑͒l�� 0 ����
            nCurrentHP = 0;
            // �R���\�[����"Dead!"��\������
            Debug.Log("Dead!");
        }
    }
    // �񕜏���
    public void AddLife(int heal)
    {
        // ���݂̗̑͒l���� ���� heal �̒l�𑫂�
        nCurrentHP += heal;
        // ���݂̗̑͒l�� maxHealth �ȏ�̏ꍇ
        if (nCurrentHP >= nMaxHP)
        {
            // ���݂̗̑͒l�� �ő�l ����
            nCurrentHP = nMaxHP;
            // �R���\�[����"Max!"��\������
            Debug.Log("Max!");
        }
    }
}
