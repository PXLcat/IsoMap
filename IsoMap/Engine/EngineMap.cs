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
        Dictionary<string,Texture2D> tilesetsTextures;
        
        Vector2 originTileCoord; //à utiliser pour les Tilesets 32x16
        Vector2 originBlockCoord; //à utiliser pour les Tilesets 32x32

        public void Load(ContentManager contentManager)
        {
            snowMap = new TmxMap("Content/testiso.tmx");
            tilesetsTextures = new Dictionary<string, Texture2D>();
            tilesetsTextures.Add("grassTileset", contentManager.Load<Texture2D>(snowMap.Tilesets[0].Name));//se référer à l'ordre dans le xml
            tilesetsTextures.Add("decorNeigeTileset", contentManager.Load<Texture2D>(snowMap.Tilesets[1].Name));//TODO générer par Factory

            originTileCoord = new Vector2(snowMap.Tilesets[0].TileWidth * snowMap.Width - snowMap.Tilesets[0].TileWidth / 2,0);
            originBlockCoord = new Vector2(snowMap.Tilesets[0].TileWidth * snowMap.Width - snowMap.Tilesets[0].TileWidth / 2, 
                -snowMap.Tilesets[0].TileHeight); //attention tout se base sur les dimensions du premier tileset

        }
        public void Update()
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < snowMap.Layers.Count; i++)
            {
                //snowMap.Layers[i]
                for (int y = 0; y < snowMap.Layers[i].Tiles.Count; y++)
                {
                    foreach (TmxLayerTile tile in layer.Tiles)
                    {
                        if (tile.Gid != 0)
                        {
                            if (tile.Gid < snowMap.Tilesets[0].FirstGid) //Correspond au 1er tileset //TODO généraliser les noms de variable et nombres de layer
                            {
                                //spriteBatch.Draw(tilesetsTextures[0],)
                            }
                            // else if (tile.Gid < snowMap.Tilesets[x].FirstGid) si il y a d'autres layers
                            else
                            {

                            }
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
