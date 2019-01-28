using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace IsoMap.Engine
{
    public class MapScene : Scene
    {
        Texture2D isometricGrid;
        

        IsometricMap snowMap;

        public MapScene(MainGame mG) : base(mG)
        {

        }
        public override void Load()
        {
            mainGame.IsMouseVisible = true;
            isometricGrid = mainGame.Content.Load<Texture2D>("isogrid");

            snowMap = new IsometricMap(); // pas super
            snowMap.Load(mainGame.Content);

            zoom = 2;


            //Player : 

            Player.Instance.Load(mainGame);
            Factory.Instance.LoadPlayer();
            //TODO appel à une méthode factory qui créera un élément avec animated sprite et coord déplaçables

            base.Load();
                       
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            snowMap.Update();
            snowMap.Update();
        }

        protected void DrawSceneToTexture(RenderTarget2D renderTarget, GameTime gameTime)
        {
            // Set the render target
            mainGame.GraphicsDevice.SetRenderTarget(renderTarget);

            mainGame.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true }; //G PA KONPRI

            // Draw the scene
            //mainGame.GraphicsDevice.Clear(Color.CornflowerBlue); probablement pas utile?
            //__________________CONTENU DU DRAW "CLASSIQUE"_____________

            mainGame.spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null); //SamplerState.PointClamp => Permet de resize du pixel art sans blur
            Tools.DrawTiled(mainGame.spriteBatch, isometricGrid, 13, 19, Vector2.Zero);

            snowMap.Draw(mainGame.spriteBatch);
            Player.Instance.currentCharacter.mapRepresentation.Draw(mainGame.spriteBatch);

            base.Draw(gameTime);

            mainGame.spriteBatch.End();
            //__________________________________________________________

            // Drop the render target
            mainGame.GraphicsDevice.SetRenderTarget(null);
        }

        public override void Draw(GameTime gameTime)
        {
            DrawSceneToTexture(renderTarget, gameTime);

            mainGame.GraphicsDevice.Clear(Color.Black);

            mainGame.spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null); //SamplerState.PointClamp => Permet de resize du pixel art sans blur

            mainGame.spriteBatch.Draw(renderTarget, new Rectangle(0, 0, 800, 600), Color.White);

            mainGame.spriteBatch.End();


        }
    }
}
