using ROH.Common.UI.Elements.EntityElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace ROH.Common.UI.Elements.EntityCategories
{
    public class StaticClassList : IEntityList
    {
        public string CategoryName => "Static Class";

        public List<UIElement> BuildList(float width)
        {
            List<UIElement> list = new List<UIElement>();
            list.Add(new ProjectileIDElement(width, 0));
            return list;
        }
    }
}
