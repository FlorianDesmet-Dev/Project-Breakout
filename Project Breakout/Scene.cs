using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace ProjectBreakout
{
    internal abstract class Scene
    {
        protected SpriteBatch _spriteBatch { get; private set; }
        protected IGetAssets _assets { get; private set; }
        protected IScreenSize _screenSize { get; private set; }
        protected GameState _gameState { get; set; }

        protected SpriteFont TitleFont { get; set; }
        protected SpriteFont TextFont { get; set; }

        protected Vector2 SizeFont { get; set; }
        protected Vector2 TitlePosition { get; set; }
        protected Vector2 ShadePosition { get; set; }
        
        protected Button StartButton { get; set; }
        protected Background[] Backgrounds { get; private set; }

        protected KeyboardState NewKeyboardState { get; set; }
        protected KeyboardState OldKeyboardState { get; set; }

        public Scene()
        {
            _spriteBatch = ServiceLocator.GetService<SpriteBatch>();
            _assets = ServiceLocator.GetService<IGetAssets>();
            _screenSize = ServiceLocator.GetService<IScreenSize>();
            _gameState = ServiceLocator.GetService<GameState>();
            Backgrounds = new Background[5];
            
            float speed = -0.02f;

            for (int i = 0; i < Backgrounds.Length; i++)
            {
                Backgrounds[i] = new("Layer_" + i, speed);
                speed -= 0.04f;
            }
        }

        public void onClickPlay(Button pSender)
        {
            _gameState.ChangeScene(GameState.SceneType.Gameplay);
        }

        public virtual void Load()
        {
        }

        public virtual void Unload()
        {
            Debug.WriteLine("Unload");
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Draw(GameTime gameTime)
        {
            foreach (Background background in Backgrounds)
            {
                background.Draw(gameTime);
            }
        }
    }
}
