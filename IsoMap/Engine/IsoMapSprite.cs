using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoMap.Engine
{
    public class IsoMapRepresentation : IDrawable, ICollidable
    {
        public AnimatedSprite idleMapSprite;

        public Rectangle HitBox => throw new NotImplementedException();

        public Vector2 CurrentPosition { get => currentPosition; set => currentPosition = value; }
        public Texture2D Texture { get => texture; set => texture = value; }

        private Vector2 currentPosition;
        private Texture2D texture;

        public void OnCollision(ICollidable other)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch sb)
        {
            idleMapSprite.Draw(sb);
        }

        public void Update() {

        }
    }
}
