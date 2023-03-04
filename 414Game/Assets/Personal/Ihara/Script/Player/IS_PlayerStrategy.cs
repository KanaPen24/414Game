/**
 * @file   IS_PlayerStrategy.cs
 * @brief  Player�̋������N���X
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 �쐬
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerStrategy : MonoBehaviour
{
    /**
     * @fn
     * �X�V����(override�O��)
     * @brief Player�̍X�V����
     */
    public virtual void UpdateStrategy()
    {
        // ������State���Ƃɏ�����������
        Debug.Log("�F����");
    }
}
