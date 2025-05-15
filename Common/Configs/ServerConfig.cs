using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace ROH.Common.Configs
{
    public class ServerConfig : ModConfig
    {

        public override ConfigScope Mode => ConfigScope.ServerSide;
        public static ServerConfig Instance { get; set; }

        [DefaultValue(false)]
        public bool JourneyAutoResearch;

        [DefaultValue(false)]
        public bool JourneyResearchCraftable;

        public override void OnLoaded()
        {
            Instance = this;
            base.OnLoaded();
        }
    }
}
