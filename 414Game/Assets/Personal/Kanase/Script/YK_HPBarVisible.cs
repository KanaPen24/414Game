/**
 * @file HPBarVisible.cs
 * @brief HPBar����������\�������肷��
 * @author �g�c����
 * @date 2023/03/06
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YK_HPBarVisible : MonoBehaviour
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

    //HPBar������
    public void HPEnableFalse()
    {
        HP.gameObject.SetActive(false);
    }

    //HPBar��\��
    public void HPEnableTrue()
    {
        HP.gameObject.SetActive(true);
    }
}
