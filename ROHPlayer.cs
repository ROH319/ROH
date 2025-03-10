using ROH.Common.Configs;
using ROH.Common.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Exceptions;
using Terraria.ModLoader.IO;

namespace ROH
{
    public class ROHPlayer : ModPlayer
    {
        public List<string> FavoriteMemberList = new();

        public int EntityListRebuildCooldown = 0;

        public override void Load()
        {
            FavoriteMemberList = new();
            base.Load();
        }
        public override void Unload()
        {
            FavoriteMemberList = null;
            base.Unload();
        }

        public override void PostUpdate()
        {
            //var str = $"raintime:{Main.rainTime} raining:{Main.raining} maxRaining:{Main.maxRaining} cloudBGActive:{Main.cloudBGActive} cloudAlpha:{Main.cloudAlpha}";
            Main.projectile[0].ai[0] += 0;
            
            //Main.NewText(str);

            if(EntityListRebuildCooldown > 0 && !Main.gamePaused)
            {
                EntityListRebuildCooldown--;
            }
            base.PostUpdate();
        }
        public PlayerDeathReason DeathByLocalization(string key)
        {
            return PlayerDeathReason.ByCustomReason(Language.GetTextValue($"Mods.ROH.DeathMessage.{key}", Player.name));
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if(ROH.ResearchKey.JustPressed)
            {
                if (ClientConfig.Instance.JourneyAutoResearch && Main.LocalPlayer.difficulty is PlayerDifficultyID.Creative)
                {
                    string researchedItems = "";
                    bool hasItemResearched = false;
                    foreach (var item in Player.inventory.Where(i => !i.IsAir))
                    {
                        if (CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(item.type, out int amountNeeded))
                        {
                            int sacrificeCount = Main.LocalPlayerCreativeTracker.ItemSacrifices.GetSacrificeCount(item.type);
                            if (amountNeeded - sacrificeCount > 0 && item.stack >= amountNeeded - sacrificeCount)
                            {
                                CreativeUI.SacrificeItem(item.Clone(), out _);
                                SoundEngine.PlaySound(SoundID.Research);
                                SoundEngine.PlaySound(SoundID.ResearchComplete);
                                researchedItems = string.Concat(researchedItems, $"[i:{item.type}]");
                                hasItemResearched = true;
                            }
                        }
                    }
                    if (hasItemResearched)
                    {
                        Main.NewText($"已研究：{researchedItems}");
                    }
                }

                if (ClientConfig.Instance.JourneyResearchCraftable && Main.LocalPlayer.difficulty is PlayerDifficultyID.Creative)
                {
                    bool needResearchAgain = true;
                    bool hasItemResearched = false;
                    string researchedItems = "";
                    while (needResearchAgain)
                    {
                        needResearchAgain = false;
                        foreach (Recipe recipe in Main.recipe)
                        {
                            CreativeUI.GetSacrificeCount(recipe.createItem.type, out bool resultAlreadyResearched);
                            if (resultAlreadyResearched) continue;

                            #region 检查所需条件
                            bool allConditionMet = true;
                            if (recipe.Conditions.Count > 0)
                            {
                                foreach (var condition in recipe.Conditions)
                                {
                                    if (!condition.IsMet())
                                    {
                                        allConditionMet = false;
                                        break;
                                    }
                                }
                            }
                            if (!allConditionMet) continue;

                            #endregion

                            #region 检查所需物块
                            bool stationResearched = false;
                            if (recipe.requiredTile.Count == 0)
                            {
                                stationResearched = true;
                            }
                            if (recipe.createItem.type == 0) continue;
                            foreach (int tile in recipe.requiredTile)
                            {
                                foreach (var valuetuple in ROH.itemsAndTileIDsOfStations)
                                {
                                    int itemid = valuetuple.Item1;
                                    int tileid = valuetuple.Item2;
                                    if (tile == tileid)
                                    {
                                        CreativeUI.GetSacrificeCount(itemid, out bool requiredStationIsResearched);
                                        if (requiredStationIsResearched)
                                        {
                                            stationResearched = true;
                                            break;
                                        }
                                    }
                                }
                                if (stationResearched) break;
                            }
                            if (!stationResearched) continue;
                            #endregion

                            #region 检查所需物品
                            bool allItemsResearched = true;
                            foreach (Item requiredItem in recipe.requiredItem)
                            {
                                CreativeUI.GetSacrificeCount(requiredItem.type, out bool requiredItemIsResearched);
                                if (!requiredItemIsResearched)
                                {
                                    allItemsResearched = false;
                                    break;
                                }
                            }
                            if (!allItemsResearched) continue;
                            #endregion

                            CreativeUI.ResearchItem(recipe.createItem.type);
                            needResearchAgain = true;
                            hasItemResearched = true;
                            researchedItems = string.Concat(researchedItems, $"[i:{recipe.createItem.type}]");
                        }
                    }
                    if (hasItemResearched)
                    {
                        Main.NewText("已研究可制作物品：" + researchedItems);
                        SoundEngine.PlaySound(SoundID.ResearchComplete);
                    }
                }
            }
            if (ROH.SacrificeKey.JustPressed)
            {
                if (Player.active && !Player.dead)
                {
                    Player.KillMe(DeathByLocalization("Sacrifice"), int.MaxValue, 0);
                }
            }
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
        }

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
        }
    }
}
