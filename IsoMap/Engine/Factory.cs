using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace IsoMap.Engine
{
    public class Factory //attention pas thread safe
    {
        private static Factory instance = null;
        public MainGame mG;
        

        public static Factory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Factory();
                }
                return instance;
            }
        }

        public void SetMainGame(MainGame mG)
        {
            this.mG = mG;
        }

        public Texture2D LoadTexture(String assetPath) => mG.Content.Load<Texture2D>(assetPath);

        private Factory()
        {
        }

        public void LoadPlayer()
        {
            Character monsieurBloc = new Character();
            monsieurBloc.mapRepresentation = new IsoMapRepresentation();
            monsieurBloc.mapRepresentation.idleMapSprite = new AnimatedSprite(mG.Content.Load<Texture2D>("vertical_object"), new Vector2(200, 200), 1); //TODO remplacer plus tard par une collection de sprites (voir un peu  la version des utopiales)
            Player.Instance.currentCharacter = monsieurBloc;
        }



        public List<Character> GetCharacters()
        {
            List<Character> charactersList = new List<Character>();
            charactersList.Add((new Character("Bidule", 20)));
            charactersList[0].avatar = mG.Content.Load<Texture2D>("ciale5050cadre");
            charactersList.Add((new Character("Truc", 30)));
            charactersList[1].avatar = mG.Content.Load<Texture2D>("machin2_5050cadre");
            charactersList[1].characterStatus = Character.Status.PARALYSED;
            charactersList.Add((new Character("Chouette", 30)));
            charactersList[2].avatar = mG.Content.Load<Texture2D>("machin2_5050cadre");
            return charactersList;
        }

        public void Load() {
            Fonts.Instance.Load(mG);
        }
        
    }

}
