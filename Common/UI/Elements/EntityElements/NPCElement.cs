using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ROH.Common.UI.Elements.EntityElements
{
    public class NPCElement : EntityElement
    {
        public override bool IsActive() => Main.npc[Index].active;
        public override object GetObject() => Main.npc[Index];
        public override string GetObjectName() => Main.npc[Index].FullName;
        public NPCElement(float width, int index) : base(width, index)
        {
            Height.Set(22, 0);
        }
    }
}
