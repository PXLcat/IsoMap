﻿using Engine;
using Engine.CommonImagery;
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
using static Engine.CommonImagery.SATCollision;

namespace Engine
{
    public class MapScene : Scene
    {
        Texture2D isometricGrid;
        

        IsometricMap snowMap;
        PolygonCollisionResult collision;

        CharacterClasses.MapRepresentation legisteTest;

        public MapScene(MainGame mG) : base(mG)
        {

        }
        public override void Load()
        {
            mainGame.IsMouseVisible = true;
            isometricGrid = mainGame.Content.Load<Texture2D>("isogrid");

            snowMap = new IsometricMap(); // pas super
            snowMap.Load(mainGame.Content);

            zoom = 2; //C'est ici qu'on définit réellement le zoom
            

            //Player : 

            Player.Instance.Load(mainGame);
            Factory.Instance.LoadPlayer(); //ça charge le json avec les données des persos du joueur
            Player.Instance.currentCharacter.mapRepresentation.Load(); //ça initialise le perso
            //test__
            Player.Instance.currentCharacter.mapRepresentation.HitboxShape = new CommonImagery.Polygon();
            Player.Instance.currentCharacter.mapRepresentation.HitboxShape.Points.Add(new CommonImagery.Vector(0, 2));
            Player.Instance.currentCharacter.mapRepresentation.HitboxShape.Points.Add(new CommonImagery.Vector(Player.Instance.currentCharacter.mapRepresentation.HitboxShape.Points.ElementAt(0).X - 9, Player.Instance.currentCharacter.mapRepresentation.HitboxShape.Points.ElementAt(0).Y - 6));
            Player.Instance.currentCharacter.mapRepresentation.HitboxShape.Points.Add(new CommonImagery.Vector(Player.Instance.currentCharacter.mapRepresentation.HitboxShape.Points.ElementAt(0).X, Player.Instance.currentCharacter.mapRepresentation.HitboxShape.Points.ElementAt(0).Y - 12));
            Player.Instance.currentCharacter.mapRepresentation.HitboxShape.Points.Add(new CommonImagery.Vector(Player.Instance.currentCharacter.mapRepresentation.HitboxShape.Points.ElementAt(0).X + 9, Player.Instance.currentCharacter.mapRepresentation.HitboxShape.Points.ElementAt(0).Y - 6));
            //test__
            legisteTest = Factory.Instance.LoadTestCharacter();
            legisteTest.HitboxShape = new CommonImagery.Polygon();
            legisteTest.HitboxShape.Points.Add(new CommonImagery.Vector(0, 2));
            legisteTest.HitboxShape.Points.Add(new CommonImagery.Vector(legisteTest.HitboxShape.Points.ElementAt(0).X-9, legisteTest.HitboxShape.Points.ElementAt(0).Y-6));
            legisteTest.HitboxShape.Points.Add(new CommonImagery.Vector(legisteTest.HitboxShape.Points.ElementAt(0).X, legisteTest.HitboxShape.Points.ElementAt(0).Y - 12));
            legisteTest.HitboxShape.Points.Add(new CommonImagery.Vector(legisteTest.HitboxShape.Points.ElementAt(0).X + 9, legisteTest.HitboxShape.Points.ElementAt(0).Y - 6));

            legisteTest.UpdateZOrder();
            base.Load();
                       
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void Update(GameTime gameTime, float deltaTime)
        {
            base.Update(gameTime, deltaTime);
            //List<InputType> playerInputs = Input.DefineInputs(ref mainGame.gameState.oldKbState); attention, en double avec le Scene
            Player.Instance.currentCharacter.mapRepresentation.Update(playerInputs , deltaTime);
            collision = SATCollision.CheckPolygonCollision(Player.Instance.currentCharacter.mapRepresentation.CurrentHitBox, legisteTest.CurrentHitBox, new Vector(0,0));

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

            legisteTest.Draw(mainGame.spriteBatch);

            base.Draw(gameTime);

            if (collision.Intersect)
            {
                mainGame.spriteBatch.DrawString(Fonts.Instance.kenPixel16, "collision", new Vector2(200,100), Color.Red);
            }

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
