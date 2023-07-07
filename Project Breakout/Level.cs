using ProjectBreakout;

internal class Level
{
    public string Author { get; set; }
    public int Lines { get; set; }
    public int Columns { get; set; }
    public int[][] Map { get; set; }

    public ListBricks ListBricks { get; private set; }

    public Level()
    {
        ListBricks = new();
    }

    public void Load()
    {
        ListBricks.CreateBrick(Lines, Columns, Map);
    }

    public void Unload()
    {

    }

    public void Update()
    {

    }

    public void Draw()
    {
        ListBricks.Draw();
    }
}
