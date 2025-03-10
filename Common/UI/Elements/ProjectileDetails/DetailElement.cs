using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using ROH.Common.Configs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

#nullable enable

namespace ROH.Common.UI.Elements.ProjectileDetails
{
    public class DetailElement : UIElement
    {
        public static List<Type> BasicType = new List<Type>() { typeof(int), typeof(bool), typeof(float), typeof(uint), typeof(string), typeof(short), typeof(byte)};
        // Provides access to the field/property contained in the item
        public PropertyFieldWrapper? MemberInfo { get; set; }

        public static DynamicSpriteFont Font => FontAssets.MouseText.Value;

        public object? Object;
        public string? Name {  get; set; }
        public string? Value { get; set; }
        public bool Favorited { get; set; }
        public bool IsStatic { get; set; }
        public int Depth;

        public virtual bool IsBasicType => MemberInfo == null ? true : BasicType.Contains(MemberInfo.Type);

        public DetailElement(PropertyFieldWrapper? memberInfo, object? o, bool isStatic, int depth)
        {
            MemberInfo = memberInfo;
            Object = o;
            Name = MemberInfo != null ? MemberInfo.Name : null;
            Value = MemberInfo?.GetValue(Object)?.ToString();
            Width.Set(Font.MeasureString(MemberInfo is null ? "null" : MemberInfo.Name + " " + MemberInfo.GetValue(Object)).X, 0);
            Height.Set(22, 0);
            OverflowHidden = false;
            Favorited = CheckFavorite();
            IsStatic = isStatic;
            Depth = depth;
        }
        public DetailElement(object? o, int depth)
        {
            Object = o;
            Name = Object?.GetType()?.Name;
            Width.Set(100, 0);
            Height.Set(22, 0);
            OverflowHidden = false;
            Favorited = CheckFavorite();
            Depth = depth;
        }

        public bool CheckFavorite() => Object == null ? false : Main.LocalPlayer.GetModPlayer<ROHPlayer>().FavoriteMemberList.Contains($"{Object.GetType()}.{Name}");

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            Vector2 position = GetDimensions().Position();

            string text = $"{Name} {(MemberInfo?.GetValue(Object) == null ? "null" : MemberInfo.GetValue(Object))}";
            position += new Vector2(6, 0);
            position += new Vector2(0, Font.MeasureString(text).Y * 0.175f);
            Color color = Color.White;
            if (IsStatic) color = Color.Lerp(color, new(128, 128, 128), 1f);
            if (!IsBasicType) color = Color.Lerp(color, new(255, 0, 255), 0.5f);
            if (Favorited) color = Color.Lerp(color,ClientConfig.Instance.FavoriteColor,0.5f);
            if (IsMouseHovering) color = Color.Lerp(color, Color.Green, 1f);
            Utils.DrawBorderString(spriteBatch, text, position, color);
            
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            if(!IsBasicType && MemberInfo != null)
            {
                var obj = MemberInfo.GetValue(Object);
                if (obj != null)
                {
                    ROHUIManager.AppendEntityDetail(obj, MemberInfo.Name, Depth + 1);
                }
            }
            base.LeftClick(evt);
        }

        public override void RightDoubleClick(UIMouseEvent evt)
        {
            ROHPlayer player = Main.LocalPlayer.GetModPlayer<ROHPlayer>();
            if (Object == null) return;
            var item = $"{Object.GetType()}.{Name}";
            if (!Favorited)
            {
                if (!player.FavoriteMemberList.Contains(item))
                    player.FavoriteMemberList.Add(item);
                Favorited = true;
            }
            else
            {
                player.FavoriteMemberList.Remove(item);
                Favorited = false;
            }
            UIListImproved? ui = (Parent.Parent as UIListImproved);
            ui?.UpdateOrder();
            base.RightDoubleClick(evt);
        }

        public override int CompareTo(object obj)
        {
            if (obj is DetailElement ui)
            {
                var result = string.Compare(this.Name, ui.Name, StringComparison.OrdinalIgnoreCase);
                var staticresult = IsStatic.CompareTo(ui.IsStatic);
                var favoriteresult = -Favorited.CompareTo(ui.Favorited);
                if (staticresult != 0) return staticresult;
                if (favoriteresult != 0) return favoriteresult;
                return result;
            }
            return base.CompareTo(obj);
        }
    }
}
