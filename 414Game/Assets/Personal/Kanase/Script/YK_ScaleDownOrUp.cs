using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YK_ScaleDownOrUp : MonoBehaviour
{
    private float time, changeSpeed;
    private bool enlarge;
    private bool m_bCollision;
    private Vector2 Scale;  //初期値を保存

    void Start()
    {
        enlarge = true;
        //初期値を保存
        Scale = transform.localScale;
    }

    void Update()
    {
        if (!m_bCollision)
            return;
        changeSpeed = Time.deltaTime * 0.8f;

        if (time < 0)
        {
            enlarge = true;
        }
        if (time > 0.45f)
        {
            enlarge = false;
        }

        if (enlarge == true)
        {
            time += Time.deltaTime;
            transform.localScale += new Vector3(changeSpeed, changeSpeed, changeSpeed);
        }
        else
        {
            time -= Time.deltaTime;
            transform.localScale -= new Vector3(changeSpeed, changeSpeed, changeSpeed);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Cursol")
        {
            m_bCollision = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Cursol")
        {
            m_bCollision = false;
            transform.localScale = Scale;
        }
    }
}