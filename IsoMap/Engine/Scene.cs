﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IsoMap.Engine.Gamestate;

namespace IsoMap.Engine
{
    abstract public class Scene
    {
        protected MainGame mainGame;

        public int windowWidth;
        public int windowHeight;
        //public Player player;
        public Point cursorPosition; //plutôt dans le gamestate?

        protected List<InputType> playerInputs;

        public int zoom = 1;
        public RenderTarget2D renderTarget;

        //____Affichage de la position de la souris____
        private MouseState mouse;
        public String mouseText;
        public Vector2 mouseTextPos;
        //_____________________________________________

        public Scene(MainGame mG)
        {
            mainGame = mG;
            if (null == Factory.Instance.mG) //moche
            {
                Factory.Instance.SetMainGame(mG);
            }
            Factory.Instance.Load();
            
        }

        public virtual void Load()
        {
            renderTarget = new RenderTarget2D(mainGame.GraphicsDevice, 800/zoom, 600/zoom);//pas sûr de la taille à mettre (doublon de dans le Draw() )

            //windowWidth = mainGame.GraphicsDevice.DisplayMode.Width; //Attention, c'est la taille de l'écran, pas de la fenêtre
            //windowHeight = mainGame.GraphicsDevice.DisplayMode.Height;

            //player = Player.Instance;

            windowWidth = mainGame.GraphicsDevice.Viewport.Bounds.Width;
            windowHeight = mainGame.GraphicsDevice.Viewport.Bounds.Height;
            Debug.WriteLine("Window width = " + windowWidth + ", window height = " + windowHeight);

        }

        public virtual void Unload()
        {
            Debug.WriteLine("Unload " + this.GetType().Name);
        }

        public virtual void Update(GameTime gameTime, float deltaTime) {

            playerInputs = Input.DefineInputs(ref mainGame.gameState.oldMouseState, ref mainGame.gameState.oldKbState); 

            if (Keyboard.GetState().GetPressedKeys().Length > 0)
            {
                if (mainGame.gameState.currentInputMethod != InputMethod.KEYBOARD)
                {
                    Debug.WriteLine("Current input method: keyboard");
                }
                    mainGame.gameState.currentInputMethod = InputMethod.KEYBOARD; //le clavier a la prio sur la souris (a déterminer si c'est ok)
                
            }
            if (cursorPosition != Mouse.GetState().Position)
            {
                if (mainGame.gameState.currentInputMethod != InputMethod.MOUSE)
                {
                    Debug.WriteLine("Current input method: mouse");
                }
                mainGame.gameState.currentInputMethod = InputMethod.MOUSE;

                cursorPosition = Mouse.GetState().Position;
            }
            

#if DEBUG
            mouse = Mouse.GetState();
            mouseText = mouse.Position.X/zoom + ":" + mouse.Position.Y/zoom;
            mouseTextPos = new Vector2(windowWidth/zoom - Fonts.Instance.kenPixel16.MeasureString(mouseText).X, 0);
#endif

        }

        public virtual void Draw(GameTime gameTime)
        {
#if DEBUG
            mainGame.spriteBatch.DrawString(Fonts.Instance.kenPixel16, mouseText??"", mouseTextPos, Color.Yellow);
#endif

        }

        public void GoToScene(MainGame mG, SceneType sT)
        {
            mG.gameState.ChangeScene(sT);
        }
    }
}
