using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectBreakout
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ScreenManager screenSize;
        private AssetsManager assetsManager;

        public GameState GameState { get; set; }

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            screenSize = new ScreenManager(_graphics);
            assetsManager = new AssetsManager(Content);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            GameState = new GameState();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 576;
            _graphics.PreferredBackBufferHeight = 324;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ServiceLocator.RegisterService<SpriteBatch>(_spriteBatch);
            ServiceLocator.RegisterService<IScreenSize>(screenSize);
            ServiceLocator.RegisterService<IGetAsset>(assetsManager);

            GameState.ChangeScene(GameState.SceneType.Menu);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (GameState.CurrentScene != null)
            {
                GameState.CurrentScene.Update(gameTime);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                GameState.ChangeScene(GameState.SceneType.Gameplay);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            if (GameState.CurrentScene != null)
            {
                GameState.CurrentScene.Draw(gameTime);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}