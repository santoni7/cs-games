using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameSystem;

namespace SpaceShooter.GameSystem
{
    /// <summary>
    /// Singletone class
    /// </summary>
    public class SpriteSheetCollection :Dictionary<String, Rectangle>
    {
        private static SpriteSheetCollection _instance;
        public static SpriteSheetCollection Instance => _instance ?? (_instance = new SpriteSheetCollection());

        public Texture2D Atlas;

        public Texture2D CircleTexture;
        public Texture2D PointTexture;

        public SpriteSheetCollection()
        {
            TextureAtlasXml textureAtlas = TextureAtlasXml.FromStream(TitleContainer.OpenStream("Content\\sheet.xml"));
            textureAtlas.Sprites.ForEach(x=>this.Add(x.Name,x.Rectangle));
        }

        public void LoadContent(ContentManager content)
        {
            Atlas = content.Load<Texture2D>("textures/sheet");

            CircleTexture = content.Load<Texture2D>("textures/circle");
            PointTexture = content.Load<Texture2D>("textures/point");

        }
    }
}
