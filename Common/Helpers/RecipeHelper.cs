using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ROH.Common.Helpers
{
    public static class RecipeHelper
    {
        public static void CreateSimpleRecipe(int ingredientID, int resultID, int tileID, int ingredientAmount = 1, int resultAmount = 1, bool disableDecraft = false, params Condition[] conditions)
        {
            var recipe = Recipe.Create(resultID, resultAmount);
            recipe.AddIngredient(ingredientID,ingredientAmount);
            recipe.AddTile(tileID);
            foreach(var condition in conditions)
            {
                recipe.AddCondition(condition);
            }
            if(disableDecraft)
            {
                recipe.DisableDecraft();
            }
            recipe.Register();
        }
    }
}
