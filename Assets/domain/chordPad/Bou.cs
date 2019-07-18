using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bou : MonoBehaviour
{
    [SerializeField] private ChordPadManager manager;

    void OnTriggerEnter(Collider collider)
    {
        var selector = collider.GetComponent<ChordSelecter>();
        if (selector == null) return;
        manager.setChord(selector.chord);
    }
}