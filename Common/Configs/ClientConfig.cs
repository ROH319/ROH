using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader.Config;

namespace ROH.Common.Configs
{
    public class ClientConfig : ModConfig
    {

        public override ConfigScope Mode => ConfigScope.ClientSide;
        public static ClientConfig Instance { get; set; }

        [DefaultValue(true)]
        public bool DisplayUseAmmo;

        [DefaultValue(false)]
        public bool DisplayProjectileName;

        public Color ProjectileTextColor;

        public Color ProjectileBorderColor;

        [DefaultValue(typeof(Color),"0, 255, 255, 255")]
        public Color FavoriteColor;

        [DefaultValue(1f)]
        [Slider]
        public float GlobalProjectileAlpha;

        [DefaultValue(1f)]
        [Slider]
        public float ModProjectileAlpha;

        public override void OnLoaded()
        {
            Instance = this;
        }

        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            GlobalProjectileAlpha = Utils.Clamp(GlobalProjectileAlpha, 0f, 1f);
            ModProjectileAlpha = Utils.Clamp(ModProjectileAlpha,0f, 1f);
        }
    }
}
