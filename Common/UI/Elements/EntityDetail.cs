using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using ReLogic.Content;
using ROH.Common.UI.Elements.ProjectileDetails;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace ROH.Common.UI.Elements
{
    public class EntityDetail : DragablePanel
    {
        public object Object;

        public const int BackWidth = 400;
        public const int BackHeight = 658;

        public UIPanel InnerPanel = new UIPanel();
        public UIScrollbar Scrollbar = new UIScrollbar();
        public UIListImproved MemberList = [];
        public UIImageButton CloseButton;

        public int Depth = 0;

        public string name = "";

        public override void OnInitialize()
        {
            //Vector2 pos = GetDimensions().Position();

            OverflowHidden = false;
            Vector2 offset = new(Main.screenWidth / 2f - BackWidth / 2f, Main.screenHeight / 2f - BackHeight / 2f);
            Scrollbar.SetView(400, 1000);
            Scrollbar.Width.Set(30, 0);
            Scrollbar.MarginRight = 4;
            Scrollbar.OverflowHidden = true;
            Scrollbar.OnScrollWheel += HotbarScrollFix;

            MemberList.SetScrollbar(Scrollbar);
            MemberList.OnScrollWheel += HotbarScrollFix;

            Left.Set(offset.X, 0);
            Top.Set(offset.Y, 0);
            Width.Set(BackWidth, 0);
            Height.Set(BackHeight, 0);
            PaddingLeft = PaddingRight = PaddingTop = PaddingBottom = 0;
            BackgroundColor = new Color(29, 33, 70) * 0.7f;

            InnerPanel.Width.Set(BackWidth - 12, 0);
            InnerPanel.Height.Set(BackHeight - 70, 0);
            InnerPanel.Left.Set(6, 0);
            InnerPanel.Top.Set(32, 0);
            InnerPanel.BackgroundColor = new Color(73, 94, 171) * 0.9f;

            CloseButton = new UIImageButton(ModContent.Request<Texture2D>("Terraria/Images/UI/SearchCancel", AssetRequestMode.ImmediateLoad));
            CloseButton.Left.Set(Width.Pixels - CloseButton.Width.Pixels, 0);
            CloseButton.OnLeftClick += CloseButton_OnLeftClick;

            MemberList.Width.Set(InnerPanel.Width.Pixels - PaddingLeft * 2f, 0);
            MemberList.Height.Set(InnerPanel.Height.Pixels - PaddingTop * 2f, 0);

            Scrollbar.Height.Set(InnerPanel.Height.Pixels - 16, 0);
            //Scrollbar.Left.Set(InnerPanel.Width.Pixels - Scrollbar.Width.Pixels - 18, 0);

            Append(InnerPanel);
            Append(CloseButton);
            InnerPanel.Append(Scrollbar);
            InnerPanel.Append(MemberList);
            AddExtraChild(Scrollbar, MemberList);
            base.OnInitialize();

        }

        private void CloseButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Remove();
            ROHUIManager.EntityDetails.Remove(this);
        }

        private void HotbarScrollFix(UIScrollWheelEvent evt, UIElement listeningElement) => Main.LocalPlayer.ScrollHotbar(PlayerInput.ScrollWheelDelta / 120);

        public EntityDetail(object obj, string name, int depth)
        {
            Depth = depth;
            BuildList(obj, name);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public virtual void BuildList(object obj, string name)
        {
            this.name = name;
            MemberList.Clear();
            MemberList.OverflowHidden = false;
            if (obj != null)
            {
                var list = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                
                foreach (FieldInfo field in list)
                {
                    if (field.Name.Contains(">k__BackingField")) continue;
                    TryAddEditor(field, obj);
                }

                var list2 = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (PropertyInfo property in list2)
                {
                    if (property.Name == "Entity" || property.Name.Contains(">k__BackingField"))
                        continue;
                    TryAddEditor(property, obj);
                }
                MemberList.RecalculateChildren();
                MemberList.UpdateOrder();

            }
        }

        public void TryAddEditor(FieldInfo f, object o)
        {
            var value = f.GetValue(o);
            if(value != null && value is IEnumerable enumerable && f.FieldType != typeof(string))
            {
                MemberList.Add(new DetailEnumerable(new Terraria.ModLoader.Config.UI.PropertyFieldWrapper(f), o, Depth));
            }
            else
            {
                MemberList.Add(new DetailElement(new Terraria.ModLoader.Config.UI.PropertyFieldWrapper(f), o, f.IsStatic, Depth));
            }
        }

        public void TryAddEditor(PropertyInfo f, object o)
        {
            try
            {
                var value = f.GetValue(o);
                if (value != null && value is IEnumerable enumerable && f.PropertyType != typeof(string))
                {
                    MemberList.Add(new DetailEnumerable(new Terraria.ModLoader.Config.UI.PropertyFieldWrapper(f), o, Depth));
                }
                else
                {
                    MemberList.Add(new DetailElement(new Terraria.ModLoader.Config.UI.PropertyFieldWrapper(f), o, f.GetMethod.IsStatic, Depth));
                }
            } 
            catch 
            { 
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            Vector2 p = GetDimensions().Position();

            Utils.DrawBorderString(spriteBatch, name, p, Color.White);
        }
    }
}
