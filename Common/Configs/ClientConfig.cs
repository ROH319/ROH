using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace ROH.Common.Configs
{
    public class ClientConfig : ModConfig
    {

        public override ConfigScope Mode => ConfigScope.ServerSide;
        public static ClientConfig Instance { get; set; }

        [DefaultValue(false)]
        public bool JourneyAutoResearch;

        [DefaultValue(false)]
        public bool JourneyResearchCraftable;

        [DefaultValue(false)]
        public bool DisplayProjectileName;

        public Color ProjectileTextColor;

        public Color ProjectileBorderColor;

        [DefaultValue(typeof(Color),"0, 255, 255, 255")]
        public Color FavoriteColor;

        public override void OnLoaded()
        {
            Instance = this;
        }
    }
}
