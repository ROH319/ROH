using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ROH.Globals
{
    public class ROHGlobalNPC : GlobalNPC
    {
        public struct HurtInfo
        {
            public int Time;
            public NPC.HitInfo HitInfo;
            public NPC.HitModifiers HitModifiers;
            public Projectile projectile;
            public Item item;

            public HurtInfo(int time, NPC.HitInfo hitInfo, NPC.HitModifiers hitModifiers, Projectile projectile = null, Item item = null)
            {
                Time = time;
                HitInfo = hitInfo;
                HitModifiers = hitModifiers;
                this.projectile = projectile;
                this.item = item;
            }
        }
        public static List<NPC.HitModifiers> HitModifiersList = new();
        public List<HurtInfo> HurtInfos = new();
        public NPC.HitModifiers CurrentModifier;
        public int HitTime;
        public override bool InstancePerEntity => true;
        public override void Load()
        {
            base.Load();
        }
        public static void Add(NPC.HitModifiers hitModifiers)
        {
            HitModifiersList.Add(hitModifiers);
        }
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            int time = (int)Main.GameUpdateCount;
            for(int i = 0;i < HurtInfos.Count; i++)
            {
                if (HurtInfos[i].Time == time)
                {
                    HurtInfo h = HurtInfos[i];
                    h.HitInfo = hit;
                    h.item = item;
                    HurtInfos[i] = h;
                }
            }
            base.OnHitByItem(npc, player, item, hit, damageDone);
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            int time = (int)Main.GameUpdateCount;
            for(int i = 0; i < HurtInfos.Count; i++)
            {
                if (HurtInfos[i].Time == time)
                {
                    HurtInfo h = HurtInfos[i];
                    h.HitInfo = hit;
                    h.projectile = projectile;
                    HurtInfos[i] = h;
                }
            }
            base.OnHitByProjectile(npc, projectile, hit, damageDone);
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            CurrentModifier = modifiers;
            HitTime = (int)Main.GameUpdateCount;
            
            base.ModifyHitByItem(npc, player, item, ref modifiers);
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            CurrentModifier = modifiers;
            HitTime = (int)Main.GameUpdateCount;

            base.ModifyHitByProjectile(npc, projectile, ref modifiers);
        }


    }
}
