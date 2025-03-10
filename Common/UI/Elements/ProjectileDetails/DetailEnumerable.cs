using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using ROH.Common.Configs;
using System;
using System.Collections;
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
    public class DetailEnumerable : DetailElement
    {
        public int EnumeratorCount { get; set; }
        public override bool IsBasicType => false;

        public DetailEnumerable(PropertyFieldWrapper? memberInfo, object? o, int depth) : base(memberInfo, o, false, depth)
        {
            IEnumerator? ie = (memberInfo?.GetValue(o) as IEnumerable)?.GetEnumerator();

            if (ie != null)
            {
                while (ie.MoveNext())
                {
                    EnumeratorCount++;
                }
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 position = GetDimensions().Position();

            string text = MemberInfo == null ? "null" : $"{MemberInfo.Name} [{EnumeratorCount}]";
            position += new Vector2(6, 0);
            position += new Vector2(0, Font.MeasureString(text).Y * 0.175f);
            Color color = Color.White;
            if (IsStatic) color = Color.Lerp(color, new(128, 128, 128), 1f);
            if (!IsBasicType) color = Color.Lerp(color, new(255, 0, 255), 0.5f);
            if (Favorited) color = Color.Lerp(color, ClientConfig.Instance.FavoriteColor, 0.5f);
            if (IsMouseHovering) color = Color.Lerp(color, Color.Green, 0.5f);
            Utils.DrawBorderString(spriteBatch, text, position, color);

        }

        public override void LeftClick(UIMouseEvent evt)
        {
            if (!IsBasicType && MemberInfo != null && Object != null)
            {
                var obj = MemberInfo.GetValue(Object);
                if (EnumeratorCount > 0)
                {
                    ROHUIManager.AppendEntityDetailEnumerable(obj, MemberInfo.Name, Depth + 1);
                }
            }
        }
    }
}
