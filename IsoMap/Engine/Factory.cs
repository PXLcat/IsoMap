using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.IO;
using IsoMap;
using IsoMap.Engine;
using Engine.CommonImagery;
using Engine.CharacterClasses;

namespace Engine
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
                    menuRepresentation = characterDTO.MenuRepresentation==null? null: //c'est chiant de faire des ternaires, c'est obligé dans une classe imbriquée?
                    new MenuRepresentation
                    {
                        //avatar5050 = String.IsNullOrEmpty(characterDTO.MenuRepresentation.Avatar5050)? null :
                        //mG.Content.Load<Texture2D>(characterDTO.MenuRepresentation.Avatar5050)
                    },
                    sideRepresentation = characterDTO.SideRepresentation == null ? null :
                    new SideRepresentation
                    {
                        idle = String.IsNullOrEmpty(characterDTO.SideRepresentation.Idle.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.SideRepresentation.Idle.ImgFile),
                        Vector2.Zero, //voir ce qu'on fout de cette position dans le constructeur pas focément utile
                        characterDTO.SideRepresentation.Idle.Columns, characterDTO.SideRepresentation.Idle.FrameSpeed),
                        run = String.IsNullOrEmpty(characterDTO.SideRepresentation.Run.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.SideRepresentation.Run.ImgFile),
                        Vector2.Zero,
                        characterDTO.SideRepresentation.Idle.Columns, characterDTO.SideRepresentation.Run.FrameSpeed),
                        jump = String.IsNullOrEmpty(characterDTO.SideRepresentation.Jump.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.SideRepresentation.Jump.ImgFile),
                        Vector2.Zero,
                        characterDTO.SideRepresentation.Idle.Columns, characterDTO.SideRepresentation.Jump.FrameSpeed),
                        fall = String.IsNullOrEmpty(characterDTO.SideRepresentation.Fall.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.SideRepresentation.Fall.ImgFile),
                        Vector2.Zero,
                        characterDTO.SideRepresentation.Idle.Columns, characterDTO.SideRepresentation.Fall.FrameSpeed),

                    },
                    mapRepresentation = characterDTO.MapRepresentation == null ? null :
                    new MapRepresentation
                    {
                        idle_front = String.IsNullOrEmpty(characterDTO.MapRepresentation.Idle_front.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.MapRepresentation.Idle_front.ImgFile),
                        Vector2.Zero,
                        characterDTO.MapRepresentation.Idle_front.Columns, characterDTO.MapRepresentation.Idle_front.Rows, framespeed:characterDTO.MapRepresentation.Idle_front.FrameSpeed),
                        idle_back = String.IsNullOrEmpty(characterDTO.MapRepresentation.Idle_back.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.MapRepresentation.Idle_back.ImgFile),
                        Vector2.Zero,
                        characterDTO.MapRepresentation.Idle_back.Columns, characterDTO.MapRepresentation.Idle_back.Rows, framespeed: characterDTO.MapRepresentation.Idle_back.FrameSpeed),
                        run_front = String.IsNullOrEmpty(characterDTO.MapRepresentation.Run_front.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.MapRepresentation.Run_front.ImgFile),
                        Vector2.Zero,
                        characterDTO.MapRepresentation.Run_front.Columns, characterDTO.MapRepresentation.Run_front.Rows, framespeed: characterDTO.MapRepresentation.Run_front.FrameSpeed),
                        run_back = String.IsNullOrEmpty(characterDTO.MapRepresentation.Run_back.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.MapRepresentation.Run_back.ImgFile),
                        Vector2.Zero,
                        characterDTO.MapRepresentation.Run_back.Columns, characterDTO.MapRepresentation.Run_back.Rows, framespeed:characterDTO.MapRepresentation.Run_back.FrameSpeed),
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
