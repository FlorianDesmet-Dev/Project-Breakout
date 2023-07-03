using Microsoft.Xna.Framework;
using SharpDX;
using System;
using System.Diagnostics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProjectBreakout;

internal abstract class Enemy : Sprite
{
    public enum StateMachine
    {
        Idle,
        Appear,
        Move,
        ChangeDirection,
        Attack
    }

    public StateMachine SM { get; private set; }

    protected Random random;
    protected float direction;
    protected int randomDirection;
    protected float timerChangeDirection;
    protected Vector2 oldSpeed;

    public Enemy(string pNameImage) : base(pNameImage)
    {
        random = new();

        float speed_x;
        do
        {
            speed_x = random.NextFloat(-1.0f, 1.0f);
        } while (speed_x == 0.0f);

        Speed = new Vector2(speed_x, 1.0f);

        direction = random.Next(1, 5);
        randomDirection = random.Next(1, 3);
        timerChangeDirection = random.Next(1, 6);

        SM = StateMachine.Move;
    }

    public override void Load()
    {
        base.Load();
    }

    public virtual void ChangeStateMachine(StateMachine pStateMachine)
    {
        switch (pStateMachine)
        {
            case StateMachine.Idle:
                Speed = Vector2.Zero;
                break;
            case StateMachine.Appear:
                break;
            case StateMachine.Move:
                Move();
                break;
            case StateMachine.ChangeDirection:
                if (randomDirection == 1)
                {
                    ChangeDirectionX();
                }
                else if (randomDirection == 2)
                {
                    ChangeDirectionY();
                }
                randomDirection = random.Next(1, 3);
                break;
            case StateMachine.Attack:
                break;
            default:
                break;
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (SM == StateMachine.Move)
        {
            ChangeStateMachine(StateMachine.Move);

            direction -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Debug.WriteLine("Direction = " + direction);

            if (direction <= 0)
            {
                direction = random.Next(1, 5);
                oldSpeed = Speed;
                SM = StateMachine.Idle;
            }
        }
        else if (SM == StateMachine.Idle)
        {
            ChangeStateMachine(StateMachine.Idle);

            timerChangeDirection -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Debug.WriteLine("Timer Direction = " + timerChangeDirection);

            if (timerChangeDirection <= 0)
            {
                timerChangeDirection = random.Next(1, 6);
                SM = StateMachine.ChangeDirection;
            }
        }
        else if (SM == StateMachine.ChangeDirection)
        {
            Speed = oldSpeed;
            ChangeStateMachine(StateMachine.ChangeDirection);
            SM = StateMachine.Move;
        }
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}
