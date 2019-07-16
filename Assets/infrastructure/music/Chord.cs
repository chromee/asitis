using System.Collections;
using System.Collections.Generic;
public class Chord
{
    public PitchName root;
    public List<Degree> type;
    public Chord(PitchName root, List<Degree> type)
    {
        this.root = root;
        this.type = type;
    }
}
