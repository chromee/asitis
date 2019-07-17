using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ChordSelecter
 * ChordPadManagerによって自動生成される
 * 生成される時点でユニークなコードネームが割り当てられる
 * isTriggerを持つRigidBodyが自身のColliderに侵入するとChordPadManagerにコードの変更を伝える
 */
public class ChordSelecter : MonoBehaviour
{
    public Chord chord;
    private ChordPadManager creator;
    public void Create(Chord chord, ChordPadManager creator)
    {
        this.chord = chord;
        this.creator = creator;
        Debug.Log(chord.root);
    }

    void OnTriggerEnter()
    {
        creator.setChord(chord);
        Debug.Log("trigger haitta" + creator);
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
