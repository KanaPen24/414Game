/**
 * @file ON_BackScroll.cs
 * @brief 背景スクロールするためマテリアルのオフセットを調整する
 * @author Osaki Noriaki
 * @date 2023/03/31
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ON_BackScroll : MonoBehaviour
{
    [System.Serializable] private class ScrollObj
    {
       public GameObject Object;
       public float speed;
        [System.NonSerialized] public Material mat;
        [System.NonSerialized] public Vector4 offset;
    }


    [SerializeField] private Transform cameraPos;  // カメラの座標
    private Vector3 oldCamPos;  // 過去のカメラ座標
    [SerializeField] private ScrollObj[] objects;  // 背景スクロールさせるオブジェクト


    // Start is called before the first frame update
    void Start()
    {
        // マテリアルの取得
        if(objects.Length >0)
        {
            for(int i = 0; i < objects.Length; ++i)
            {
                objects[i].mat = objects[i].Object.GetComponent<Renderer>().material;
                objects[i].offset = new Vector4(.0f, .0f, .0f, .0f);
            }
        }

        oldCamPos = cameraPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (objects.Length <= 0)
            return;

        // カメラの移動量を取得
        var value = cameraPos.position.x - oldCamPos.x;

        for(int i = 0; i < objects.Length; ++i)
        {
            // 相対的に移動しない
            var position = objects[i].Object.transform.position;
            objects[i].Object.transform.position = new Vector3(cameraPos.position.x, position.y, position.z);

            // オフセットを調整
            objects[i].offset.x += value * objects[i].speed;
            objects[i].mat.SetVector("_Offset", objects[i].offset);
        }

        oldCamPos = cameraPos.position;
    }
}
