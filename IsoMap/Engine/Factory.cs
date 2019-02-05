using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.IO;

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
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore, //attention dino danger
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            StreamReader sr = new StreamReader("./Content/charactersList.json");
            String jsonFile = sr.ReadToEnd();
            CharactersListDTO characterList = JsonConvert.DeserializeObject<CharactersListDTO>(jsonFile, settings);

            JsonToPlayerCharacters(characterList);

            //monsieurBloc.mapRepresentation.idleMapSprite = new AnimatedSprite(mG.Content.Load<Texture2D>("vertical_object"), new Vector2(200, 200), 1); //TODO remplacer plus tard par une collection de sprites (voir un peu  la version des utopiales)
            //Player.Instance.currentCharacter = monsieurBloc;
        }

        private void JsonToPlayerCharacters(CharactersListDTO characterList)
        {
            foreach (CharacterDTO characterDTO in characterList.Characters)
            {
                Character character = new Character
                {
                    name = characterDTO.Name,
                    maxHP = characterDTO.Hp,
                    mapRepresentation = new MapRepresentation
                    {
                        idle = new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.MapRepresentationDTO.Idle.ImgFile),
                        Vector2.Zero, //voir ce qu'on fout de cette positiondans le constructeur pas focément utile
                        characterDTO.MapRepresentationDTO.Idle.Columns, characterDTO.MapRepresentationDTO.Idle.FrameSpeed)
                    }
                };


                Player.Instance.charactersList.Add(character);
            }


        }

        public void Load() {
            Fonts.Instance.Load(mG);
        }
        
    }

}
