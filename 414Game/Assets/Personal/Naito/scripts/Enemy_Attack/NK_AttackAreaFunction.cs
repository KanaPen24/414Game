// ==============================================================
// NK_AttackAreaFunction.cs
// Auther:Naito
// Update:2023/03/10 cs�쐬
// ==============================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_AttackAreaFunction : MonoBehaviour
{
    private void Start()
    {
        //�U���͈͂������鏈��
        Invoke("AttackAreaDereta", 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //�����Ƀv���C���[���_���[�W��H�炤�����������Ă�
        }
    }

    private void AttackAreaDereta()
    {
        Destroy(this.gameObject);
    }

}
