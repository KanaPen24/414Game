using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_SlimeShadow : MonoBehaviour
{
    [SerializeField] private slime m_Slime;

    private void Update()
    {
        if(m_Slime.GetSetSlimeState == SlimeState.SlimeMove)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }
}
