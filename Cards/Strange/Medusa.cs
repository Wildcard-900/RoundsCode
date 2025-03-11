using ModdingUtils.GameModes;
using NolansCards.Monos;
using System.Collections;
using UnboundLib.Cards;
using UnityEngine;

namespace NolansCards.Cards
{
    public class Medusa : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Debug.Log($"Medusa Card added to Player {player.playerID} - Card is being recognized!");

            // Attaches MedusaEffect if it's not already attached
            if (player.gameObject.GetComponent<MedusaEffect>() == null)
            {
                player.gameObject.AddComponent<MedusaEffect>().Initialize(player);
            }
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Debug.Log($"Medusa Card removed from Player {player.playerID}");

            // Removes the effect if the card is removed
            var effect = player.GetComponent<MedusaEffect>();
            if (effect != null)
            {
                Destroy(effect);
            }
        }

        protected override string GetTitle()
        {
            Debug.Log("Medusa card title is being accessed.");
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
            return CardInfo.Rarity.Rare;
        }
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat()
                {
                    positive = false,
                    stat = "Effect",
                    amount = "Stuns nearby enemies looking at you",
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
    public class MedusaEffect : MonoBehaviour
    {
        private Player player;
        private float stunDuration = 2f;
        private float stunRange = 15f;
        private float stunAngleThreshold = 0.5f;

        public void Initialize(Player attachedPlayer)
        {
            player = attachedPlayer;
            Debug.Log($"MedusaEffect initialized for Player {player.playerID}");
        }

        private void Update()
        {
            if (PlayerManager.instance == null || player == null)
            {
                Debug.LogWarning("PlayerManager or player is not initialized.");
                return;
            }

            foreach (var enemy in PlayerManager.instance.players)
            {
                if (enemy == null || enemy == player || enemy.data == null || enemy.data.input == null)
                {
                    continue;
                }

                ProcessStunCheck(enemy);
            }
        }

        private void ProcessStunCheck(Player enemy)
        {
            Vector2 toPlayer = (player.transform.position - enemy.transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(player.transform.position, enemy.transform.position);

            Vector2 enemyAimDirection = enemy.data.input.aimDirection;
            if (enemyAimDirection == Vector2.zero)
            {
                Debug.LogWarning($"{enemy.playerID} has zero aim direction. Skipping check.");
                return;
            }
            enemyAimDirection.Normalize();

            float dotProduct = Vector2.Dot(toPlayer, enemyAimDirection);
            bool isFacingPlayer = dotProduct >= stunAngleThreshold;

            Debug.Log($"Checking enemy {enemy.playerID}: Dot={dotProduct}, Distance={distanceToPlayer}, Facing={isFacingPlayer}");

            if (isFacingPlayer && distanceToPlayer <= stunRange)
            {
                ApplyStun(enemy);
            }
        }

        private void ApplyStun(Player target)
        {
            var stats = target.data.stats;

            if (!stats.movementSpeed.Equals(0)) // Avoids redundant stun applications
            {
                stats.movementSpeed = 0.01f; // Prevents movement (near zero)
                stats.jump = 0; // Disables jumping
                Debug.Log($"{target.playerID} is stunned by Medusa's gaze!");

                StartCoroutine(RemoveStunAfterDelay(target, stunDuration)); // Delayed stun removal
            }
        }

        private IEnumerator RemoveStunAfterDelay(Player target, float delay)
        {
            yield return new WaitForSeconds(delay);

            if (target != null && target.data != null)
            {
                target.data.stats.movementSpeed = 1f; // Restores normal speed
                target.data.stats.jump = 1; // Restores jumping
                Debug.Log($"{target.playerID} is freed from Medusa's gaze.");
            }
        }
    }
}
