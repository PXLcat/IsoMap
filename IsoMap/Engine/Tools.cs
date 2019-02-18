using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Tools
    {
        public static void DrawTiled(SpriteBatch sb, Texture2D texture, int horizontalTilesNb, 
            int verticalTilesNb, Vector2 initalPosition, bool horizontalFlip = false)
        {
            for (int i = 0; i < verticalTilesNb; i++)
            {
                for (int y = 0; y < horizontalTilesNb; y++)
                {
                    //n'utilise pas le centre, va se caler sur le point en haut à gauche
                    sb.Draw(texture, new Rectangle((int)initalPosition.X + y * texture.Width,
                        (int)initalPosition.Y + i * texture.Height, texture.Width, texture.Height), Color.White);
                }
            }


        }
    }
}
