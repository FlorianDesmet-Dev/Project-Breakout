using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace ProjectBreakout
{
    internal abstract class Scene
    {
        protected SpriteBatch _spriteBatch { get; private set; }
        protected IGetAssets _assets { get; private set; }
        protected IScreenSize _screenSize { get; private set; }
        protected GameState _gameState { get; set; }

        private Background[] Layers { get; set; }

        public Scene()
        {
            _spriteBatch = ServiceLocator.GetService<SpriteBatch>();
            _assets = ServiceLocator.GetService<IGetAssets>();
            _screenSize = ServiceLocator.GetService<IScreenSize>();
            _gameState = ServiceLocator.GetService<GameState>();
        }

        public virtual void Load()
        {
            Layers = new Background[5];
            float speed = -0.1f;

            for (int i = 0; i < Layers.Length; i++)
            {
                Layers[i] = new("Layer_" + i, speed);
                speed -= 0.2f;
            }
        }

        public virtual void Unload()
        {
            Debug.WriteLine("Unload");
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (Background layer in Layers)
            {
                layer.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            foreach (Background layer in Layers)
            {
                layer.Draw(gameTime);
            }
        }
    }
}
