using Microsoft.Xna.Framework;
using ROH.Common.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.UI;

namespace ROH.Common.Systems
{
    public class UIManagerSystem : ModSystem
    {
        public override void UpdateUI(GameTime gameTime)
        {
            ROHUIManager.UpdateUI(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            ROHUIManager.ModifyInterfaceLayers(layers);
        }
    }
}
