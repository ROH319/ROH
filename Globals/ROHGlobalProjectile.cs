using Microsoft.Xna.Framework;
using ROH.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ROH.Globals
{
    public class ROHGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool IsMouseHovering;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {

            base.OnSpawn(projectile, source);
        }

        public override bool PreAI(Projectile projectile)
        {
            return base.PreAI(projectile);
        }

        public override void PostAI(Projectile projectile)
        {
            base.PostAI(projectile);
        }

        public override Color? GetAlpha(Projectile projectile, Color lightColor)
        {
            if(!projectile.hostile && (ClientConfig.Instance.GlobalProjectileAlpha < 1f || ClientConfig.Instance.ModProjectileAlpha < 1f))
            {
                Color? color = projectile.ModProjectile?.GetAlpha(lightColor);
                if(color != null)
                {
                    Color newcolor = color.Value;
                    if(projectile.ModProjectile != null)
                        newcolor *= ClientConfig.Instance.ModProjectileAlpha;
                    return newcolor * ClientConfig.Instance.GlobalProjectileAlpha;
                }
                lightColor *= projectile.Opacity * ClientConfig.Instance.GlobalProjectileAlpha;
                if(projectile.ModProjectile != null)
                    lightColor *= ClientConfig.Instance.ModProjectileAlpha;
                return lightColor;
            }
            return base.GetAlpha(projectile, lightColor);
        }

        public override void PostDraw(Projectile projectile, Color lightColor)
        {
            if (ClientConfig.Instance.DisplayProjectileName)
            {
                Utils.DrawBorderStringFourWay(Main.spriteBatch,
                    FontAssets.MouseText.Value,
                    Lang.GetProjectileName(projectile.type).Value + $"{projectile.ModProjectile?.Name}",
                    projectile.position.X - Main.screenPosition.X,
                    projectile.position.Y - Main.screenPosition.Y,
                    ClientConfig.Instance.ProjectileTextColor,
                    ClientConfig.Instance.ProjectileBorderColor,
                    Vector2.Zero);
            }

            if (IsMouseHovering)
            {
                Vector2 topleft = projectile.position;
                Vector2 topright = projectile.position + new Vector2(projectile.width, 0);
                Vector2 bottomleft = projectile.position + new Vector2(0, projectile.height);
                Vector2 bottomright = projectile.position + new Vector2(projectile.width, projectile.height);
                Utils.DrawLine(Main.spriteBatch, topleft, topright, Color.Red,Color.Red,2);
                Utils.DrawLine(Main.spriteBatch, topright, bottomright, Color.Red, Color.Red, 2);
                Utils.DrawLine(Main.spriteBatch, bottomright, bottomleft, Color.Red, Color.Red, 2);
                Utils.DrawLine(Main.spriteBatch, topleft, bottomleft, Color.Red, Color.Red, 2);
            }
            base.PostDraw(projectile, lightColor);
        }
    }
}
