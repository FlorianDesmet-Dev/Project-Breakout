using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace ProjectBreakout;

internal class SceneMenu : Scene
{
    private Song Menu { get; set; }
    private Vector2 TextPosition { get; set; }
    private List<EnemyBlue> ListEnemyBlue { get; set; }
    private List<EnemyYellow> ListEnemyYellow { get; set; }

    public SceneMenu() : base()
    {
        TitleFont = _assets.GetFont("Title");
        SizeFont = TitleFont.MeasureString("COLORBREAKER");

        TitlePosition = new Vector2(
            _screenSize.width / 2 - SizeFont.X / 2,
            (_screenSize.height / 2) - SizeFont.Y);

        ShadePosition = new Vector2(
            _screenSize.width / 2 - SizeFont.X / 2 + 5,
            (_screenSize.height / 2) - SizeFont.Y + 5);

        TextFont = _assets.GetFont("SubTitle");
        SizeFont = TextFont.MeasureString("Alpha 1.0");

        TextPosition = new Vector2(
            _screenSize.width - SizeFont.X - 10,
            _screenSize.height - SizeFont.Y - 10);

        StartButton = new Button("Button");
        StartButton.Position = new Vector2(
            _screenSize.width / 2 - StartButton.Width / 2,
            (_screenSize.height / 2) + (StartButton.Height / 2));

        ListEnemyBlue = new List<EnemyBlue>();
        ListEnemyYellow = new List<EnemyYellow>();

        Random random = new Random();

        for (int i = 0; i <= 3; i++)
        {
            EnemyBlue newEnemyBlue = new("Enemy_1");
            newEnemyBlue.SetPosition(
                random.Next(0, _screenSize.width - newEnemyBlue.Width),
                random.Next(0, _screenSize.height - newEnemyBlue.Height));
            ListEnemyBlue.Add(newEnemyBlue);
        }

        for (int i = 0; i <= 3; i++)
        {
            EnemyYellow newEnemyYellow = new("Enemy_2");
            newEnemyYellow.SetPosition(
                random.Next(0, _screenSize.width - newEnemyYellow.Width),
                random.Next(0, _screenSize.height - newEnemyYellow.Height));
            ListEnemyYellow.Add(newEnemyYellow);
        }
    }    

    public override void Load()
    {
        Menu = _assets.GetSong("synthwave-palms");
        MediaPlayer.Play(Menu);
        MediaPlayer.IsRepeating = true;
        
        StartButton.OnClick = onClickPlay;        

        base.Load();
    }

    public override void Unload()
    {
        MediaPlayer.Stop();
        base.Unload();
    }

    public override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
        {
            _gameState.ChangeScene(GameState.SceneType.Gameplay);
        }

        StartButton.Update(gameTime);

        foreach (EnemyBlue enemyBlue in ListEnemyBlue)
        {
            enemyBlue.Update(gameTime);
        }

        foreach (EnemyYellow enemyYellow in ListEnemyYellow)
        {
            enemyYellow.Update(gameTime);
        }

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {           
        base.Draw(gameTime);
        
        foreach (EnemyBlue enemyBlue in ListEnemyBlue)
        {
            enemyBlue.Draw(gameTime);
        }

        foreach (EnemyYellow enemyYellow in ListEnemyYellow)
        {
            enemyYellow.Draw(gameTime);
        }

        _spriteBatch.Draw(StartButton.SpriteTexture, StartButton.Position, Color.White);
        _spriteBatch.DrawString(TitleFont, "COLORBREAKER", ShadePosition, Color.Red);
        _spriteBatch.DrawString(TitleFont, "COLORBREAKER", TitlePosition, Color.White);
        _spriteBatch.DrawString(TextFont, "Alpha 1.0", TextPosition, Color.White);
    }
}
