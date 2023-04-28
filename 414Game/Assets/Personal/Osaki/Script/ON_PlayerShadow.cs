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
    [SerializeField] private float distance = 0.8f;    // プレイヤーが地面の上に立っているときのプレイヤーと地面との差
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var state = target.GetSetPlayerState;
        var pos = target.gameObject.transform.position;
        switch (state)
        {
            case PlayerState.PlayerDrop:
                break;
            case PlayerState.PlayerJump:
                break;
            default:
                // 地面に立っているとき
                pos.z = transform.position.z;
                transform.position = pos;
                break;
        }
    }
}
