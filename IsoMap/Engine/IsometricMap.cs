using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using IsoMap.Engine.Tiles;

namespace IsoMap.Engine
{
    public class IsometricMap
    {
        TmxMap snowMap;
        Dictionary<string, Texture2D> tilesetsTextures;

        Point originTileCoord; //à utiliser pour les Tilesets 32x16
        Point originBlockCoord; //à utiliser pour les Tilesets 32x32
        int tileOrBlockWidth;
        Point tileSize;
        Point blockSize;

        List<ModelTile> mapElements;

        // TODO : faire une fonction pour déterminer si un tileset a des tiles "plates" ou "block" et utiliser pour savoir quoi charger dans le Laod

        public void Load(ContentManager contentManager)
        {
            snowMap = new TmxMap("Content/testiso.tmx");
            tilesetsTextures = new Dictionary<string, Texture2D>
            {
                { "grassTileset", contentManager.Load<Texture2D>(snowMap.Tilesets[0].Name) },//se référer à l'ordre dans le xml
                { "decorNeigeTileset", contentManager.Load<Texture2D>(snowMap.Tilesets[1].Name) }//TODO générer par Factory
            };

            tileOrBlockWidth = snowMap.Tilesets[0].TileWidth;
            tileSize = new Point(snowMap.Tilesets[0].TileWidth, snowMap.Tilesets[0].TileHeight);
            blockSize = new Point(snowMap.Tilesets[1].TileWidth, snowMap.Tilesets[1].TileHeight);

            //originTileCoord = new Point(snowMap.Tilesets[0].TileWidth * (snowMap.Width / 4) - snowMap.Tilesets[0].TileWidth / 2, 0);
            originTileCoord = new Point(160, 0); //à suppr
            originBlockCoord = new Point(snowMap.Tilesets[0].TileWidth * (snowMap.Width / 2) - snowMap.Tilesets[0].TileWidth / 2,
                -snowMap.Tilesets[0].TileHeight); //attention tout se base sur les dimensions du premier tileset

            FillMapElements();

        }
        public void FillMapElements()
        {
            mapElements = new List<ModelTile>();

            for (int i = 0; i < snowMap.Layers.Count; i++)// Pour chaque layer
            {
                int orthogonalX = 0;
                int orthogonalY = 0;

                string layerNameZ = snowMap.Layers[i].Name.Substring(0, 2);


                ///A "true" si le nom du layer commence bien par deux chifres. layerZ représente la hauteur en blocs du layer
                bool correctLayerName = Int32.TryParse(layerNameZ, out int layerZ);
                if (!correctLayerName)
                {
                    throw new Exception("Erreur dans le nommage du layer " + snowMap.Layers[i].Name +
                        ". Le nom doit commencer par deux chiffres indiquant la hauteur du layer.");
                }
                Debug.WriteLine(snowMap.Layers[i].Name + " " + layerZ);


                ////snowMap.Layers[i]
                //for (int y = 0; y < snowMap.Layers[i].Tiles.Count; y++) // Pour chaque tile
                //{

                //    if (snowMap.Layers[i].Tiles[y].Gid != 0)
                //    {
                //        if (snowMap.Layers[i].Tiles[y].Gid < snowMap.Tilesets[1].FirstGid) //Correspond au 1er tileset //TODO généraliser les noms de variable et nombres de layer
                //        {
                //            spriteBatch.Draw(tilesetsTextures.Values.ElementAt(0)
                //                //destinationRectangle :
                //                , new Rectangle(CarthesianToIsometric(new Point(orthogonalX, orthogonalY), originTileCoord)
                //                , tileSize)
                //                //sourceRectangle :
                //                , new Rectangle(new Point((snowMap.Layers[i].Tiles[y].Gid - 1) % snowMap.Tilesets[0].Columns.Value * snowMap.Tilesets[0].TileWidth
                //                , (int)Math.Floor((double)(snowMap.Layers[i].Tiles[y].Gid / snowMap.Tilesets[0].Columns.Value) * snowMap.Tilesets[0].TileHeight))
                //                , tileSize)
                //                , Color.White, 0f
                //                //origin :
                //                , new Vector2(0, snowMap.Tilesets[0].TileHeight) //dessin à l'origine bas gauche, peu importe la hauteur
                //                , SpriteEffects.None, 0f);
                //        }
                //        // else if (tile.Gid < snowMap.Tilesets[x].FirstGid) si il y a d'autres layers
                //        else
                //        { //Attention Tileset "blocs"
                //            spriteBatch.Draw(tilesetsTextures.Values.ElementAt(1) //attention redondance, y'a que cet arg qui change
                //                                                                  //destinationRectangle :
                //                , new Rectangle(CarthesianToIsometric(new Point(orthogonalX - 1, orthogonalY - 1), originTileCoord) //PK ça marche avec -1 aux coord??
                //                , blockSize)
                //                //sourceRectangle :

                //                , new Rectangle(((snowMap.Layers[i].Tiles[y].Gid - snowMap.Tilesets[1].FirstGid) % snowMap.Tilesets[1].Columns.Value) * snowMap.Tilesets[1].TileWidth
                //                , (int)Math.Floor((double)((snowMap.Layers[i].Tiles[y].Gid - snowMap.Tilesets[1].FirstGid) / snowMap.Tilesets[1].Columns.Value) * snowMap.Tilesets[1].TileHeight) //Attention "Tilesets[1]"

                //                , snowMap.Tilesets[1].TileWidth
                //                , snowMap.Tilesets[1].TileHeight)

                //                /*
                //                , new Rectangle(226, 0, 32, 32)
                //                */
                //                , Color.White, 0f
                //                //origin :
                //                , new Vector2(0, snowMap.Tilesets[0].TileHeight) //dessin à l'origine bas gauche, peu importe la hauteur
                //                , SpriteEffects.None, 1f);
                //        }
                //    }
                //    orthogonalX++;
                //    if (orthogonalX >= snowMap.Width) //en théorie le = devrait suffire
                //    {
                //        orthogonalX = 0;
                //        orthogonalY++;
                //        if (orthogonalY > snowMap.Height)
                //        {
                //            Debug.Write("fin de la map");
                //        }

                //    }
                //}
            }
        }


        public void Update()
        {

        }

        public enum TileStyle
        {
            FLAT,
            BLOCK
        }
        public TileStyle GetTileStyle(TmxTileset tileset)
        {
            TileStyle result;
            if (tileset.TileHeight == tileset.TileWidth / 2)
            {
                result = TileStyle.FLAT;
            }
            else
            {
                result = TileStyle.BLOCK;
            }
            return result;
        }

        public Point CarthesianToIsometric(Point coordToTranslate, Point origin)
        {
            Point upLeftCorner = new Point(origin.X + coordToTranslate.X * (snowMap.TileWidth / 2) - coordToTranslate.Y * (snowMap.TileWidth / 2)
                , origin.Y + coordToTranslate.Y * (snowMap.TileHeight / 2) + coordToTranslate.X * (snowMap.TileHeight / 2));
            Point downLeftCorner = upLeftCorner + new Point(0, snowMap.TileHeight);
            return downLeftCorner;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < snowMap.Layers.Count; i++)// Pour chaque layer
            {
                int orthogonalX = 0;
                int orthogonalY = 0;

                //snowMap.Layers[i]
                for (int y = 0; y < snowMap.Layers[i].Tiles.Count; y++) // Pour chaque tile
                {
                    if (snowMap.Layers[i].Tiles[y].Gid != 0)
                    {
                        for (int ts = 0; ts < snowMap.Tilesets.Count; ts++)
                        {
                            if ((snowMap.Layers[i].Tiles[y].Gid >= snowMap.Tilesets[ts].FirstGid)
                                &&(snowMap.Layers[i].Tiles[y].Gid < snowMap.Tilesets[ts].FirstGid+snowMap.Tilesets[ts].TileCount))
                            {

                            }
                            //else ce Gid ne fait pas partie de ce tileset

                        }

                        if (snowMap.Layers[i].Tiles[y].Gid < snowMap.Tilesets[1].FirstGid) //Correspond au 1er tileset //TODO généraliser les noms de variable et nombres de layer
                        {
                            spriteBatch.Draw(tilesetsTextures.Values.ElementAt(0)
                                //destinationRectangle :
                                , new Rectangle(CarthesianToIsometric(new Point(orthogonalX, orthogonalY), originTileCoord)
                                , new Point(snowMap.Tilesets[0].TileWidth, snowMap.Tilesets[0].TileHeight))
                                //sourceRectangle :
                                , new Rectangle(new Point((snowMap.Layers[i].Tiles[y].Gid - 1) % snowMap.Tilesets[0].Columns.Value * snowMap.Tilesets[0].TileWidth
                                , (int)Math.Floor((double)(snowMap.Layers[i].Tiles[y].Gid / snowMap.Tilesets[0].Columns.Value) * snowMap.Tilesets[0].TileHeight))
                                , tileSize)
                                , Color.White, 0f
                                //origin :
                                , new Vector2(0, snowMap.Tilesets[0].TileHeight) //dessin à l'origine bas gauche, peu importe la hauteur
                                , SpriteEffects.None, 0f);
                        }
                        // else if (tile.Gid < snowMap.Tilesets[x].FirstGid) si il y a d'autres layers
                        else
                        { //Attention Tileset "blocs"
                            spriteBatch.Draw(tilesetsTextures.Values.ElementAt(1) //attention redondance, y'a que cet arg qui change
                                                                                  //destinationRectangle :
                                , new Rectangle(CarthesianToIsometric(new Point(orthogonalX - 1, orthogonalY - 1), originTileCoord) //PK ça marche avec -1 aux coord??
                                , blockSize)
                                //sourceRectangle :

                                , new Rectangle(((snowMap.Layers[i].Tiles[y].Gid - snowMap.Tilesets[1].FirstGid) % snowMap.Tilesets[1].Columns.Value) * snowMap.Tilesets[1].TileWidth
                                , (int)Math.Floor((double)((snowMap.Layers[i].Tiles[y].Gid - snowMap.Tilesets[1].FirstGid) / snowMap.Tilesets[1].Columns.Value) * snowMap.Tilesets[1].TileHeight) //Attention "Tilesets[1]"

                                , snowMap.Tilesets[1].TileWidth
                                , snowMap.Tilesets[1].TileHeight)

                                /*
                                , new Rectangle(226, 0, 32, 32)
                                */
                                , Color.White, 0f
                                //origin :
                                , new Vector2(0, snowMap.Tilesets[0].TileHeight) //dessin à l'origine bas gauche, peu importe la hauteur
                                , SpriteEffects.None, 1f);
                        }
                    }
                    orthogonalX++;
                    if (orthogonalX >= snowMap.Width) //en théorie le = devrait suffire
                    {
                        orthogonalX = 0;
                        orthogonalY++;
                        if (orthogonalY > snowMap.Height)
                        {
                            Debug.Write("fin de la map");
                        }

                    }
                }
            }

        }


        public void Unload()
        {

        }
    }
}