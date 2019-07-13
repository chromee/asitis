using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyLazyLibrary;


public class HMDPosTracer : MonoBehaviour
{

    EasyOpenVRUtil util = new EasyOpenVRUtil(); //姿勢取得ライブラリ

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
        UpdateCameraTexture();
    }

    private void UpdateCameraTexture()
    {
        if (!util.IsReady())
        {
            util.Init();
            return;
        }

        var pos = util.GetHMDTransform();

        this.transform.position = pos.position;
        this.transform.rotation = pos.rotation;
    }
}
