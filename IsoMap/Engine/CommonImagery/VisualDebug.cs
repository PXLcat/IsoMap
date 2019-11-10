using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.CommonImagery
{
    public static class VisualDebug //classe outil
    {
        public static void DrawPosition(SpriteBatch spritebatch, Texture2D textureDebug, Vector2 currentPosition)
        {
            spritebatch.Draw(textureDebug, new Rectangle((int)currentPosition.X - 2, (int)currentPosition.Y - 2, 4, 4), Color.White * 0.5f);
        }


        // from David Gouveia on gamedev.stackexchange
        public static void DrawLine(this SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 end, float opacity)
        {
            spriteBatch.Draw(texture, start, null, Color.White * opacity,
                             (float)Math.Atan2(end.Y - start.Y, end.X - start.X),
                             new Vector2(0f, (float)texture.Height / 2),
                             new Vector2(Vector2.Distance(start, end), 1f),
                             SpriteEffects.None, 0f);
        }

        public static void DrawSATHitbox(SpriteBatch spriteBatch, Texture2D segTexture, Polygon CurrentHitBox)
        {
            for (int i = 0; i < CurrentHitBox.Points.Count - 1; i++)
            {
                DrawLine(spriteBatch, segTexture,
                    new Vector2(CurrentHitBox.Points.ElementAt(i).X, CurrentHitBox.Points.ElementAt(i).Y),
                    new Vector2(CurrentHitBox.Points.ElementAt(i + 1).X, CurrentHitBox.Points.ElementAt(i + 1).Y), 0.5f);
                //TODO à terme il faudrait une seule classe qui regroupe Vector2 et Vector
            }
            DrawLine(spriteBatch, segTexture,
                    new Vector2(CurrentHitBox.Points.ElementAt(CurrentHitBox.Points.Count - 1).X, CurrentHitBox.Points.ElementAt(CurrentHitBox.Points.Count - 1).Y),
                    new Vector2(CurrentHitBox.Points.ElementAt(0).X, CurrentHitBox.Points.ElementAt(0).Y), 0.5f);

        }

    }
}
