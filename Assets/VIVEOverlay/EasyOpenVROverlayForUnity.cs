/**
 * ModifiedEasyOpenVROverlayForUnity by memex_pibo v0.01
 * 
 * original: EasyOpenVROverlayForUnity by gpsnmeajp
 * https://sabowl.sakura.ne.jp/gpsnmeajp/
 */

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR; //Steam VR


public class EasyOpenVROverlayForUnity : MonoBehaviour
{
    
    //エラーフラグ
    public bool error = true; //初期化失敗
    //イベントに関するログを表示するか
    public bool eventLog = false;

    [Header("RenderTexture")]
    //取得元のRenderTexture
    public RenderTexture renderTexture;

    [Header("Name")]
    //ユーザーが確認するためのオーバーレイの名前
    public string OverlayFriendlyName = "SampleOverlay";

    //グローバルキー(システムのオーバーレイ同士の識別名)。
    //ユニークでなければならない。乱数やUUIDなどを勧める
    public string OverlayKeyName = "SampleOverlay";

    [Header("DeviceTracking")]
    //絶対空間か
    public bool DeviceTracking = true;


    [Header("Absolute space")]
    //(絶対空間の場合)ルームスケールか、着座状態か
    public bool Seated = false;

    //着座カメラのリセット(リセット後自動でfalseに戻ります)
    public bool ResetSeatedCamera = false;

    //--------------------------------------------------------------------------

    [Header("Device Info")]
    //現在接続されているデバイス一覧をログに出力(自動でfalseに戻ります)
    public bool putLogDevicesInfo = false;
    //(デバイスを選択した時点で)現在接続されているデバイス数
    public int ConnectedDevices = 0;
    //選択デバイス番号
    public int SelectedDeviceIndex = 0;
    //選択デバイスのシリアル番号
    public string DeviceSerialNumber = null;
    //選択デバイスのモデル名
    public string DeviceRenderModelName = null;

    //右手か左手か
    enum LeftOrRight
    {
        Left = 0,
        Right = 1
    }

    //--------------------------------------------------------------------------

    //オーバーレイのハンドル(整数)
    private ulong overlayHandle = INVALID_HANDLE;

    //OpenVRシステムインスタンス
    private CVRSystem openvr = null;

    //Overlayインスタンス
    private CVROverlay overlay = null;

    //オーバーレイに渡すネイティブテクスチャ
    private Texture_t overlayTexture;

    //HMD視点位置変換行列
    private HmdMatrix34_t p;

    //無効なハンドル
    private const ulong INVALID_HANDLE = 0;

    //オーバーレイの大きさ設定(幅のみ。高さはテクスチャの比から自動計算される)
    private float width = 0.382f;


    //--------------------------------------------------------------------------

    //Overlayが表示されているかどうか外部からcheck
    public bool IsVisible()
    {
        return overlay.IsOverlayVisible(overlayHandle) && !IsError();
    }

    //エラー状態かをチェック
    public bool IsError()
    {
        return error || overlayHandle == INVALID_HANDLE || overlay == null || openvr == null;
    }

    //エラー処理(開放処理)
    private void ProcessError()
    {

#pragma warning disable 0219
        string Tag = "[" + this.GetType().Name + ":" + System.Reflection.MethodBase.GetCurrentMethod(); //クラス名とメソッド名を自動取得
#pragma warning restore 0219
        Debug.Log(Tag + "Begin");

        //ハンドルを解放
        if (overlayHandle != INVALID_HANDLE && overlay != null)
        {
            overlay.DestroyOverlay(overlayHandle);
        }

        overlayHandle = INVALID_HANDLE;
        overlay = null;
        openvr = null;
        error = true;
    }

    //オブジェクト破棄時
    private void OnDestroy()
    {

#pragma warning disable 0219
        string Tag = "[" + this.GetType().Name + ":" + System.Reflection.MethodBase.GetCurrentMethod(); //クラス名とメソッド名を自動取得
#pragma warning restore 0219
        Debug.Log(Tag + "Begin");

        //ハンドル類の全開放
        ProcessError();
    }

    //アプリケーションの終了を検出した時
    private void OnApplicationQuit()
    {

#pragma warning disable 0219
        string Tag = "[" + this.GetType().Name + ":" + System.Reflection.MethodBase.GetCurrentMethod(); //クラス名とメソッド名を自動取得
#pragma warning restore 0219
        Debug.Log(Tag + "Begin");

        //ハンドル類の全開放
        ProcessError();
    }

    //アプリケーションを終了させる
    private void ApplicationQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    //--------------------------------------------------------------------------

    //初期化処理
    private void Start()
    {

#pragma warning disable 0219
        string Tag = "[" + this.GetType().Name + ":" + System.Reflection.MethodBase.GetCurrentMethod(); //クラス名とメソッド名を自動取得
#pragma warning restore 0219
        Debug.Log(Tag + "Begin");

        var openVRError = EVRInitError.None;
        var overlayError = EVROverlayError.None;
        error = false;

        //フレームレートを90fpsにする。(しないと無限に早くなることがある)
        Application.targetFrameRate = 120;
        Debug.Log(Tag + "Set Frame Rate 90");

        //OpenVRの初期化
        openvr = OpenVR.Init(ref openVRError, EVRApplicationType.VRApplication_Overlay);
        if (openVRError != EVRInitError.None)
        {
            Debug.LogError(Tag + "OpenVRの初期化に失敗." + openVRError.ToString());
            ProcessError();
            return;
        }

        //オーバーレイ機能の初期化
        overlay = OpenVR.Overlay;
        overlayError = overlay.CreateOverlay(OverlayKeyName, OverlayFriendlyName, ref overlayHandle);
        if (overlayError != EVROverlayError.None)
        {
            Debug.LogError(Tag + "Overlayの初期化に失敗. " + overlayError.ToString());
            ProcessError();
            return;
        }

        Debug.Log(overlayError);

        var OverlayTextureBounds = new VRTextureBounds_t();
        //pTexture
        overlayTexture.eType = ETextureType.DirectX;
        //上下反転する
        OverlayTextureBounds.uMin = 0;
        OverlayTextureBounds.vMin = 1;
        OverlayTextureBounds.uMax = 1;
        OverlayTextureBounds.vMax = 0;
        overlay.SetOverlayTextureBounds(overlayHandle, ref OverlayTextureBounds);

        Debug.Log(Tag + "初期化完了しました");
    }


    private void Update()
    {
        

#pragma warning disable 0219
        string Tag = "[" + this.GetType().Name + ":" + System.Reflection.MethodBase.GetCurrentMethod(); //クラス名とメソッド名を自動取得
#pragma warning restore 0219

        //エラーが発生した場合や、ハンドルが無効な場合は実行しない
        if (IsError())
        {
            return;
        }


        //イベントを処理する(終了された時true)
        if (ProcessEvent())
        {
            Debug.Log(Tag + "VRシステムが終了されました");
            ApplicationQuit();
        }

        //サイドバイサイド3D
        overlay.SetOverlayFlag(overlayHandle, VROverlayFlags.SideBySide_Parallel, true);
        overlay.ShowOverlay(overlayHandle);

        
        //オーバーレイが表示されている時
        if (overlay.IsOverlayVisible(overlayHandle))
        {

            if (overlay.IsOverlayVisible(overlayHandle))
            {
                //widthをセット　インスペクタからいじれる
                overlay.SetOverlayWidthInMeters(overlayHandle, width);

                //HMD視点位置変換行列に書き込む。
                //ここでは回転なし、平行移動ありのHUD的な状態にしている。
                var wx = -0f;
                var wy = -0f;
                var wz = -0.1f;
                
                p.m0 = 1; p.m1 = 0; p.m2 = 0; p.m3 = wx;
                p.m4 = 0; p.m5 = 1; p.m6 = 0; p.m7 = wy;
                p.m8 = 0; p.m9 = 0; p.m10 = 1; p.m11 = wz;

                //回転行列を元に、HMDからの相対的な位置にオーバーレイを表示する。
                //代わりにSetOverlayTransformAbsoluteを使用すると、ルーム空間に固定することができる
                uint indexunTrackedDevice = 0;//0=HMD(他にControllerやTrackerにすることもできる)
                overlay.SetOverlayTransformTrackedDeviceRelative(overlayHandle, indexunTrackedDevice, ref p);

                //RenderTextureが生成されているかチェック
                if (!renderTexture.IsCreated())
                {
                    Debug.Log(Tag + "RenderTextureがまだ生成されていない");
                    return;
                }

                //RenderTextureからネイティブテクスチャのハンドルを取得
                try
                {
                    overlayTexture.handle = renderTexture.GetNativeTexturePtr();
                }
                catch (UnassignedReferenceException e)
                {
                    Debug.LogError(Tag + "RenderTextureがセットされていません");
                    ApplicationQuit();
                    return;
                }

                //オーバーレイにテクスチャを設定
                var overlayError = EVROverlayError.None;
                overlayError = overlay.SetOverlayTexture(overlayHandle, ref overlayTexture);
                if (overlayError != EVROverlayError.None)
                {
                    Debug.LogError(Tag + "Overlayにテクスチャをセットできませんでした. " + overlayError.ToString());
                    ApplicationQuit();
                    return;
                }
            }

        }

 
        

    }

  
    //終了イベントをキャッチした時に戻す
    private bool ProcessEvent()
    {

#pragma warning disable 0219
        string Tag = "[" + this.GetType().Name + ":" + System.Reflection.MethodBase.GetCurrentMethod(); //クラス名とメソッド名を自動取得
#pragma warning restore 0219

        //イベント構造体のサイズを取得
        uint uncbVREvent = (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(VREvent_t));

        //イベント情報格納構造体
        VREvent_t Event = new VREvent_t();
        //イベントを取り出す
        while (overlay.PollNextOverlayEvent(overlayHandle, ref Event, uncbVREvent))
        {
            //イベントのログを表示
            if (eventLog)
            {
                Debug.Log(Tag + "Event:" + ((EVREventType)Event.eventType).ToString());
            }

            //イベント情報で分岐
            switch ((EVREventType)Event.eventType)
            {
                case EVREventType.VREvent_Quit:
                    Debug.Log(Tag + "Quit");
                    return true;
            }
        }
        return false;
    }


   
    


}