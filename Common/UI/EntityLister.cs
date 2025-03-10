using Microsoft.Xna.Framework;
using ROH.Common.UI.Elements;
using ROH.Common.UI.Elements.EntityCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace ROH.Common.UI
{
    public class EntityLister : UIState
    {
        public const int BackWidth = 560;
        public const int BackHeight = 658;

        public DragablePanel BackPanel;

        public UIPanel FilterPanel;
        public UIPanel InnerPanel;
        public UIScrollbar Scrollbar;
        public UIListImproved EntityList;

        public UIPanel CategoryPanel;
        public UIScrollbar CategoryScroll;
        public UIListImproved CategoryList;

        public string SelectedCategory;
        public List<IEntityList> EntityLists;
        public Dictionary<string, List<UIElement>> EntityListStorage;


        public override void OnInitialize()
        {
            Vector2 offset = new(Main.screenWidth / 2f - BackWidth / 2f, Main.screenHeight / 2f - BackHeight / 2f);

            Scrollbar = new UIScrollbar();
            Scrollbar.SetView(400, 1000);
            Scrollbar.Width.Set(20, 0);
            Scrollbar.OverflowHidden = true;
            Scrollbar.OnScrollWheel += HotbarScrollFix;

            EntityList = [];
            EntityList.SetScrollbar(Scrollbar);
            EntityList.OnScrollWheel += HotbarScrollFix;

            CategoryScroll = new UIScrollbar();
            CategoryScroll.SetView(400, 800);
            CategoryScroll.Width.Set(16, 0);
            CategoryScroll.OverflowHidden = true;
            CategoryScroll.OnScrollWheel += HotbarScrollFix;

            CategoryList = [];
            CategoryList.SetScrollbar(CategoryScroll);
            CategoryList.OnScrollWheel += HotbarScrollFix;

            BackPanel = new DragablePanel(Scrollbar, EntityList, CategoryScroll, CategoryList);
            BackPanel.Left.Set(offset.X, 0);
            BackPanel.Top.Set(offset.Y, 0);
            BackPanel.Width.Set(BackWidth, 0);
            BackPanel.Height.Set(BackHeight, 0);
            BackPanel.PaddingLeft = BackPanel.PaddingRight = BackPanel.PaddingTop = BackPanel.PaddingBottom = 0;
            BackPanel.BackgroundColor = new Color(29, 33, 70) * 0.7f;

            CategoryPanel = new UIPanel();
            CategoryPanel.Width.Set(160, 0);
            CategoryPanel.Height.Set(BackHeight - 70, 0);
            CategoryPanel.Left.Set(6, 0);
            CategoryPanel.Top.Set(32, 0);
            CategoryPanel.BackgroundColor = new Color(73, 94, 171) * 0.9f;

            InnerPanel = new UIPanel();
            InnerPanel.Width.Set(BackWidth - CategoryPanel.Width.Pixels - 24, 0);
            InnerPanel.Height.Set(BackHeight - 70, 0);
            InnerPanel.Left.Set(CategoryPanel.Left.Pixels + CategoryPanel.Width.Pixels + 6, 0);
            InnerPanel.Top.Set(32, 0);
            InnerPanel.BackgroundColor = new Color(73, 94, 171) * 0.9f;

            CategoryList.Width.Set(CategoryPanel.Width.Pixels - CategoryPanel.PaddingLeft * 2f - CategoryScroll.Width.Pixels, 0);
            CategoryList.Height.Set(CategoryPanel.Height.Pixels - CategoryPanel.PaddingTop * 2f, 0);

            CategoryScroll.Height.Set(CategoryPanel.Height.Pixels - 16, 0);
            CategoryScroll.Left.Set(CategoryPanel.Width.Pixels - CategoryScroll.Width.Pixels - 18, 0);

            EntityList.Width.Set(InnerPanel.Width.Pixels - InnerPanel.PaddingLeft * 2f - Scrollbar.Width.Pixels, 0);
            EntityList.Height.Set(InnerPanel.Height.Pixels - InnerPanel.PaddingTop * 2f, 0);

            Scrollbar.Height.Set(InnerPanel.Height.Pixels - 16, 0);
            Scrollbar.Left.Set(InnerPanel.Width.Pixels - Scrollbar.Width.Pixels - 18, 0);


            Append(BackPanel);
            BackPanel.Append(CategoryPanel);
            BackPanel.Append(InnerPanel);
            CategoryPanel.Append(CategoryScroll);
            CategoryPanel.Append(CategoryList);
            InnerPanel.Append(Scrollbar);
            InnerPanel.Append(EntityList);

            EntityLists = GetLoadedEntityList();
            EntityListStorage = GetLoadedList();
            base.OnInitialize();
        }

        private void HotbarScrollFix(UIScrollWheelEvent evt, UIElement listeningElement) => Main.LocalPlayer.ScrollHotbar(PlayerInput.ScrollWheelDelta / 120);

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            ROHPlayer modplayer = Main.LocalPlayer.GetModPlayer<ROHPlayer>();

            //CategoryList.Clear();
            if (CategoryList.Count < 1)
            {
                BuildCategory();
            }
            if (SelectedCategory == "")
            {
                WhiteAllCategoryButtons();
                SelectedCategory = EntityListStorage.ElementAt(0).Key;
                SetCurrentList(SelectedCategory);
            }
            EntityList.UpdateOrder();
        }

        public void BuildCategory()
        {
            CategoryList.Clear();
            float yPos = 0;
            for(int i = 0; i < EntityListStorage.Count; i++)
            {
                UIButton<string> button = new UIButton<string>(EntityListStorage.ElementAt(i).Key);
                button.Width.Set(CategoryList.Width.Pixels - 6,0);
                button.Height.Set(44, 0);
                button.Left.Set(0, 0);
                button.OnLeftClick += Button_OnLeftClick;
                CategoryList.Add(button);
            }
        }

        private void Button_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            UIButton<string> button = listeningElement as UIButton<string>;
            WhiteAllCategoryButtons();
            SetCurrentList(button.Text);
            button.TextColor = Color.Yellow;
        }

        public void WhiteAllCategoryButtons()
        {
            for(int i = 0;i < CategoryList.Children.Count();i++)
            {
                foreach(var list in CategoryList.Children)
                {
                    foreach (var button in list.Children)
                    {
                        (button as UIButton<string>).TextColor = Color.White;
                    }
                }
            }
        }

        public void SetCurrentList(string categoryName)
        {
            EntityList.Clear();
            EntityList.AddRange(EntityListStorage[categoryName]);
        }

        public List<IEntityList> GetLoadedEntityList()
        {
            List<IEntityList> list = new List<IEntityList>();
            foreach (var mod in ModLoader.Mods)
            {
                foreach(Type type in AssemblyManager.GetLoadableTypes(mod.Code))
                {
                    if(!type.IsAbstract && type.GetInterfaces().Contains(typeof(IEntityList)))
                    {
                        var instance = Activator.CreateInstance(type);
                        list.Add(instance as IEntityList);
                    }
                }
            }
            return list;
        }

        public Dictionary<string, List<UIElement>> GetLoadedList()
        {
            Dictionary<string, List<UIElement>> dic = new Dictionary<string, List<UIElement>>();

            foreach (IEntityList list in EntityLists)
            {
                dic.Add(list.CategoryName, list.BuildList(EntityList.Width.Pixels));
            }
            return dic;
        }
    }
}
