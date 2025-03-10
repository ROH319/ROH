using ROH.Common.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ROH
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class ROH : Mod
	{
        public static ModKeybind ResearchKey;
        public static ModKeybind SacrificeKey;
        public static ModKeybind EntityListKey;
        public override void Load()
        {
            ResearchKey = KeybindLoader.RegisterKeybind(this, "ResearchKey", Microsoft.Xna.Framework.Input.Keys.Y);
            SacrificeKey = KeybindLoader.RegisterKeybind(this, "SacrificeKey", Microsoft.Xna.Framework.Input.Keys.Home);
            EntityListKey = KeybindLoader.RegisterKeybind(this, "ProjectileListKey", Microsoft.Xna.Framework.Input.Keys.G);

            ROHUIManager.LoadUI();

            base.Load();
        }

        public override void Unload()
        {
            ResearchKey = null;
            SacrificeKey = null;
            EntityListKey = null;
            itemsAndTileIDsOfStations = null;
            base.Unload();
        }
        public override void PostSetupContent()
        {
            itemsAndTileIDsOfStations = new();
            HashSet<int> craftingTilesIDs = new();
            Recipe[] recipe = Main.recipe;
            for(int i = 0; i < recipe.Length; i++)
            {
                foreach(int requiredTile in recipe[i].requiredTile)
                {
                    craftingTilesIDs.Add(requiredTile);
                }
            }
            for(int i = 0; i < ItemLoader.ItemCount; i++)
            {
                Item item = new Item(i, 1, 0);
                foreach(int craftingTileID in craftingTilesIDs)
                {
                    int tileid = item.createTile;
                    if (tileid.Equals(craftingTileID))
                    {
                        itemsAndTileIDsOfStations.Add(new ValueTuple<int, int>(i, craftingTileID));
                    }
                    else if (ModContent.GetModTile(tileid) != null)
                    {
                        ModTile modtile = ModContent.GetModTile(tileid);
                        if (modtile.AdjTiles.Contains(craftingTileID))
                        {
                            itemsAndTileIDsOfStations.Add(new ValueTuple<int, int>(i, craftingTileID));
                        }
                    }
                }
            }
            base.PostSetupContent();
        }

        public static HashSet<ValueTuple<int, int>> itemsAndTileIDsOfStations;
	}
}
