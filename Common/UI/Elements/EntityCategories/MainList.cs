using ROH.Common.UI.Elements.EntityElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace ROH.Common.UI.Elements.EntityCategories
{
    public class MainList : IEntityList
    {
        public string CategoryName => "Main";

        public List<UIElement> BuildList(float width)
        {
            List<UIElement> list = new List<UIElement>();
            list.Add(new MainElement(width, 0));
            return list;
        }
    }
}
