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
        }

        public MapRepresentation LoadTestCharacter()
        {
            MapRepresentation legisteTest = new MapRepresentation();
            legisteTest.CurrentPosition = new Vector2(130, 200);
            legisteTest.idle_s = new AnimatedSprite(mG.Content.Load<Texture2D>("./Images/legiste_map_idle_s"),legisteTest.CurrentPosition,1,1,Origin.MIDDLE_DOWN);
            legisteTest.currentSprite = legisteTest.idle_s;
            
            return legisteTest;
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
                        #region Idle
                        idle_n = String.IsNullOrEmpty(characterDTO.MapRepresentation.Idle.North.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.MapRepresentation.Idle.North.ImgFile),
                        Vector2.Zero, characterDTO.MapRepresentation.Idle.North.Columns, characterDTO.MapRepresentation.Idle.North.Rows,
                        Origin.MIDDLE_DOWN, framespeed: characterDTO.MapRepresentation.Idle.North.FrameSpeed),

                        idle_ne = String.IsNullOrEmpty(characterDTO.MapRepresentation.Idle.Northeast.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.MapRepresentation.Idle.Northeast.ImgFile),
                        Vector2.Zero, characterDTO.MapRepresentation.Idle.Northeast.Columns, characterDTO.MapRepresentation.Idle.Northeast.Rows,
                        Origin.MIDDLE_DOWN, framespeed: characterDTO.MapRepresentation.Idle.Northeast.FrameSpeed),

                        idle_e = String.IsNullOrEmpty(characterDTO.MapRepresentation.Idle.East.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.MapRepresentation.Idle.East.ImgFile),
                        Vector2.Zero, characterDTO.MapRepresentation.Idle.East.Columns, characterDTO.MapRepresentation.Idle.East.Rows,
                        Origin.MIDDLE_DOWN, framespeed: characterDTO.MapRepresentation.Idle.East.FrameSpeed),

                        idle_se = String.IsNullOrEmpty(characterDTO.MapRepresentation.Idle.SouthEast.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.MapRepresentation.Idle.SouthEast.ImgFile),
                        Vector2.Zero, characterDTO.MapRepresentation.Idle.SouthEast.Columns, characterDTO.MapRepresentation.Idle.SouthEast.Rows,
                        Origin.MIDDLE_DOWN, framespeed: characterDTO.MapRepresentation.Idle.SouthEast.FrameSpeed),

                        idle_s = String.IsNullOrEmpty(characterDTO.MapRepresentation.Idle.South.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.MapRepresentation.Idle.South.ImgFile),
                        Vector2.Zero, characterDTO.MapRepresentation.Idle.South.Columns, characterDTO.MapRepresentation.Idle.South.Rows,
                        Origin.MIDDLE_DOWN, framespeed: characterDTO.MapRepresentation.Idle.South.FrameSpeed),
                        #endregion
                        #region Walk
                        walk_n = String.IsNullOrEmpty(characterDTO.MapRepresentation.Walk.North.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.MapRepresentation.Walk.North.ImgFile),
                        Vector2.Zero, characterDTO.MapRepresentation.Walk.North.Columns, characterDTO.MapRepresentation.Walk.North.Rows,
                        Origin.MIDDLE_DOWN, framespeed: characterDTO.MapRepresentation.Walk.North.FrameSpeed),

                        walk_ne = String.IsNullOrEmpty(characterDTO.MapRepresentation.Walk.Northeast.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.MapRepresentation.Walk.Northeast.ImgFile),
                        Vector2.Zero, characterDTO.MapRepresentation.Walk.Northeast.Columns, characterDTO.MapRepresentation.Walk.Northeast.Rows,
                        Origin.MIDDLE_DOWN, framespeed: characterDTO.MapRepresentation.Walk.Northeast.FrameSpeed),

                        walk_e = String.IsNullOrEmpty(characterDTO.MapRepresentation.Walk.East.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.MapRepresentation.Walk.East.ImgFile),
                        Vector2.Zero, characterDTO.MapRepresentation.Walk.East.Columns, characterDTO.MapRepresentation.Walk.East.Rows,
                        Origin.MIDDLE_DOWN, framespeed: characterDTO.MapRepresentation.Walk.East.FrameSpeed),

                        walk_se = String.IsNullOrEmpty(characterDTO.MapRepresentation.Walk.SouthEast.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.MapRepresentation.Walk.SouthEast.ImgFile),
                        Vector2.Zero, characterDTO.MapRepresentation.Walk.SouthEast.Columns, characterDTO.MapRepresentation.Walk.SouthEast.Rows,
                        Origin.MIDDLE_DOWN, framespeed: characterDTO.MapRepresentation.Walk.SouthEast.FrameSpeed),

                        walk_s = String.IsNullOrEmpty(characterDTO.MapRepresentation.Walk.South.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>(characterDTO.MapRepresentation.Walk.South.ImgFile),
                        Vector2.Zero, characterDTO.MapRepresentation.Walk.South.Columns, characterDTO.MapRepresentation.Walk.South.Rows,
                        Origin.MIDDLE_DOWN, framespeed: characterDTO.MapRepresentation.Walk.South.FrameSpeed),
                        #endregion

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
