using Microsoft.Xna.Framework;
using SharpDX;
using System;
using System.Collections.Generic;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProjectBreakout;

internal abstract class Enemy : Sprite
{
    public enum EnemyState
    {
        Idle,
        Appear,
        Move,
        ChangeDirection
    }

    public EnemyState EState { get; private set; }

    private Random Random { get; set; }
    private float Direction { get; set; }
    private float RandomDirection { get; set; }
    private float TimerChangeDirection { get; set; }
    private Vector2 OldSpeed { get; set; }
    private float Alpha { get; set; }

    public Enemy(string pNameImage) : base(pNameImage)
    {
        Random = new();

        float speed_x;
        do
        {
            speed_x = Random.NextFloat(-1.0f, 1.0f);
        } while (speed_x == 0.0f);

        Speed = new Vector2(speed_x, 1.0f);

        Direction = Random.Next(1, 4);
        RandomDirection = Random.Next(1, 3);
        TimerChangeDirection = Random.Next(1, 6);
        Alpha = 0f;

        EState = EnemyState.Appear;
    }

    public override void Load()
    {
        base.Load();
    }

    public virtual void ChangeState(GameTime gameTime, EnemyState pState)
    {
        switch (pState)
        {
            case EnemyState.Idle:
                Speed = Vector2.Zero;
                break;
            case EnemyState.Appear:
                if (Alpha <= 1)
                {
                    Alpha += 0.2f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.ChangeDirection:
                if (RandomDirection == 1)
                {
                    ChangeDirectionX();
                }
                else if (RandomDirection == 2)
                {
                    ChangeDirectionY();
                }
                RandomDirection = Random.Next(1, 3);
                break;
            default:
                break;
        }
    }

    public virtual void StateMachine(GameTime gameTime)
    {
        if (EState == EnemyState.Appear)
        {
            ChangeState(gameTime, EnemyState.Appear);

            if (Alpha >= 1)
            {
                EState = EnemyState.Move;
            }
        }
        else if (EState == EnemyState.Move)
        {
            ChangeState(gameTime, EnemyState.Move);

            Direction -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Direction <= 0)
            {
                Direction = Random.Next(1, 4);
                OldSpeed = Speed;
                EState = EnemyState.Idle;
            }

            if (Position.Y + Height >= (ScreenSize.height / 4) * 3)
            {
                Position = new Vector2(Position.X, (ScreenSize.height / 4) * 3 - Height);
                ChangeDirectionY();
            }
        }
        else if (EState == EnemyState.Idle)
        {
            ChangeState(gameTime, EnemyState.Idle);

            TimerChangeDirection -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (TimerChangeDirection <= 0)
            {
                TimerChangeDirection = Random.Next(1, 6);
                EState = EnemyState.ChangeDirection;
            }
        }
        else if (EState == EnemyState.ChangeDirection)
        {
            Speed = OldSpeed;
            ChangeState(gameTime, EnemyState.ChangeDirection);
            EState = EnemyState.Move;
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        StateMachine(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Draw(SpriteTexture, Position, Microsoft.Xna.Framework.Color.White * Alpha);
    }
}
