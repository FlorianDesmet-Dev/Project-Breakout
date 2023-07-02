internal class Level
{
    public string Author { get; set; }
    public int Lines { get; private set; }
    public int Columns { get; private set; }
    public int[][] Map { get; set; }

    public Level()
    {
        Lines = 20;
        Columns = 18;
    }
}
