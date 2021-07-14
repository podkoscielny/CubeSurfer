[System.Serializable]
public struct Highscore
{
    public int score;
    public int level;

    public Highscore(int score, int level)
    {
        this.score = score;
        this.level = level;
    }
}
