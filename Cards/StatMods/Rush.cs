using ModdingUtils.Extensions;
using ModdingUtils.GameModes;
using NolansCards.Monos;
using System;
using System.Collections;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.Extensions;
using UnityEngine;

namespace NolansCards.Cards
{
    class Rush : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`
            gun.reloadTimeAdd = -0.5f;
            gun.projectileSpeed = 1.3f;
            gun.ammo = 3;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            //Edits values on player when card is selected
            player.gameObject.AddComponent<TemplateMono>();
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            //Run when the card is removed from the player
            var mono = player.GetComponent<TemplateMono>();

            if (mono)
            {
                Destroy(mono);
            }
        }
        protected override string GetTitle()
        {
            return "Rush";
        }
        protected override string GetDescription()
        {
            return "Go get em";
        }
        protected override GameObject GetCardArt()
        {
            return null;
        }
        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Common;
        }
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat()
                {
                    positive = true,
                    stat = "bullets",
                    amount = "+3",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },

                new CardInfoStat()
                {
                    positive = true,
                    stat = "FasterReload",
                    amount = "Yes",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.ColdBlue;
        }
        public override string GetModName() 
        {
            return NolansCards.ModInitials;
        }
    }
}
