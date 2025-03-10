using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using ROH.Common.Helpers;
using ROH.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace ROH.Common.UI.Elements.EntityElements
{
    public class ProjectileElement : EntityElement
    {
        public override bool IsActive() => Main.projectile[Index].active;
        public override object GetObject() => Main.projectile[Index];
        public override string GetObjectName() => Main.projectile[Index].Name;
        public ProjectileElement(float width, int index) : base(width, index)
        {
            Height.Set(22, 0);
        }

        //protected override void DrawSelf(SpriteBatch spriteBatch)
        //{
        //    base.DrawSelf(spriteBatch);
        //    var di = GetDimensions();
        //    var di2 = Parent.GetDimensions();
        //    Vector2 position = GetDimensions().Position();

        //    string name = IsNull ? "null" : GetObjectName();
        //    string text = $"[{Index}] {name}";
        //    position += new Vector2(20 * Main.UIScale + 11, 0);
        //    position += new Vector2(0, UIHelper.Font.MeasureString(text).Y * 0.175f);

        //    Color color = Color.White;
        //    if (IsNull) color = Color.Red;
        //    if (!IsActive()) color = Color.Lerp(color, Color.Black, 0.5f);
        //    if (IsMouseHovering) color = Color.Lerp(color, Color.Green, 0.5f);

        //    Utils.DrawBorderString(spriteBatch, text, position, color);
        //}
        public override void MouseOver(UIMouseEvent evt)
        {
            Main.projectile[Index].TryGetGlobalProjectile(out ROHGlobalProjectile global);
            if (global != null) global.IsMouseHovering = true;
            base.MouseOver(evt);
        }
        public override void MouseOut(UIMouseEvent evt)
        {
            Main.projectile[Index].TryGetGlobalProjectile(out ROHGlobalProjectile global);
            if (global != null) global.IsMouseHovering = false;
            base.MouseOut(evt);
        }

    }
}
