using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YK_Book : MonoBehaviour
{
    private bool m_Hit;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_Hit = true;
        }
        Destroy(gameObject);
    }
}
