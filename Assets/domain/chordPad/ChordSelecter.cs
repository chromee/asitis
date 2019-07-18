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

    public void Create(Chord chord)
    {
        this.chord = chord;
        Debug.Log(chord.root);
    }
}