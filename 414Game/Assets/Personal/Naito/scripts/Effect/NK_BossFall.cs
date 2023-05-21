using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossFall : MonoBehaviour
{
    [SerializeField] private float m_FallPow;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.up * m_FallPow);
    }
}
