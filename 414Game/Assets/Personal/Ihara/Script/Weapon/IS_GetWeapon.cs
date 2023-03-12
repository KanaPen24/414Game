using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_GetWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            this.gameObject.transform.parent = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Playerが当たったら武器を持たせる(子オブジェクトにする)
        if(collision.gameObject.tag == "Player")
        {
            this.gameObject.transform.parent = GameObject.Find("PlayerWeapons").transform;
            //this.gameObject.SetActive(false);
        }
    }
}
