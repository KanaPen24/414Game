using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour
{
    [SerializeField] private ON_BlurController controller;
    bool flg = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(flg);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            flg = flg ? false : true;
        }
        controller.SetBlur(flg);
    }
}
