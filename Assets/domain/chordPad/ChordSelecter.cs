using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordSelecter : MonoBehaviour
{
    public Chord chord;
    public void Create(Chord chord)
    {
        this.chord = chord;
        Debug.Log(this.chord.root);
        Debug.Log(this.chord.type[0]);
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
