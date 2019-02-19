using Engine;
using IsoMap;
using IsoMap.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace Engine
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
            Player.Instance.currentCharacter.mapRepresentation.Load();
            //TODO appel à une méthode factory qui créera un élément avec animated sprite et coord déplaçables

            base.Load();
                       
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void Update(GameTime gameTime, float deltaTime)
        {
            base.Update(gameTime, deltaTime);
            List<InputType> playerInputs = Input.DefineInputs(ref mainGame.gameState.oldKbState);
            Player.Instance.currentCharacter.mapRepresentation.Update(playerInputs , deltaTime);


            snowMap.Update();
        }

        protected void DrawSceneToTexture(RenderTarget2D renderTarget, GameTime gameTime)
        {
            // Set the render target
            mainGame.GraphicsDevice.SetRenderTarget(renderTarget);

            mainGame.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true }; //G PA KONPRI

            // Draw the scene
            //__________________CONTENU DU DRAW "CLASSIQUE"_____________

            mainGame.spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null); //SamplerState.PointClamp => Permet de resize du pixel art sans blur
            Tools.DrawTiled(mainGame.spriteBatch, isometricGrid, 13, 19, new Vector2(0,-1));

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
