using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HPBar : MonoBehaviour
{
    [SerializeField] Slider HP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            HPEnableFalse();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            HPEnableTrue();
        }
    }
    private void HPEnableFalse()
    {
        HP.gameObject.SetActive(false);
    }
    private void HPEnableTrue()
    {
        HP.gameObject.SetActive(true);
    }
}
