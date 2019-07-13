using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordPadManager : MonoBehaviour
{
    List<int> notes = new List<int>(){
        //CMaj
        12, 24, 28, 31
    };

    MidiChannel channel = MidiChannel.Ch1;
    private static ChordPadManager _instance;

    public static ChordPadManager instance
    {
        get
        {
            if (!_instance)
            {
                var go = new GameObject("ChordPadManager");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<ChordPadManager>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void Ring(float velocity)//velocity range: 0~1f
    {
        foreach (int item in notes)
        {
            MidiOut.SendNoteOn(channel, item, velocity);
        }
    }
    public void Mute()
    {
        foreach (int item in notes)
        {
            MidiOut.SendNoteOff(channel, item);
        }
    }
}