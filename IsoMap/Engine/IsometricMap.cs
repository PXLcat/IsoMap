using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using Microsoft.Xna.Framework.Content;

namespace IsoMap.Engine
{
    public class IsometricMap
    {
        TmxMap snowMap;
        Dictionary<string, Texture2D> tilesetsTextures;

        Point originTileCoord; //à utiliser pour les Tilesets 32x16
        Point originBlockCoord; //à utiliser pour les Tilesets 32x32

        public void Load(ContentManager contentManager)
        {
            snowMap = new TmxMap("Content/testiso.tmx");
            tilesetsTextures = new Dictionary<string, Texture2D>();
            tilesetsTextures.Add("grassTileset", contentManager.Load<Texture2D>(snowMap.Tilesets[0].Name));//se référer à l'ordre dans le xml
            tilesetsTextures.Add("decorNeigeTileset", contentManager.Load<Texture2D>(snowMap.Tilesets[1].Name));//TODO générer par Factory

            originTileCoord = new Point(snowMap.Tilesets[0].TileWidth * (snowMap.Width / 4) - snowMap.Tilesets[0].TileWidth / 2, 0);
            originBlockCoord = new Point(snowMap.Tilesets[0].TileWidth * (snowMap.Width / 2) - snowMap.Tilesets[0].TileWidth / 2,
                -snowMap.Tilesets[0].TileHeight); //attention tout se base sur les dimensions du premier tileset

        }
        public void Update()
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < snowMap.Layers.Count; i++)// Pour chaque layer
            {
                //snowMap.Layers[i]
                for (int y = 0; y < snowMap.Layers[i].Tiles.Count; y++) // Pour chaque tile
                {
                    if (snowMap.Layers[i].Tiles[y].Gid != 0)
                    {
                        if (snowMap.Layers[i].Tiles[y].Gid < snowMap.Tilesets[1].FirstGid) //Correspond au 1er tileset //TODO généraliser les noms de variable et nombres de layer
                        {
                            spriteBatch.Draw(tilesetsTextures.Values.ElementAt(0), new Rectangle(originTileCoord
                                , new Point(snowMap.Tilesets[0].TileWidth, snowMap.Tilesets[0].TileHeight))
                                , new Rectangle((snowMap.Layers[i].Tiles[y].Gid - 1) % snowMap.Tilesets[0].Columns.Value * snowMap.Tilesets[0].TileWidth
                                , (int)Math.Floor((double)(snowMap.Layers[i].Tiles[y].Gid/ snowMap.Tilesets[0].Columns.Value) * snowMap.Tilesets[0].TileHeight)
                                //, Math.Floor((double)(2/1))
                                , snowMap.Tilesets[0].TileWidth
                                , snowMap.Tilesets[0].TileHeight)
                                , Color.White);

                        }
                        // else if (tile.Gid < snowMap.Tilesets[x].FirstGid) si il y a d'autres layers
                        else
                        {

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