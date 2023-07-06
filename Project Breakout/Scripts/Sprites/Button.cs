using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace ProjectBreakout;

internal delegate void OnClick(Button pSender);

internal class Button : Sprite

{
    public bool IsHover { get; private set; }
    public OnClick OnClick { get; set; }

    private MouseState oldMouseState;

    public Button(string pNameImage) : base(pNameImage)
    {

    }

    public override void Update(GameTime pGameTime)
    {
        MouseState newMouseState = Mouse.GetState();
        Point MousePos = newMouseState.Position;

        if (BoundingBox.Contains(MousePos))
        {
            if (!IsHover)
            {
                IsHover = true;
                Debug.WriteLine("The button is now hover !");
            }
        }
        else
        {
            if (IsHover)
            {
                Debug.WriteLine("The button is no more hover !");
            }
            IsHover = false;
        }

        if (IsHover)
        {
            if (newMouseState.LeftButton == ButtonState.Pressed &&
                oldMouseState.LeftButton == ButtonState.Released)
            {
                Debug.WriteLine("Button is clicked");

                if (OnClick != null)
                {
                    OnClick(this);
                }
            }
        }

        oldMouseState = newMouseState;

        base.Update(pGameTime);
    }
}
