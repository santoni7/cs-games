using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SmartPackmans.GameSystem;
using SpaceShooter.GameSystem;

namespace SpaceShooter.Screens
{
    class MainMenuScreen:Screen
    {
        private List<MenuItem> items = new List<MenuItem>();
        private int currentItem = 0;
        private const float BackgroundMoveSpeed = 0.05f;
        private Texture2D _backgroundTexture;
        private float _bgDeltaX = 0;
        private SpriteFont font;

        private int colorId = 1;
        private int textureId = 1;
        private MenuItem colorChangeItem;
        private MenuItem textureChangeItem;

        private Sprite previewSprite;
        public MainMenuScreen(string name, ScreenManager screenManager) : base(name, screenManager)
        {
            items.Add(new MenuItem()
            {
                Caption = "Play",
                IsHovered = true,
                OnClicked =
                    () => { screenManager.SetActiveScreen("main"); }
            });

            colorChangeItem = new MenuItem()
            {
                Caption = "< Color: blue >",
                IsHovered = false,
                OnLeftPress =
                    () =>
                    {
                        colorId--;
                        if (colorId < 1) colorId = 4;
                        UpdateData();
                    },
                OnRightPress =
                    () =>
                    {
                        colorId++;
                        if (colorId > 4) colorId = 1;
                        UpdateData();
                    }
            };
            items.Add(colorChangeItem);

            textureChangeItem = new MenuItem()
            {
                Caption = "< Ship: 1 >",
                IsHovered = false,
                OnLeftPress =
                    () =>
                    {
                        textureId--;
                        if (textureId < 1) textureId = 3;
                        UpdateData();
                    },
                OnRightPress =
                    () =>
                    {
                        textureId++;
                        if (textureId > 3) textureId = 1;
                        UpdateData();
                    }
            };
            items.Add(textureChangeItem);

            items.Add(new MenuItem()
            {
                Caption = "Exit",
                IsHovered = false,
                OnClicked =
                    () => { screenManager.Game.Exit(); }
            });
            previewSprite = 
                new Sprite("preview", SpriteSheetCollection.Instance.Atlas, 1f, SpriteSheetCollection.Instance[Game.PlayerTextureName]);
        }

        private void UpdateData()
        {
            string color = "";
            switch (colorId)
            {
                case 1:
                    color = "blue";
                    break;
                case 2:
                    color = "green";
                    break;
                case 3:
                    color = "red";
                    break;
                case 4:
                    color = "orange";
                    break;
            }
            Game.PlayerTextureColor = color;

            Game.PlayerTextureNumber = textureId;
            colorChangeItem.Caption = $"< Color: {color} >";
            textureChangeItem.Caption = $"< Ship: {textureId} >";
            previewSprite =
                new Sprite("preview", SpriteSheetCollection.Instance.Atlas, 1f, SpriteSheetCollection.Instance[Game.PlayerTextureName]);
        }

        public override void Initialize()
        {
            // TODO
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //DRAW BACKGROUND
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            spriteBatch.Draw(_backgroundTexture, null,
                new Rectangle(-(int)_bgDeltaX, 0, Game.GraphicsDevice.Viewport.Width + (int)_bgDeltaX, Game.GraphicsDevice.Viewport.Height),
                new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width + (int)_bgDeltaX, Game.GraphicsDevice.Viewport.Height)
                );
            spriteBatch.End();
            //END BACKGROUND

            int dy = 50;
            int i = 0;
            spriteBatch.Begin();
            foreach (var item in items)
            {
                if(i == currentItem) spriteBatch.DrawString(font, item.Caption, new Vector2(200, 300 + i * dy), Color.Red);
                else spriteBatch.DrawString(font, item.Caption, new Vector2(200, 300+i*dy), Color.Green );
                i++;
            }
            previewSprite.Draw(new Vector2(500, 300), spriteBatch,gameTime );
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            _bgDeltaX = _bgDeltaX + gameTime.ElapsedGameTime.Milliseconds * BackgroundMoveSpeed;
            if (_bgDeltaX <= -256) _bgDeltaX = 0;
            if (InputSingletone.Instance.KeyboardState.IsKeyDown(Keys.Escape)) Game.Exit();

            if (InputSingletone.Instance.IsKeyPress(Keys.Down)) currentItem += 1;
            if (InputSingletone.Instance.IsKeyPress(Keys.Up)) currentItem -= 1;
            if (InputSingletone.Instance.IsKeyPress(Keys.Enter)) items[currentItem].OnClicked();
            if (InputSingletone.Instance.IsKeyPress(Keys.Right)) items[currentItem].OnRightPress();
            if (InputSingletone.Instance.IsKeyPress(Keys.Left)) items[currentItem].OnLeftPress();
            if (currentItem < 0) currentItem = items.Count - 1;
            currentItem = currentItem % (items.Count);


        }

        public override void LoadContent(ContentManager contentManager)
        {
            _backgroundTexture = contentManager.Load<Texture2D>("textures/background/darkPurple");
            font = contentManager.Load<SpriteFont>("fonts/Calibri16");
        }
    }

    class MenuItem
    {
        public string Caption { get; set; }
        public bool IsHovered { get; set; }
        public Action OnClicked { get; set; }
        public Action OnRightPress { get; set; }
        public Action OnLeftPress { get; set; }

        public MenuItem()
        {
            OnClicked = () => { };
            OnLeftPress = () => { };
            OnRightPress = () => { };
        }
    }

    
}
