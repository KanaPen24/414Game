/**
 * @file   IS_PlayerGroundCollision.cs
 * @brief  Player�̒n�ʔ���N���X
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 �쐬
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerGroundCollision : MonoBehaviour
{
    [SerializeField] private GameObject[] m_RayPoints; // Ray���΂��n�_(4��)
    [SerializeField] private float m_fRayLength;       // Ray�̒���

    /**
     * @fn
     * Player�̒n�ʔ���֐�
     * @brief�@�n�ʂ�ray���������Ă�����true��Ԃ�,
     *         �������Ă��Ȃ����false��Ԃ�
     */
    public bool IsGroundCollision()
    {
        // =========================================== 
        // �ϐ��錾 
        // ===========================================

        // Ray�̏�����
        Ray[] ray = new Ray[m_RayPoints.Length];
        //Ray�����������I�u�W�F�N�g�̏������锠
        RaycastHit hit;

        // ===========================================

        // Ray�̐�������������
        for (int i = 0; i < m_RayPoints.Length; ++i)
        {
            //Ray�̍쐬�@�@�@�@�@�@�@��Ray���΂����_�@�@�@��Ray���΂�����
            ray[i] = new Ray(m_RayPoints[i].transform.position, Vector3.down);
            Debug.DrawRay(m_RayPoints[i].transform.position, Vector3.down, Color.red, m_fRayLength);
        }

        // �n�ʂƂ̓����蔻��(���ł��ʂ��true)
        for (int i = 0; i < m_RayPoints.Length; i++)
        {
            if (Physics.Raycast(ray[i], out hit, m_fRayLength))
            {
                return true;
            }
        }
        return false;
    }
}
