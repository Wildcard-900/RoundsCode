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
    class SniperPrecision : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`
            gun.ammo = -1;
            gun.damage = 4;
            gun.projectileSpeed = 4f;
            gun.recoilMuiltiplier = 3f;
            gun.reloadTimeAdd = 4;
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
            return "SinperPrecision";
        }
        protected override string GetDescription()
        {
            return "You're a sniper now";
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
                    stat = " Damage",
                    amount = "+4",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },

                new CardInfoStat()
                {
                    positive = true,
                    stat = " ProjectileSpeed",
                    amount = "4x",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },

                new CardInfoStat()
                {
                    positive = true,
                    stat = " ReloadTime",
                    amount = "+4s",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },

                new CardInfoStat()
                {
                    positive = false,
                    stat = " Ammo",
                    amount = "-1",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },

                new CardInfoStat()
                {
                    positive = false,
                    stat = " Recoil",
                    amount = "3x",
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