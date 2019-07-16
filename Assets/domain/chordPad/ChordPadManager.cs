using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordPadManager : MonoBehaviour
{
    [SerializeField]
    private GameObject chordSelecterPrefab;
    List<int> notes = new List<int>(){
        //CMaj
        12, 24, 28, 31
    };

    MidiChannel channel = MidiChannel.Ch1;

    void Start()
    {
        //TODO: ここ関数にする
        GameObject chordSelecter = Instantiate(chordSelecterPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        ChordSelecter cs1 = chordSelecter.GetComponent<ChordSelecter>();
        cs1.Create(chord: new Chord(PitchName.C, Type.maj));
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