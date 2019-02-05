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

        private void JsonToPlayerCharacters(CharactersListDTO characterList) //TODO: attention à la possibilité de champs vides. Faire des vérifs pour
        {
            Player.Instance.charactersList = new List<Character>();

            foreach (CharacterDTO characterDTO in characterList.Characters)
            {
                Character character = new Character
                {
                    name = characterDTO.Name,
                    maxHP = characterDTO.Hp,
                    menuRepresentation = characterDTO.MenuRepresentationDTO==null? null: //c'est chiant de faire des ternaires, c'est obligé dans une classe imbriquée?
                    new MenuRepresentation
                    {
                        avatar5050 = String.IsNullOrEmpty(characterDTO.MenuRepresentationDTO.Avatar5050)? null :
                        mG.Content.Load<Texture2D>(characterDTO.MenuRepresentationDTO.Avatar5050)
                    },
                    sideRepresentation = characterDTO.SideRepresentationDTO == null ? null :
                    new SideRepresentation
                    {
                        idle = String.IsNullOrEmpty(characterDTO.SideRepresentationDTO.Idle.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.SideRepresentationDTO.Idle.ImgFile),
                        Vector2.Zero, //voir ce qu'on fout de cette position dans le constructeur pas focément utile
                        characterDTO.MapRepresentationDTO.Idle.Columns, characterDTO.SideRepresentationDTO.Idle.FrameSpeed),
                        run = String.IsNullOrEmpty(characterDTO.SideRepresentationDTO.Run.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.SideRepresentationDTO.Run.ImgFile),
                        Vector2.Zero,
                        characterDTO.MapRepresentationDTO.Idle.Columns, characterDTO.SideRepresentationDTO.Run.FrameSpeed),
                        jump = String.IsNullOrEmpty(characterDTO.SideRepresentationDTO.Jump.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.SideRepresentationDTO.Jump.ImgFile),
                        Vector2.Zero,
                        characterDTO.MapRepresentationDTO.Idle.Columns, characterDTO.SideRepresentationDTO.Jump.FrameSpeed),
                        fall = String.IsNullOrEmpty(characterDTO.SideRepresentationDTO.Fall.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.SideRepresentationDTO.Fall.ImgFile),
                        Vector2.Zero,
                        characterDTO.MapRepresentationDTO.Idle.Columns, characterDTO.SideRepresentationDTO.Fall.FrameSpeed),

                    },
                    mapRepresentation = characterDTO.MapRepresentationDTO == null ? null :
                    new MapRepresentation
                    {
                        idle = String.IsNullOrEmpty(characterDTO.MapRepresentationDTO.Idle.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.MapRepresentationDTO.Idle.ImgFile),
                        Vector2.Zero,
                        characterDTO.MapRepresentationDTO.Idle.Columns, characterDTO.MapRepresentationDTO.Idle.FrameSpeed),
                        run = String.IsNullOrEmpty(characterDTO.MapRepresentationDTO.Run.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.MapRepresentationDTO.Run.ImgFile),
                        Vector2.Zero,
                        characterDTO.MapRepresentationDTO.Run.Columns, characterDTO.MapRepresentationDTO.Run.FrameSpeed),
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
