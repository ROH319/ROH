using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROH.Common.UI.Elements.EntityElements;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace ROH.Common.UI.Elements
{
    public class DetailButton : UIElement
    {
        public Texture2D Texture;

        public bool Expanded;

        public DetailButton(Texture2D texture)
        {
            Texture = texture;
            Width.Set(800, 0);
            Height.Set(1000, 0);
            Recalculate();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            float scale = 1f;

            if (!ROHUIManager.EntityDetails.Any(ui => ui.Depth == 0)) Expanded = false;
            if (ContainsPoint(Main.MouseScreen) || IsMouseHovering)
            {
                if(Main.mouseLeft && Main.mouseLeftRelease)
                {
                    if(!Expanded)
                    {
                        var list = Parent.Parent.Children.ToList();
                        foreach (var item in list)
                        {
                            foreach (var child in item.Children)
                            {
                                if (child is DetailButton db) db.Expanded = false;
                            }
                        }
                        ROHUIManager.AppendEntityDetail((Parent as EntityElement).GetObject(), (Parent as EntityElement).GetObjectName(), 0);
                        
                    }
                    else
                    {
                        ROHUIManager.RemoveEntityDetailByDepth(0);
                    }
                    Expanded = !Expanded;
                }
                scale = 1.2f;
            }
            float rot = Expanded ? MathHelper.PiOver2 : 0;
            var di = GetDimensions();
            Vector2 position = GetDimensions().Position() + Texture.Size() / 2;
            spriteBatch.Draw(Texture, position, null, Color.White, rot, Texture.Size()/2, scale, SpriteEffects.None, 0);

        }
    }
}
