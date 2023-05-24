using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YK_Tonkathi : MonoBehaviour
{
    [SerializeField] private IS_Player Player;
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            Player.GetSetItemHit = true;
        }
        Destroy(gameObject);
    }
}
