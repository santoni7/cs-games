using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace SpaceShooter.GameSystem
{
    [XmlRoot("TextureAtlas", IsNullable = false)]
    public class TextureAtlasXml
    {
        public static TextureAtlasXml FromFile(String file)
        {
            using (var stream = File.OpenRead(file))
            {
                return FromStream(stream);
            }
        }

        public static TextureAtlasXml FromStream(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(TextureAtlasXml));
            return (TextureAtlasXml)serializer.Deserialize(stream);
        }


        [XmlAttribute("imagePath")]
        public String ImagePath;
        

        [XmlElement("SubTexture")]
        public List<SpriteXml> Sprites;
    }

    public class SpriteXml
    {
        [XmlAttribute("name")]
        public String Name;

        [XmlAttribute("x")]
        public Int32 X;

        [XmlAttribute("y")]
        public Int32 Y;

        [XmlAttribute("width")]
        public Int32 Width;

        [XmlAttribute("height")]
        public Int32 Height;

        public Rectangle Rectangle { get { return new Rectangle(this.X, this.Y, this.Width, this.Height); } }
    }
}
