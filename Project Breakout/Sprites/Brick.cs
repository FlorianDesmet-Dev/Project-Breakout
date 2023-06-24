using Microsoft.Xna.Framework;

namespace ProjectBreakout
{
    internal class Brick : Sprite
    {
        public enum BrickType
        {
            Green,
            Orange,
            Violet
        }

        public enum BrickState
        {
            Intact,
            Crack,
            Break
        }

        public string Type { get; private set; }
        public string State { get; private set; }
        public int Strength { get; set; }

        public Brick(string pType, string pState) : base()
        {
            Type = pType;
            State = pState;
            SpriteTexture = Asset.GetTexture("Brick_" + Type + "_" + State);

            Strength = 3;
        }

        public void ChangeState(BrickState pState)
        {
            switch (pState)
            {
                case BrickState.Intact:
                    State = "intact";
                    break;
                case BrickState.Crack:
                    State = "crack";
                    break;
                case BrickState.Break:
                    State = "break";
                    break;
                default:
                    break;
            }
            SpriteTexture = Asset.GetTexture("Brick_" + Type + "_" + State);
        }

        public void ChangeType(BrickType pType)
        {
            switch (pType)
            {
                case BrickType.Green:
                    Type = "green";
                    break;
                case BrickType.Orange:
                    Type = "orange";
                    break;
                case BrickType.Violet:
                    Type = "violet";
                    break;
                default:
                    break;
            }
            SpriteTexture = Asset.GetTexture("Brick_" + Type + "_" + State);
        }

        public override void Load()
        {
            base.Load();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Batch.DrawString(Asset.GetFont("FontMenu"), "" + Strength, Position, Color.White);
        }
    }
}
