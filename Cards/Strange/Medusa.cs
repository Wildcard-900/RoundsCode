﻿using ModdingUtils.Extensions;
using ModdingUtils.GameModes;
using ModsPlus;
using NolansCards.Monos;
using System;
using System.Collections;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.Extensions;
using UnityEngine;

namespace NolansCards.Cards
{
    class Medusa : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            //Edits values on player when card is selected
            player.gameObject.AddComponent<MedusaMono>();
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            //Run when the card is removed from the player
            var mono = player.GetComponent<MedusaMono>();

            if (mono)
            {
                Destroy(mono);
            }
        }


        protected override string GetTitle()
        {
            return "Medusa";
        }
        protected override string GetDescription()
        {
            return "Don't look at me";
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
                    stat = "Effect",
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

namespace NolansCards.Monos
{
    class MedusaMono : MonoBehaviour, IGameStartHookHandler, IGameEndHookHandler, IRoundStartHookHandler, IRoundEndHookHandler, IPointStartHookHandler, IPointEndHookHandler
    {
        private Player player;
        private Block block;
        private Gun gun;

        private float lastTickTime = 0f;
        private const float timeoutThreshold = 2.5f;
        private Coroutine coroutine;
        private float notyouxpos;
        private float playerxpos;

        void Start()
        {
            InterfaceGameModeHooksManager.instance.RegisterHooks(this);
            this.player = gameObject.GetComponentInParent<Player>();
            this.block = player.data.block;
            this.block.BlockAction += this.OnBlock;
            this.gun = player.data.weaponHandler.gun;
            this.gun.ShootPojectileAction += this.OnShoot;
        }
        void Update()
        {
            if (coroutine != null)
            {
                float timeSinceLastTick = Time.time - lastTickTime;

                if (timeSinceLastTick >= timeoutThreshold)
                {
                    Console.WriteLine("Warning: Coroutine exists but is not running! Restarting...");
                    coroutine = null; // Mark as stopped
                }
            }

            if (coroutine == null)
            {
                OnRoundStart(); // Restart the coroutine if it stopped
            }
        }
        private IEnumerator Coroutine()
        {
            while (true)
            {
                lastTickTime = Time.time;

                yield return new WaitForSeconds(1f);
            }
        }
        void OnShoot(GameObject obj)
        {

        }
        void OnBlock(BlockTrigger.BlockTriggerType type)
        {

        }
        private void OnDestroy()
        {
            InterfaceGameModeHooksManager.instance.RemoveHooks(this);
            StopCoroutine(coroutine);
            this.block.BlockAction -= this.OnBlock;
            this.gun.ShootPojectileAction -= this.OnShoot;
        }
        public void OnRoundStart()
        {
            if (coroutine == null)
            {
                coroutine = StartCoroutine(Coroutine());
            }
        }
        public void OnRoundEnd()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
        public void OnGameStart()
        {
            UnityEngine.GameObject.Destroy(this);
        }

        public void OnGameEnd()
        {
            throw new NotImplementedException();
        }

        public void OnPointStart()
        {
            foreach (var notyou in PlayerManager.instance.players)
            {
                notyouxpos = notyou.transform.position.x;
                playerxpos = player.transform.position.x;
                if (notyou != player)
                {
                    if ((notyou.data.playerActions.Left.IsPressed) & (notyouxpos < playerxpos))
                    {
                        //freeze notyou
                    }
                    else if ((notyou.data.playerActions.Right.IsPressed) & (notyouxpos > playerxpos))
                    {
                        //freeze notyou
                    }
                    else
                    {
                        //unfreese notyou
                    }


                }
            }

        }

        public void OnPointEnd()
        {
            throw new NotImplementedException();
        }
    }
}