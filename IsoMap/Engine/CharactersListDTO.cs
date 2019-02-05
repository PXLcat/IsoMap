using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoMap.Engine
{
    public class CharactersListDTO
    {
        public CharacterDTO[] Characters { get; set; }
    }
    public class CharacterDTO
    {
        public String Name { get; set; }
        public int Hp { get; set; }
        public MenuRepresentationDTO MenuRepresentationDTO { get; set; }
        public SideRepresentationDTO SideRepresentationDTO { get; set; }
        public MapRepresentationDTO MapRepresentationDTO { get; set; }

    }
    public class MenuRepresentationDTO
    {
        public String Avatar5050 { get; set; } //prévoir smileys selon le statut dans les menus?
    }
    public class SideRepresentationDTO
    {
        public SpriteStateDTO Idle { get; set; }
        public SpriteStateDTO Run { get; set; }
        public SpriteStateDTO Jump { get; set; }
        public SpriteStateDTO Fall { get; set; }
    }
    public class MapRepresentationDTO
    {
        public SpriteStateDTO Idle { get; set; }
        public SpriteStateDTO Run { get; set; }
    }
    public class SpriteStateDTO
    {
        public String ImgFile { get; set; }
        public int Columns { get; set; }
        public int FrameSpeed { get; set; }
    }
}
