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

        // TODO : faire une fonction pour déterminer si un tileset a des tiles "plates" ou "block" et utiliser pour savoir quoi charger dans le Laod

        public void Load(ContentManager contentManager)
        {
            snowMap = new TmxMap("Content/testiso.tmx");
            tilesetsTextures = new Dictionary<string, Texture2D>();
            tilesetsTextures.Add("grassTileset", contentManager.Load<Texture2D>(snowMap.Tilesets[0].Name));//se référer à l'ordre dans le xml
            tilesetsTextures.Add("decorNeigeTileset", contentManager.Load<Texture2D>(snowMap.Tilesets[1].Name));//TODO générer par Factory

            tileOrBlockWidth = snowMap.Tilesets[0].TileWidth;
            tileSize = new Point(snowMap.Tilesets[0].TileWidth, snowMap.Tilesets[0].TileHeight);
            blockSize = new Point(snowMap.Tilesets[1].TileWidth, snowMap.Tilesets[1].TileHeight);

            //originTileCoord = new Point(snowMap.Tilesets[0].TileWidth * (snowMap.Width / 4) - snowMap.Tilesets[0].TileWidth / 2, 0);
            originTileCoord = new Point(160, 0); //à suppr
            originBlockCoord = new Point(snowMap.Tilesets[0].TileWidth * (snowMap.Width / 2) - snowMap.Tilesets[0].TileWidth / 2,
                -snowMap.Tilesets[0].TileHeight); //attention tout se base sur les dimensions du premier tileset

        }
        public void Update()
        {

        }

        public enum TileStyle
        {
            FLAT,
            BLOCK
        }
        public TileStyle getTileStyle(TmxTileset tileset)
        {
            TileStyle result;
            if (tileset.TileHeight == tileset.TileWidth/2)
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
                , origin.Y + coordToTranslate.Y*(snowMap.TileHeight/2) + coordToTranslate.X*(snowMap.TileHeight/2));
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
                        if (snowMap.Layers[i].Tiles[y].Gid < snowMap.Tilesets[1].FirstGid) //Correspond au 1er tileset //TODO généraliser les noms de variable et nombres de layer
                        {
                            spriteBatch.Draw(tilesetsTextures.Values.ElementAt(0)
                                //destinationRectangle :
                                , new Rectangle(new Point(originTileCoord.X + (snowMap.Tilesets[0].TileWidth / 2) * orthogonalX - (orthogonalY * (snowMap.Tilesets[0].TileWidth / 2))
                                    , originTileCoord.Y + ((snowMap.Tilesets[0].TileHeight / 2) * orthogonalX) + (orthogonalY * (snowMap.Tilesets[0].TileHeight / 2)))
                                , new Point(snowMap.Tilesets[0].TileWidth, snowMap.Tilesets[0].TileHeight))
                                //sourceRectangle :
                                , new Rectangle((snowMap.Layers[i].Tiles[y].Gid - 1) % snowMap.Tilesets[0].Columns.Value * snowMap.Tilesets[0].TileWidth
                                , (int)Math.Floor((double)(snowMap.Layers[i].Tiles[y].Gid / snowMap.Tilesets[0].Columns.Value) * snowMap.Tilesets[0].TileHeight)
                                , snowMap.Tilesets[0].TileWidth
                                , snowMap.Tilesets[0].TileHeight)
                                , Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                        }
                        // else if (tile.Gid < snowMap.Tilesets[x].FirstGid) si il y a d'autres layers
                        else
                        { //Attention Tileset "blocs"
                            spriteBatch.Draw(tilesetsTextures.Values.ElementAt(1) //attention redondance, y'a que cet arg qui change
                                                                                  //destinationRectangle :
                                , new Rectangle(new Point(originTileCoord.X + (snowMap.Tilesets[1].TileWidth / 2) * orthogonalX - (orthogonalY * (snowMap.Tilesets[1].TileWidth / 2))
                                    , originTileCoord.Y + ((snowMap.Tilesets[1].TileHeight / 2) * orthogonalX) + (orthogonalY * (snowMap.Tilesets[1].TileHeight / 2)))
                                , new Point(snowMap.Tilesets[1].TileWidth, snowMap.Tilesets[1].TileHeight))
                                //sourceRectangle :
                                /*
                                , new Rectangle((snowMap.Layers[i].Tiles[y].Gid- snowMap.Tilesets[1].FirstGid - 1) % snowMap.Tilesets[1].Columns.Value * snowMap.Tilesets[1].TileWidth
                                , (int)Math.Floor((double)(snowMap.Layers[i].Tiles[y].Gid - snowMap.Tilesets[1].FirstGid / snowMap.Tilesets[0].Columns.Value) * snowMap.Tilesets[1].TileHeight)
                                , snowMap.Tilesets[1].TileWidth
                                , snowMap.Tilesets[1].TileHeight)
                                */
                                , new Rectangle(226, 0, 32, 32)
                                , Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
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