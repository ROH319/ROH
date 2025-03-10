using ROH.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROH.Common.Recipes
{
    public class ConversionRecipe : ModSystem
    {
        public override void AddRecipes()
        {
            AddEvilConvertions();
            base.AddRecipes();
        }

        public void AddEvilConvertions()
        {
            AddConvertRecipe(ItemID.Plum, ItemID.Cherry);//李子<->樱桃
            AddConvertRecipe(ItemID.Mango, ItemID.Pineapple);//芒果<->菠萝
            AddConvertRecipe(ItemID.Coconut, ItemID.Banana);//椰子<->香蕉
            AddConvertRecipe(ItemID.Elderberry, ItemID.BlackCurrant);//接骨木果<->黑醋栗
            AddConvertRecipe(ItemID.Dragonfruit, ItemID.Starfruit);//火龙果<->杨桃
            AddConvertRecipe(ItemID.BloodOrange, ItemID.Rambutan);//血橙<->红毛丹
            AddConvertRecipe(ItemID.SpicyPepper, ItemID.Pomegranate);//辣椒<->石榴
        }

        public static void AddConvertRecipe(int item1ID, int item2ID)
        {
            RecipeHelper.CreateSimpleRecipe(item1ID, item2ID, TileID.DemonAltar, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(item2ID, item1ID, TileID.DemonAltar, disableDecraft: true);
        }
    }
}
