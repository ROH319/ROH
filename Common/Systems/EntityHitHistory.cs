using Mono.Cecil.Cil;
using MonoMod.Cil;
using ROH.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ROH.Common.Systems
{
    public class EntityHitHistory : ModSystem
    {
        public override void Load()
        {
            On_NPC.HitModifiers.ToHitInfo += HitModifiers_ToHitInfo;

            var damage = typeof(Projectile).GetMethod("Damage", BindingFlags.Public | BindingFlags.Instance);
            MonoModHooks.Modify(damage, IL_Projectile_Damage);

            base.Load();
        }

        public override void Unload()
        {
            On_NPC.HitModifiers.ToHitInfo -= HitModifiers_ToHitInfo;
            base.Unload();
        }

        public static void IL_Projectile_Damage(ILContext il)
        {
            FieldInfo projectiledamage = typeof(Projectile).GetField(nameof(Projectile.damage));
            FieldInfo NPChurtinfo = typeof(ROHGlobalNPC).GetField(nameof(ROHGlobalNPC.HurtInfos));
            var c = new ILCursor(il);
            if(!c.TryGotoNext(MoveType.AfterLabel,
                i => i.MatchLdloca(28),
                i => i.MatchLdarg0(),
                i => i.MatchLdfld(projectiledamage),
                i => i.MatchConvR4(),
                i => i.MatchLdloc(30),
                i => i.MatchLdloc(29)
                ))
            {
                throw new Exception("IL edit failed");
            }
            //for(Instruction i = c.Next;!i.MatchLdarg0();i = i.Next)
            //{
            //    c.Emit(i.OpCode, i.Operand);
            //}
            NPC.HitModifiers hits = new NPC.HitModifiers();

            c.EmitLdloc(28);
            c.EmitDelegate<Action<NPC.HitModifiers>>((hit) =>
            {
                hits = hit;
            });
            //c.EmitPop();

            if (!c.TryGotoNext(MoveType.AfterLabel,
                i => i.MatchLdloc(26)
                ))
            {
                throw new Exception("IL edit failed");
            }
            //for (Instruction i = c.Next;!i.Next.MatchLdloc(26);i = i.Next)
            //{
            //    c.Emit(i.OpCode, i.Operand);
            //}

            c.EmitLdloc(26);
            c.EmitDelegate<Action<NPC>>((npc) =>
            {
                ROHGlobalNPC.HurtInfo hurtinfo = new ROHGlobalNPC.HurtInfo();
                hurtinfo.HitModifiers = hits;
                hurtinfo.Time = (int)Main.GameUpdateCount;
                npc.GetGlobalNPC<ROHGlobalNPC>().HurtInfos.Add(hurtinfo);
                //ROHGlobalNPC.Add(hits);
            });
            //c.EmitPop();
        }

        private NPC.HitInfo HitModifiers_ToHitInfo(On_NPC.HitModifiers.orig_ToHitInfo orig, ref NPC.HitModifiers self, float baseDamage, bool crit, float baseKnockback, bool damageVariation, float luck)
        {
            return orig(ref self, baseDamage, crit, baseKnockback, damageVariation, luck);
        }
    }
}
