using Microsoft.Xna.Framework;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace ProjectBreakout
{
    internal class Brick : Sprite
    {
        public enum BrickType
        {
            Blue,
            Red,
            Yellow
        }

        public enum BrickState
        {
            Full,
            TwoBar,
            OneBar
        }

        public Brick(string pNameImage ,string pType, string pState) : base(pNameImage, pType, pState)
        {
            Life = 1;
        }

        public void ChangeState(BrickState pState)
        {
            switch (pState)
            {
                case BrickState.Full:
                    State = "Full";
                    break;
                case BrickState.TwoBar:
                    State = "2-3";
                    break;
                case BrickState.OneBar:
                    State = "1-3";
                    break;
                default:
                    break;
            }
            SpriteTexture = Asset.GetTexture(NameImage + "_" + Type + "_" + State);
        }

        public void ChangeType(BrickType pType)
        {
            switch (pType)
            {
                case BrickType.Blue:
                    Type = "Blue";
                    break;
                case BrickType.Red:
                    Type = "Red";
                    break;
                case BrickType.Yellow:
                    Type = "Yellow";
                    break;
                default:
                    break;
            }
            SpriteTexture = Asset.GetTexture(NameImage + "_" + Type + "_" + State);
        }

        public override void Load()
        {
            base.Load();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            switch (Life)
            {
                case 1:
                    ChangeState(Brick.BrickState.OneBar);
                    break;
                case 2:
                    ChangeState(Brick.BrickState.TwoBar);
                    break;
                case 3:
                    ChangeState(Brick.BrickState.Full);
                    break;
                default:
                    break;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
