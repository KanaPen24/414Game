/**
 * @file HPBarVisible.cs
 * @brief HPBar����������\�������肷��
 * @author �g�c����
 * @date 2023/03/06
 * @date 2023/03/12 HPBar��\���Ǘ�����bool�^�쐬(Ihara)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; // �^�L���X�g����ۂɕK�v

public class YK_HPBarVisible : MonoBehaviour
{
    [SerializeField] Slider HP;
    [SerializeField] private IS_WeaponHPBar m_WeaponHpBar;
    private bool m_bVisible;
    private int m_nCnt;

    // Start is called before the first frame update
    void Start()
    {
        // �����o�̏�����
        m_bVisible = true;
        m_nCnt = Convert.ToInt32(m_bVisible);

        // �\����Ԃ�������
        if (m_bVisible)
        {
            HPEnableTrue();
        }
        // ��\����Ԃ�������
        else
        {
            HPEnableFalse();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �O��Ə�Ԃ��������
        if(m_nCnt != Convert.ToInt32(m_bVisible))
        {
            // �\����Ԃ�������
            if(m_bVisible)
            {
                HPEnableTrue();
            }
            // ��\����Ԃ�������
            else
            {
                HPEnableFalse();
            }
        }

        // ���݂̏�ԂɍX�V
        m_nCnt = Convert.ToInt32(m_bVisible);
    }

    //HPBar������
    public void HPEnableFalse()
    {
        HP.gameObject.SetActive(false);
        m_WeaponHpBar.gameObject.SetActive(true);
    }

    //HPBar��\��
    public void HPEnableTrue()
    {
        HP.gameObject.SetActive(true);
        m_WeaponHpBar.gameObject.SetActive(false);
    }

    /**
     * @fn
     * PlayerHP�\����getter�Esetter
     * @return m_bVisible(bool)
     * @brief PlayerHP�\����Ԃ��E�Z�b�g
     */
    public bool GetSetVisible
    {
        get { return m_bVisible; }
        set { m_bVisible = value; }
    }
}
