/**
 * @file   IS_XboxInput.cs
 * @brief  Xboxの入力クラス
 * @author IharaShota
 * @date   2023/06/12
 * @Update 2023/06/12 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_XBoxInput : MonoBehaviour
{
    static public string A = "joystick button 0";
    static public string B = "joystick button 1";
    static public string X = "joystick button 2";
    static public string Y = "joystick button 3";
    static public string LB = "joystick button 4";
    static public string RB = "joystick button 5";
    static public string View = "joystick button 6";
    static public string Menu = "joystick button 7";
    static public string LStick = "joystick button 8";
    static public string RStick = "joystick button 9";

    static public float LStick_H;
    static public float LStick_V;
    static public float RStick_H;
    static public float RStick_V;
    static public float DPad_H;
    static public float DPad_V;
    static public float LR_Trigger;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(A))
        //{
        //    Debug.Log("A");
        //}
        //if (Input.GetKeyDown(B))
        //{
        //    Debug.Log("B");
        //}
        //if (Input.GetKeyDown(X))
        //{
        //    Debug.Log("X");
        //}
        //if (Input.GetKeyDown(Y))
        //{
        //    Debug.Log("Y");
        //}
        //if (Input.GetKeyDown(LB))
        //{
        //    Debug.Log("LB");
        //}
        //if (Input.GetKeyDown(RB))
        //{
        //    Debug.Log("RB");
        //}
        //if (Input.GetKeyDown(View))
        //{
        //    Debug.Log("View");
        //}
        //if (Input.GetKeyDown(Menu))
        //{
        //    Debug.Log("Menu");
        //}
        //if (Input.GetKeyDown(LStick))
        //{
        //    Debug.Log("LStick");
        //}
        //if (Input.GetKeyDown(RStick))
        //{
        //    Debug.Log("RStick");
        //}
        LStick_H = Input.GetAxis("L_Stick_H");
        LStick_V = Input.GetAxis("L_Stick_V");
        //if ((LStick_H != 0) || (LStick_V != 0))
        //{
        //    Debug.Log("L_Stick:" + LStick_H + "," + LStick_V);
        //}

        RStick_H = Input.GetAxis("R_Stick_H");
        RStick_V = Input.GetAxis("R_Stick_V");
        //if ((RStick_H != 0) || (RStick_V != 0))
        //{
        //    Debug.Log("R_Stick:" + RStick_H + "," + RStick_V);
        //}

        DPad_H = Input.GetAxis("D_Pad_H");
        DPad_V = Input.GetAxis("D_Pad_V");
        //if ((DPad_H != 0) || (DPad_V != 0))
        //{
        //    Debug.Log("D_Pad:" + DPad_H + "," + DPad_V);
        //}

        LR_Trigger = Input.GetAxis("L_R_Trigger");
        //if (LR_Trigger > 0)
        //{
        //    Debug.Log("R trigger:" + LR_Trigger);
        //}
        //else if (LR_Trigger < 0)
        //{
        //    Debug.Log("L trigger:" + LR_Trigger);
        //}
    }
}
