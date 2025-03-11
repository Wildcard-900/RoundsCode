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
    class SnipeAway : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`
            gun.reloadTimeAdd = 5;
            gun.projectileSpeed = 3f;
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
            return "SnipeAway";
        }
        protected override string GetDescription()
        {
            return "Your bullets become fast, but good luck reloading";
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
                    stat = "5x Bullet Velocity",
                    amount = "No",
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