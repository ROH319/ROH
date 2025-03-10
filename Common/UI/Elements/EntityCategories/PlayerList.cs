using ROH.Common.UI.Elements.EntityElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;
using Terraria;

namespace ROH.Common.UI.Elements.EntityCategories
{
    public class PlayerList : IEntityList
    {
        public string CategoryName => "Player";
        public List<UIElement> BuildList(float width)
        {
            List<UIElement> list = new List<UIElement>();
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                list.Add(new PlayerElement(width, i));
            }
            return list;
        }
    }
}
