using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_Bat_SonicBallet : MonoBehaviour
{
    [SerializeField] private float m_SonicSpeed;

    private void FixedUpdate()
    {
        this.gameObject.transform.Translate(m_SonicSpeed * Time.deltaTime, 0, 0);
    }
}
