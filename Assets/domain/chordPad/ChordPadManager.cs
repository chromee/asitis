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

    private Chord nowChord;

    MidiChannel channel = MidiChannel.Ch1;

    void Start()
    {
        var pitchNames = new[] {(PitchName.C, 0f), (PitchName.D, 1f), (PitchName.E, 2f)};

        foreach (var pitchName in pitchNames)
        {
            //TODO: ここ関数にする
            GameObject chordSelecter = Instantiate(chordSelecterPrefab, new Vector3(pitchName.Item2, 0f, 0f),
                Quaternion.identity);
            ChordSelecter cs1 = chordSelecter.GetComponent<ChordSelecter>();
            cs1.Create(chord: new Chord(pitchName.Item1, Type.maj));
        }
    }

    public void setChord(Chord receivedChord)
    {
        //多分だけどここにイベント入れておくとこのイベントに登録したGraphicとかOSCのOutができる気がする
        this.nowChord = receivedChord;
        Debug.Log(this.nowChord.root);
        Debug.Log(this.nowChord.type[0]);
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