using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class testplay : MonoBehaviour
{
    [SerializeField] VisualEffect vfx;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            vfx.SendEvent("OnPlay");
        }

    }
}
