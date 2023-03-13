/**
 * @file HPBarHP.cs
 * @brief �̗̓o�[�̗̑�
 * @author �g�c����
 * @date 2023/03/06
 * @date 2023/03/13 �̗͂ɂ��F��ς���
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YK_HPBerHP : MonoBehaviour
{
    // �̗͒l���i�[����ϐ��i��U 100�j
    public static int nMaxHP = 100;
    // ���݂̗̑͒l���i�[����ϐ��i�����l�� maxHealth�j
    public int nCurrentHP = nMaxHP;
    //����̃Q�[���I�u�W�F�N�g
    [SerializeField] private GameObject HpBarWeapon;
    // Start is called before the first frame update
    void Start()
    {
        //�����J���[
        HpBarWeapon.GetComponent<Renderer>().material.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        //50�ȉ�
        if (nCurrentHP <= 50)
            HpBarWeapon.GetComponent<Renderer>().material.color = Color.yellow;
        //20�ȉ�
        if (nCurrentHP <= 20)
            HpBarWeapon.GetComponent<Renderer>().material.color = Color.red;
        //0
        if (nCurrentHP <= 0)
            Destroy(HpBarWeapon);
            /*�Q�[���I�[�o�[�����Ăяo��*/
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
            // �R���\�[����"HPBarHPMax!"��\������
            Debug.Log("HPBarHPMax!");
        }
    }
}
