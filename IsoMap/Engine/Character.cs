using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoMap.Engine
{
    public class Character
    {
        public String name;
        public int currentHP;
        public int maxHP;
        public Status characterStatus;
        public Texture2D avatar; //shit faut que le cadre soit à part
        

        public Character(string name, int maxHP)
        {
            this.name = name;
            this.maxHP = maxHP;
            currentHP = maxHP;
            this.characterStatus = Status.NONE;
        }
        
        public enum Status
        {
            NONE,
            POISONED,
            PARALYSED,
            KO
        }
    }
}
