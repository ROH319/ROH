using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace ROH.Common.UI.Elements.EntityElements
{
    public class ProjectileIDElement : EntityElement
    {
        public override bool IsActive() => true;
        public override object GetObject() => null;
        public override string GetObjectName() => "ProjectileID";
        public ProjectileIDElement(float width, int index) : base(width, index)
        {
            Height.Set(22, 0);
        }
    }
}
