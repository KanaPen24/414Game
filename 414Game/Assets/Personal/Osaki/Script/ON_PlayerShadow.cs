/**
 * @file ON_PlayerShadow.cs
 * @brief プレイヤーの足元にある影
 * @author Osaki Noriaki
 * @date 2023/04/26
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ON_PlayerShadow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private IS_Player target;  // プレイヤー
    [SerializeField] private GameObject ground; // 地面
    [SerializeField] private float onGrounPosY = 0.4f;    // プレイヤーが地面の上に立っているときのY座標
    [SerializeField] private float radius = 45.0f;  // 影の移動する角度(ディグリース角
    private Vector3 defaultScale;
    void Start()
    {
        defaultScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        var state = target.GetSetPlayerState;
        var pos = target.gameObject.transform.position;
        var distance = Mathf.Abs(transform.position.y - onGrounPosY);
        switch (state)
        {
            case PlayerState.PlayerDrop:
                pos.x += Mathf.Cos(radius * Mathf.Deg2Rad);
                pos.y =  Mathf.Sin(radius * Mathf.Deg2Rad);
                break;
            case PlayerState.PlayerJump:
                pos.y = Mathf.Sin(radius * Mathf.Deg2Rad);
                pos.x += Mathf.Cos(radius * Mathf.Deg2Rad);
                break;
            default:
                // 地面に立っているとき
                pos.z = transform.position.z;
                break;
        }
        transform.position = pos;
        //transform.localScale = defaultScale * Mathf.Lerp(1.0f, 0.5f, distance);
    }
}
