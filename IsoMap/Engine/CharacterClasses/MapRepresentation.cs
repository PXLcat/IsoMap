using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.CommonImagery;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace Engine.CharacterClasses
{
    public class MapRepresentation : CommonImagery.IDrawable, /*ICollidable,*/ IMapDrawable
    {
        public AnimatedSprite idle_n, idle_ne, idle_e, idle_s, idle_se;
        public AnimatedSprite walk_n, walk_ne, walk_e, walk_se, walk_s;
        public AnimatedSprite currentSprite;

        public Polygon CurrentHitBox
        {
            get {
                Polygon result = new Polygon();
                foreach (var shapePoint in HitboxShape.Points)
                {
                    result.Points.Add(new Vector(CurrentPosition.X + shapePoint.X, CurrentPosition.Y + shapePoint.Y));
                }
                return result;
            }
        }

        public Vector2 CurrentPosition { get => currentPosition; set => currentPosition = value; }
        public Texture2D Texture { get => texture; set => texture = value; } //TODO: c'était pour implémenter l'interface IDrawable : contourner
        private Polygon hitboxShape;

        public Polygon HitboxShape
        {
            get { return hitboxShape; }
            set { hitboxShape = value; }
        }


        private Vector2 currentPosition;
        private Texture2D texture;

        public Vector2 Movement { get; set; }
        public int ZOrder { get; set; }

        private float deltaTime;

        public bool HorizontalFlip { get; set; }

#if DEBUG
        public Texture2D textureDebug;
#endif


        public void OnCollision(ICollidable other)
        {
            throw new NotImplementedException();
        }
        public MapRepresentation()
        {

        }
        public void Load()
        {
            currentSprite = idle_se;
            UpdateZOrder();

        }

        public void UpdateZOrder()
        {
            Vector2 gridPosition = Tools.IsometricToCarthesian(new TmxMap("Content/testiso.tmx"),
                new Point((int)CurrentPosition.X, (int)CurrentPosition.Y), new Point(160, 0)); //TODO faire ça proporement
            int ZPosition = 1; //TODO changer pour prendre en compte la hauteur
            ZOrder = (int)gridPosition.X + (int)gridPosition.Y+ ZPosition; 
        }

        public void Draw(SpriteBatch sb)
        {
#if DEBUG
            textureDebug = new Texture2D(sb.GraphicsDevice, 1, 1); //tention doublon à chaque Draw
            textureDebug.SetData(new[] { Color.Red });
            //VisualDebug.DrawPosition(sb, textureDebug, CurrentPosition);
            //VisualDebug.DrawSATHitbox(sb, textureDebug, CurrentHitBox);
#endif
            currentSprite.Draw(sb,1/ZOrder, HorizontalFlip);
            VisualDebug.DrawSATHitbox(sb, textureDebug, CurrentHitBox);
        }


        public void Update(List<InputType> playerInputs, float deltaTime) {
            Movement = Vector2.Zero;
            this.deltaTime = deltaTime; //qu'est ce qui est le mieux entre stocker le dT ou le passer de méthode en méthode? 
            if (playerInputs.Count > 0)
            {
                SortAndExecuteInput(playerInputs);
            }

            CurrentPosition += Movement;
            currentSprite.CurrentPosition = CurrentPosition;
            CurrentHitBox.BuildEdges(); //n'a pas aidé
            UpdateZOrder();

            if (Movement != Vector2.Zero)
            {

                //TODO Paramètre orientation/status?
            }

            currentSprite.Update(deltaTime);
        }

        private void SortAndExecuteInput(List<InputType> inputs)
        {
            if (inputs.Contains(InputType.UP) && !inputs.Contains(InputType.DOWN))
            {
                if (inputs.Contains(InputType.LEFT) && !inputs.Contains(InputType.RIGHT))
                    MoveNorthWest();
                else if (inputs.Contains(InputType.RIGHT) && !inputs.Contains(InputType.LEFT))
                    MoveNorthEast();
                else
                    MoveNorth();                
            }
            else if (inputs.Contains(InputType.DOWN) && !inputs.Contains(InputType.UP))
            {
                if (inputs.Contains(InputType.LEFT) && !inputs.Contains(InputType.RIGHT))
                    MoveSouthWest();
                else if (inputs.Contains(InputType.RIGHT) && !inputs.Contains(InputType.LEFT))
                    MoveSouthEast();
                else
                    MoveSouth();
            }
            else if (inputs.Contains(InputType.RIGHT) && !inputs.Contains(InputType.LEFT))
                MoveEast();
            else if (inputs.Contains(InputType.LEFT) && !inputs.Contains(InputType.RIGHT))
                MoveWest();
        }

        #region DEPLACEMENTS

        private void MoveNorth()
        {
            Movement += new Vector2(0, -2 * deltaTime);
            currentSprite = idle_n;
            HorizontalFlip = false;
        }
        private void MoveNorthEast()
        {
            Movement += new Vector2(2 * deltaTime, -1 * deltaTime);
            currentSprite = idle_ne;
            HorizontalFlip = false;
        }
        private void MoveEast()
        {
            Movement += new Vector2(2 * deltaTime, -0);
            currentSprite = idle_e;
            HorizontalFlip = false;
        }
        private void MoveSouthEast()
        {
            Movement += new Vector2(2 * deltaTime, 1 * deltaTime);
            currentSprite = idle_se;
            HorizontalFlip = false;
        }
        private void MoveSouth()
        {
            Movement += new Vector2(0, 2 * deltaTime);
            currentSprite = idle_s;
            HorizontalFlip = true;
        }

        private void MoveSouthWest()
        {
            Movement += new Vector2(-2 * deltaTime, 1 * deltaTime); // TODO remplacer par vitesse de déplacement
            currentSprite = idle_se;
            HorizontalFlip = true;
        }
        private void MoveWest()
        {
            Movement += new Vector2(-2 * deltaTime, 0);
            currentSprite = idle_e;
            HorizontalFlip = true;
        }
        private void MoveNorthWest()
        {
            Movement += new Vector2(-2 * deltaTime, -1 * deltaTime);
            currentSprite = idle_ne;
            HorizontalFlip = true;
        }

        #endregion
    }
}
