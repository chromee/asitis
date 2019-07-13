using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyLazyLibrary;


public class ControllerPosTracer : MonoBehaviour
{

    EasyOpenVRUtil util = new EasyOpenVRUtil(); //姿勢取得ライブラリ
    public enum LR
    {
        Left, Right
    }

    public LR controller;


    // Use this for initialization
    void Start()
    {
        //姿勢取得ライブラリを初期化
        if (!util.IsReady())
        {
            util.Init();
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePos();
    }

    private void UpdatePos()
    {
        if (!util.IsReady())
        {
            util.Init();
            return;
        }

        EasyOpenVRUtil.Transform pos;
        if (controller == LR.Left)
        {
            pos = util.GetLeftControllerTransform();
        }
        else
        {
            pos = util.GetRightControllerTransform();
        }
        this.transform.position = pos.position;
        this.transform.rotation = pos.rotation;
    }
}
