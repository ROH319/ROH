using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ROH.Common.UI.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace ROH.Common.UI
{
    public static class ROHUIManager
    {
        public static UserInterface EntityListUserInterface { get; set; }
        public static UserInterface EntityDetailUserInterface { get; set; }
        public static UserInterface ScreenUserInterface { get; set; }

        public static List<EntityDetail> EntityDetails { get; set; }
        public static EntityLister EntityLister { get; set; }
        public static Screen Screen { get; set; }
        private static GameTime LastUpdateUIGameTime { get; set; }

        public static Asset<Texture2D> DetailButton { get; set; }

        public static void LoadUI()
        {
            if (!Main.dedServ)
            {
                DetailButton = ModContent.Request<Texture2D>("Terraria/Images/UI/ButtonPlay", AssetRequestMode.ImmediateLoad);

                EntityListUserInterface = new();
                EntityDetailUserInterface = new();
                ScreenUserInterface = new();

                EntityLister = new();
                EntityLister.Activate();

                Screen = new();
                Screen.Activate();

                EntityDetails = new();

                //EntityDetailUserInterface.SetState(EntityDetail);
                ScreenUserInterface.SetState(Screen);
            }
        }

        public static void UpdateUI(GameTime gameTime)
        {
            LastUpdateUIGameTime = gameTime;

            if (ROH.EntityListKey.JustPressed)
            {
                ROHUIManager.ToggleEntityLister();
            }

            if (!Main.playerInventory)
            {
                CloseEntityLister();
                EntityDetailUserInterface.SetState(null);
            }

            if (EntityListUserInterface?.CurrentState != null)
                EntityListUserInterface.Update(gameTime);
            if (EntityDetailUserInterface?.CurrentState != null)
                EntityDetailUserInterface.Update(gameTime);
            if(ScreenUserInterface?.CurrentState != null)
                ScreenUserInterface.Update(gameTime);
        }

        public static void AppendEntityDetail(object obj, string name, int depth)
        {
            RemoveEntityDetailByDepth(depth);

            EntityDetail ed = new EntityDetail(obj, name, depth);
            ed.Activate();
            ed.Left.Set(Main.MouseScreen.X + 40 * depth, 0);
            ed.Top.Set(Main.MouseScreen.Y + 40 * depth, 0);
            EntityDetails.Add(ed);
            ROHUIManager.Screen.Append(ed);
        }
        public static void AppendEntityDetailEnumerable(object obj, string name, int depth)
        {
            RemoveEntityDetailByDepth(depth);

            EntityDetail ed = new EntityDetailEnumerable(obj, name, depth);
            ed.Activate();
            ed.Left.Set(Main.MouseScreen.X + 40 * depth, 0);
            ed.Top.Set(Main.MouseScreen.Y + 40 * depth, 0);
            EntityDetails.Add(ed);
            ROHUIManager.Screen.Append(ed);
        }

        public static void RemoveEntityDetailByDepth(int depth)
        {
            EntityDetails.RemoveAll(ui => ui.Depth >= depth);
        }

        public static bool IsEntityListerOpen() => EntityListUserInterface.CurrentState == null;

        public static void CloseEntityLister()
        {
            EntityListUserInterface?.SetState(null);

        }
        public static void OpenEntityLister() => EntityListUserInterface.SetState(EntityLister);

        public static void ToggleEntityLister()
        {
            if(IsEntityListerOpen())
            {
                SoundEngine.PlaySound(SoundID.MenuOpen);
                OpenEntityLister();
                Main.LocalPlayer.GetModPlayer<ROHPlayer>().EntityListRebuildCooldown = 0;
                Main.playerInventory = true;
            }
            else
            {
                SoundEngine.PlaySound(SoundID.MenuClose);
                CloseEntityLister();
                Main.playerInventory = false;
            }
        }

        public static void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex((layer) => layer.Name == "Vanilla: Inventory");
            if(index != -1)
            {
                layers.Insert(index, new LegacyGameInterfaceLayer("ROH: Entity Lister", delegate
                {
                    if (EntityListUserInterface.CurrentState != null)
                        EntityListUserInterface.Draw(Main.spriteBatch, LastUpdateUIGameTime);
                    if (EntityDetailUserInterface.CurrentState != null)
                        EntityDetailUserInterface.Draw(Main.spriteBatch, LastUpdateUIGameTime);
                    if (ScreenUserInterface.CurrentState != null)
                        ScreenUserInterface.Draw(Main.spriteBatch, LastUpdateUIGameTime);
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
}
