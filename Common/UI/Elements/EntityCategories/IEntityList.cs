using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace ROH.Common.UI.Elements.EntityCategories
{
    public interface IEntityList
    {
        public abstract string CategoryName { get; }

        public abstract List<UIElement> BuildList(float width);
    }
}
