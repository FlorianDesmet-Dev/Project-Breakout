using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ProjectBreakout
{
    internal class Paddle : Sprite
    {
        public enum PaddleType
        {
            Blue,
            Red,
            Yellow
        }

        public enum PaddleState
        {
            Small,
            Large,
            XLarge,
            XXLarge
        }

        public float Position_x { get; set; }
        public float Position_y { get; set; }

        MouseState oldMouseState;

        public Paddle(string pNameImage, string pType, string pState) : base(pNameImage, pType, pState)
        {
            Life = 3;

            Position_x = ScreenSize.width / 2 - (SpriteTexture.Width / 2);
            Position_y = ScreenSize.height - (SpriteTexture.Height * 2);
        }

        public override void Load()
        {
            SetPosition(Position_x, Position_y);

            base.Load();
        }

        public void ChangeState(PaddleState pState)
        {
            switch (pState)
            {
                case PaddleState.Small:
                    State = "Small";
                    break;
                case PaddleState.Large:
                    State = "Large";
                    break;
                case PaddleState.XLarge:
                    State = "XLarge";
                    break;
                case PaddleState.XXLarge:
                    State = "XXLarge";
                    break;
                default:
                    break;
            }
            SpriteTexture = Asset.GetTexture(NameImage + "_" + Type + "_" + State);
        }

        public void ChangeType(PaddleType pType)
        {
            switch(pType)
            {
                case PaddleType.Blue:
                    Type = "Blue";
                    break;
                case PaddleType.Red:
                    Type = "Red";
                    break;
                case PaddleType.Yellow:
                    Type = "Yellow";
                    break;
                default:
                    break;
            }
            SpriteTexture = Asset.GetTexture(NameImage + "_" + Type + "_" + State);
        }

        public void Controller()
        {
            float mouseGSX = Mouse.GetState().X - (Width) / 2;
            SetPosition(mouseGSX, Position.Y);

            if (Position.X + Width >= ScreenSize.width)
            {
                Position = new Vector2(ScreenSize.width - Width, Position.Y);
            }
            else if (Position.X <= 0)
            {
                Position = new Vector2(0, Position.Y);
            }

            MouseState newMouseState = Mouse.GetState();

            if (newMouseState.RightButton == ButtonState.Pressed &&
                oldMouseState != newMouseState)
            {
                switch (Type)
                {
                    case "Blue":
                        ChangeType(PaddleType.Red);
                        break;
                    case "Red":
                        ChangeType(PaddleType.Yellow);
                        break;
                    case "Yellow":
                        ChangeType(PaddleType.Blue);
                        break;
                    default:
                        break;
                }
            }

            oldMouseState = newMouseState;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Batch.DrawString(
                Asset.GetFont("SubTitle"), 
                "Life : " + Life, 
                new Vector2(10, ScreenSize.height - 24), 
                Color.White);
        }
    }
}
