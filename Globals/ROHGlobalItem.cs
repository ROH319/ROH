using ROH.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ROH.Globals
{
    public class ROHGlobalItem : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(item, tooltips);
            int index = tooltips.FindLastIndex((x => x.Mod == "Terraria"));
            if(index != -1 && item.useAmmo != AmmoID.None && ClientConfig.Instance.DisplayUseAmmo)
            {
                tooltips.Insert(index, new TooltipLine(Mod, "ROH : ItemUseAmmo", Language.GetTextValue("Mods.ROH.Items.Extra.UseAmmo") + $"[i:{item.useAmmo}]"));
            }
        }
    }
}
