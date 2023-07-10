using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ON_TestInput : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(0.0f, 1.0f)] [SerializeField] private float rate;
    [SerializeField] private Material[] materials;
    
    void Update()
    {
        for (int i = 0; i < materials.Length; ++i)
        {
            materials[i].SetFloat("_Rate", rate);
        }
    }
}
