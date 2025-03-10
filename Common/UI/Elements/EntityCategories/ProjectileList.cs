using ROH.Common.UI.Elements.EntityElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace ROH.Common.UI.Elements.EntityCategories
{
    public class ProjectileList : IEntityList
    {
        public string CategoryName => "Projectile";

        public List<UIElement> BuildList(float width)
        {
            List<UIElement> list = new List<UIElement>();
            for(int i = 0;i < Main.maxProjectiles;i++)
            {
                list.Add(new ProjectileElement(width, i));
            }
            return list;
        }
    }
}
