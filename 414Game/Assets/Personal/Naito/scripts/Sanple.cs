using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sanple : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_a;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            m_a.Stop();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            m_a.Play();
        }
    }
}
