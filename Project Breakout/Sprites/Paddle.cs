using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ProjectBreakout
{
    internal class Paddle : Sprite
    {
        public enum PaddleType
        {
            Green,
            Orange,
            Violet
        }

        public enum PaddleState
        {
            Small,
            Large,
            Extra_large
        }

        public string Type { get; private set; }
        public string State { get; private set; }

        public float Position_x { get; set; }
        public float Position_y { get; set; }

        public Paddle(string pType, string pState) : base()
        {
            Type = pType;
            State = pState;
            SpriteTexture = Asset.GetTexture("Paddle_" + Type + "_" + State);

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
                    State = "small";
                    break;
                case PaddleState.Large:
                    Type = "large";
                    break;
                case PaddleState.Extra_large:
                    Type = "extra_large";
                    break;
                default:
                    break;
            }
            SpriteTexture = Asset.GetTexture("Paddle_" + Type + "_" + State);
        }

        public void ChangeType(PaddleType pType)
        {
            switch(pType)
            {
                case PaddleType.Green:
                    Type = "green";
                    break;
                case PaddleType.Orange:
                    Type = "orange";
                    break;
                case PaddleType.Violet:
                    Type = "violet";
                    break;
                default:
                    break;
            }
            SpriteTexture = Asset.GetTexture("Paddle_" + Type + "_" + State);
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

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
