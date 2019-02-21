using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace Engine
{
    public class Tools
    {
        /// <summary>
        /// Dessine un motif répété
        /// </summary>
        /// <param name="sb">Le spritebatch</param>
        /// <param name="texture">Le motif à dessiner</param>
        /// <param name="horizontalTilesNb">Le nombre de fois où le motif se répète horizontalement</param>
        /// <param name="verticalTilesNb">Le nombre de fois où le motif se répète verticalement</param>
        /// <param name="initalPosition">Le coin haut gauche du motif</param>
        /// <param name="horizontalFlip">Initialement à false. Applique un effet "miroir"</param>
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

        /// <summary>
        /// Renvoie la position du coin bas gauche d'une tile à partir de sa coordonnée carthésienne.
        /// </summary>
        /// <param name="map">La map avec ses tailles de tiles</param>
        /// <param name="coordToTranslate">Les coordonnées du point</param>
        /// <param name="origin">L'origine de dessin de la map</param>
        /// <returns>Un point avec les coordonnées X et Y correspondant au bas gauche de la zone de la tile</returns>
        public static Point CarthesianToIsometricTile(TmxMap map, Point coordToTranslate, Point origin) //TODO vérif que cette méthode est bonne
        {
            Point upLeftCorner = new Point(origin.X + coordToTranslate.X * (map.TileWidth / 2) - coordToTranslate.Y * (map.TileWidth / 2)
                , origin.Y + coordToTranslate.Y * (map.TileHeight / 2) + coordToTranslate.X * (map.TileHeight / 2));
            Point downLeftCorner = upLeftCorner + new Point(0, map.TileHeight);
            return downLeftCorner;
        }

        /// <summary>
        /// ATTENTION CETTE METHODE NE CALCULE PAS LES POSITIONS INFERIEURES A SON ORIGINE
        /// //TODO
        /// </summary>
        /// <param name="map"></param>
        /// <param name="coordToTranslate"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static Vector2 IsometricToCarthesian(TmxMap map, Point coordToTranslate, Point origin)
        {
            Point originAtMiddle = new Point(origin.X + map.TileWidth / 2, origin.Y);//l'angle haut de 0,0 . Sinon ça prend le haut gauche du rectangle contenant l'origine

            //int cartX = ((coordToTranslate.X - originAtMiddle.X) / (map.TileWidth/2) + (coordToTranslate.Y - originAtMiddle.Y) / (map.TileWidth / 2))/2;
            int cartX = ((coordToTranslate.Y - originAtMiddle.Y) *2 + (coordToTranslate.X - originAtMiddle.X)) / map.TileWidth;
            //int cartY = ((coordToTranslate.Y - originAtMiddle.Y) / (map.TileHeight / 2) - (coordToTranslate.X - originAtMiddle.X) / (map.TileHeight / 2)) / 2;
            int cartY = ((coordToTranslate.Y - originAtMiddle.Y) *2 - (coordToTranslate.X - originAtMiddle.X)) / map.TileWidth;
            //malgré les floats, pas de virgule?
            return new Vector2(cartX, cartY);
        }
    }
}
