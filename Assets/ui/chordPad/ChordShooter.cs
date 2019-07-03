using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EasyLazyLibrary;
public class ChordShooter : MonoBehaviour
{
    public bool isTriggerPressed = false;
    private ulong button;
    public int velocity = 127;

    private Vector3 prePos = new Vector3(0, 0, 0);
    private Vector3 preV = new Vector3(0, 0, 0);

    public Vector3 accele;
    public float acceleAvg;

    public enum LR
    {
        Left, Right
    }

    public LR controller;
    EasyOpenVRUtil util = new EasyOpenVRUtil();

    // Use this for initialization
    void Start()
    {
        util.Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!util.IsReady())
        {
            util.Init();
            return;
        }

        button = 0;

        Vector3 nowPos;
        if (controller == LR.Left)
        {
            util.GetControllerButtonPressed(util.GetLeftControllerIndex(), out button);
            nowPos = util.GetLeftControllerTransform().position;
        }
        else
        {
            util.GetControllerButtonPressed(util.GetRightControllerIndex(), out button);
            nowPos = util.GetRightControllerTransform().position;
        }

        /**
        * 加速度
        */
        float deltaTime = 1; //1フレーム
        Vector3 v = (nowPos - prePos) / deltaTime;
        Vector3 a = (v - preV) / deltaTime;
        accele = a * 10000;
        acceleAvg = Mathf.Sqrt(
            (a.x * a.x) +
            (a.y * a.y) +
            (a.z * a.z)
        );

        prePos = nowPos;
        preV = v;

        /**
         * コードショット部分
         */
        if (isTriggerPressed)
        {
            //トリガー押されてる状態から離されるとコードを放つ
            if (button != ViveConstants.TRIGGER)
            {
                ChordPadManager.instance.Ring(nowPos.y - 0.5f);
                Debug.Log(nowPos.y - 0.5f);
                isTriggerPressed = false;
            }
        }
        else
        {
            //トリガーが離されてる状態から押されるとコードを止める
            if (button == ViveConstants.TRIGGER)
            {
                ChordPadManager.instance.Mute();
                isTriggerPressed = true;
            }
        }
    }
}
