/**
 * @file YK_Controller.cs
 * @brief コントローラーの処理
 * @author 吉田叶聖
 * @date 2023/06/03
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class YK_Controller : MonoBehaviour
{
    public static YK_Controller instance;
    //ゲームパットの設定
    private Gamepad gamepad = Gamepad.current;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void ControllerVibration(float time)
    {
        //ゲームパットが接続されてるかどうか
        if (gamepad == null)
        {
            Debug.Log("ゲームパッド未接続");
            return;
        }

        //SetMotorSpeeds(Left,Right)0.0～1.0
        //引数見るとLowとHighになってるが実質左右の差
        gamepad.SetMotorSpeeds(1.0f, 1.0f);

        // StopVibrationをtime秒後に呼び出す
        Invoke(nameof(StopVibration), time);

    }

    //バイブレーションを止める
    public void StopVibration()
    {
        gamepad.SetMotorSpeeds(0.0f, 0.0f);
    }
}
