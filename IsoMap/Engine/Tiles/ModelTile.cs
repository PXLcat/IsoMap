﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IsoMap.Engine;

namespace IsoMap.Engine.Tiles
{
    abstract public class ModelTile : ICollidable, IMapDrawable
    {
        private Rectangle sourceRectangle;
        public bool traversablePourHumain = false;


        public Rectangle HitBox
        {
            get => new Rectangle((int)CurrentPosition.X, (int)CurrentPosition.Y, 100, 100);//TODO
        }

        public Vector2 CurrentPosition { get; set; }
        public Vector2 BasePosition { get; set; }

        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int ZPosition { get; set; }
        public int ZOrder { get; set; }

        public virtual void OnCollision(ICollidable other)
        {
            
        }

        public void Draw(SpriteBatch sb)
        {
            //sb.Draw(Map.tileset, new Vector2(CurrentPosition.X, CurrentPosition.Y), sourceRectangle, Color.White);
        }

        public ModelTile(Vector2 basePosition, Rectangle sourceRectangle,int width, int height)
        {
            BasePosition = basePosition;
            CurrentPosition = BasePosition;
            this.sourceRectangle = sourceRectangle;
        }

        public void Update(int scrollX, int scrollY) {
            CurrentPosition = new Vector2(BasePosition.X - scrollX, BasePosition.Y - scrollY);
        }
    }
}