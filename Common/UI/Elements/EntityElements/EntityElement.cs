using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROH.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace ROH.Common.UI.Elements.EntityElements
{
    public abstract class EntityElement : UIElement
    {
        public DetailButton detailButton { get; set; }

        public int Index = 0;
        public bool IsNull => GetObject() == null;
        public virtual bool IsActive() => GetObject() != null;
        public virtual object GetObject() { return null; }
        public virtual string GetObjectName() { return null; }

        public EntityElement(float width, int index)
        {
            Index = index;
            Width.Set(width, 0);
            detailButton = new DetailButton(ROHUIManager.DetailButton.Value);
            Append(detailButton);
        }

        protected EntityElement(int index)
        {
            Index = index;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            Vector2 position = GetDimensions().Position();

            string name = IsNull ? "null" : GetObjectName();
            string text = $"[{Index}] {name}";
            position += new Vector2(20 * Main.UIScale + 11, 0);
            position += new Vector2(0, UIHelper.Font.MeasureString(text).Y * 0.175f);

            Color color = Color.White;
            if (IsNull) color = Color.Red;
            if (!IsActive()) color = Color.Lerp(color, Color.Black, 0.5f);
            if (IsMouseHovering) color = Color.Lerp(color, Color.Green, 0.5f);

            Utils.DrawBorderString(spriteBatch, text, position, color);
        }

        public override int CompareTo(object obj)
        {
            if (obj is EntityElement ee)
            {
                var result = Index.CompareTo(ee.Index);
                var nullResult = IsNull.CompareTo(ee.IsNull);
                var activeResult = -(IsActive().CompareTo(ee.IsActive()));
                if (nullResult != 0) return nullResult;
                if (activeResult != 0) return activeResult;
                return result;
                //if (IsNull)
                //{
                //    if (ee.IsNull) return result;
                //    else return 1;
                //}
                //else
                //{
                //    if (IsActive())
                //    {
                //        if (ee.IsActive()) return result;
                //        else return -1;
                //    }
                //    else
                //    {
                //        if (ee.IsActive()) return 1;
                //        else return result;
                //    }
                //}
            }
            return base.CompareTo(obj);
        }
    }
}
