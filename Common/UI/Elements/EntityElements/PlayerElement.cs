using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ROH.Common.UI.Elements.EntityElements
{
    public class PlayerElement : EntityElement
    {
        public override bool IsActive() => Main.player[Index].active;
        public override object GetObject() => Main.player[Index];
        public override string GetObjectName() => Main.player[Index].name;
        public PlayerElement(float width, int index) : base(width, index)
        {
            Height.Set(22, 0);
        }
    }
}
