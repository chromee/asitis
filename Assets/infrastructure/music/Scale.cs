using MusicTheory;
//Aメジャーの2ndNoteの3オクターブ目！って言われたら23(B0) + 12 * 2 = 47を返すクラス
interface Scale
{
    int note(int degree, int octave);
}


public class MajorScale : Scale
{
    PitchName key;
    //static int numberOfPitch; degrees.countでいいじゃん
    public int[] degrees = new int[7] { 0, 2, 4, 5, 7, 9, 11 };

    public MajorScale(PitchName key)
    {
        this.key = key;
    }

    public int note(int degree, int octave)
    {
        return (int)key + degrees[degree] + 12 * octave;
    }
}