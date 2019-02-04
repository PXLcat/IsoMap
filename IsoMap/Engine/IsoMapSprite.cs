using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public Vector2 Movement { get; set; }

        public void OnCollision(ICollidable other)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch sb)
        {
            idleMapSprite.Draw(sb);
            
        }

        public void Update(List<InputType> playerInputs) {
            Movement = Vector2.Zero;
            if (playerInputs.Count > 0)
            {
                SortAndExecuteInput(playerInputs);
            }
            CurrentPosition += Movement;
            idleMapSprite.CurrentPosition = CurrentPosition;
        }

        private void SortAndExecuteInput(List<InputType> inputs)
        {
            if (inputs.Contains(InputType.LEFT) && (inputs.Contains(InputType.RIGHT)))
            {
                //ResetPose();
            }
            else if (inputs.Contains(InputType.LEFT) && (!inputs.Contains(InputType.RIGHT)))
            {
                MoveLeft();
            }
            else if (inputs.Contains(InputType.RIGHT) && (!inputs.Contains(InputType.LEFT)))
            {
                MoveRight();
            }
            if (inputs.Contains(InputType.UP))
            {
                MoveUp();
            }
            else if (inputs.Contains(InputType.DOWN))
            {
                MoveDown();
            }

        }

        private void MoveDown()
        {
            Movement += new Vector2(-4,2); //remplacer par vitesse de déplacement
        }

        private void MoveUp()
        {
            Movement += new Vector2(4,-2);
        }

        private void MoveRight()
        {
            Movement += new Vector2(4, 2);
        }

        private void MoveLeft()
        {
            Movement += new Vector2(-4, -2);
        }
    }
}
