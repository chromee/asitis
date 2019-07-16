using System.Collections;
using System.Collections.Generic;
public static class Type
{
    public static List<Degree> maj = new List<Degree>() { Degree.M3, Degree.P5 };
    public static List<Degree> min = new List<Degree>() { Degree.m3, Degree.P5 };
    public static List<Degree> sus4 = new List<Degree>() { Degree.P4, Degree.P5 };
    public static List<Degree> flat5 = new List<Degree>() { Degree.m3, Degree.d5 };
    public static List<Degree> maj7 = new List<Degree>() { Degree.M3, Degree.P5, Degree.M7 };
    public static List<Degree> min7 = new List<Degree>() { Degree.m3, Degree.P5, Degree.m7 };
    public static List<Degree> seventh = new List<Degree>() { Degree.M3, Degree.P5, Degree.m7 };
    public static List<Degree> mM7 = new List<Degree>() { Degree.m3, Degree.P5, Degree.M7 };
    public static List<Degree> seventhSus4 = new List<Degree>() { Degree.P4, Degree.P5, Degree.m7 };
    public static List<Degree> min7Flat5 = new List<Degree>() { Degree.m3, Degree.d5, Degree.m7 };
}