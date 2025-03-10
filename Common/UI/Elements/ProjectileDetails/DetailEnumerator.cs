using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROH.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

#nullable enable

namespace ROH.Common.UI.Elements.ProjectileDetails
{
    public class DetailEnumerator : DetailElement
    {
        public int Index;
        public override bool IsBasicType => Object == null ? true : BasicType.Contains(Object.GetType());
        public DetailEnumerator(object o, int depth, int index) : base(o, depth)
        {
            Index = index;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 position = GetDimensions().Position();

            string text = $"[{Index}] {(Object == null ? "null" : Object.ToString())}";
            position += new Vector2(6, 0);
            position += new Vector2(0, Font.MeasureString(text).Y * 0.175f);
            Color color = Color.White;
            if (!IsBasicType) color = Color.Lerp(color, new(255, 0, 255), 0.5f);
            if (Favorited) color = Color.Lerp(color, ClientConfig.Instance.FavoriteColor, 0.5f);
            if (IsMouseHovering) color = Color.Lerp(color, Color.Green, 0.5f);
            Utils.DrawBorderString(spriteBatch, text, position, color);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            if (!IsBasicType && Object != null)
            {
                var obj = Object;
                if (obj != null)
                {
                    ROHUIManager.AppendEntityDetail(obj, Object.GetType().Name, Depth + 1);
                }
            }
        }

        public object? GetValue(object obj)
        {
            return (obj as IEnumerable<object>)?.ToArray()[Index];
        }

        public override int CompareTo(object obj)
        {
            return Index.CompareTo((obj as DetailEnumerator)?.Index);
        }
    }
}
