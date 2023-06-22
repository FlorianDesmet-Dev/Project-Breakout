using Microsoft.Xna.Framework;
using System.IO;
using System.Text.Json;

namespace ProjectBreakout
{
    internal class Level
    {
        public int Lines { get; private set; }
        public int Columns { get; private set; }
        public int[][] Map { get; set; }

        public Level()
        {
            Lines = 10; 
            Columns = 18;
        }

        public virtual void Load()
        {
            
        }

        public void CreateLevel()
        {
            {
                Map = new int[Lines][];
                Map[0] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                Map[1] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                Map[2] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                Map[3] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                Map[4] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                Map[5] = new int[] { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 };
                Map[6] = new int[] { 0, 0, 0, 0, 1, 1, 2, 2, 2, 2, 2, 2, 1, 1, 0, 0, 0, 0 };
                Map[7] = new int[] { 0, 0, 0, 0, 1, 1, 2, 3, 3, 3, 3, 2, 1, 1, 0, 0, 0, 0 };
                Map[8] = new int[] { 0, 0, 0, 0, 1, 1, 2, 2, 2, 2, 2, 2, 1, 1, 0, 0, 0, 0 };
                Map[9] = new int[] { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 };
            }
        }

        public void Save(int pNumero)
        {
            string jsonLevel = JsonSerializer.Serialize(this);
            File.WriteAllText("level" + pNumero + ".json", jsonLevel);
        }

        public void Unload()
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime)
        {

        }
    }
}
