using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ON_PlayerShadow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var pos = target.transform.position;
       // pos.y = transform.position.y;
        pos.z = transform.position.z;
        transform.position = pos;
    }
}
