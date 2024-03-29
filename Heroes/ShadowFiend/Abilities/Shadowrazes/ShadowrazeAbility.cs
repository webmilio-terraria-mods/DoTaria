﻿using DoTaria.Abilities;
using DoTaria.Commons;
using DoTaria.Dusts.Heroes.ShadowFiend.Default;
using DoTaria.Enums;
using DoTaria.Extensions;
using DoTaria.Helpers;
using DoTaria.Players;
using DoTaria.Skins.ShadowFiend.Default;
using DoTaria.Statistic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;

namespace DoTaria.Heroes.ShadowFiend.Abilities.Shadowrazes
{
    public abstract class ShadowrazeAbility : AbilityDefinition
    {
        public const string UNLOCALIZED_NAME_PREFIX = ShadowFiendHero.UNLOCALIZED_NAME + ".";

        public static readonly AbilityDefinition[] shadowrazes = new AbilityDefinition[] { AbilityDefinitionManager.Instance.ShadowrazeNear, AbilityDefinitionManager.Instance.ShadowrazeMiddle, AbilityDefinitionManager.Instance.ShadowrazeFar };


        protected ShadowrazeAbility(string unlocalizedName, string displayName, AbilitySlot abilitySlot, int baseCastRange, bool affectsTotalAbilityLevelCount) :
            base(UNLOCALIZED_NAME_PREFIX + unlocalizedName, displayName, AbilityType.Active, AbilityTargetType.NoTarget, AbilityTargetFaction.Enemies, AbilityTargetUnitType.Living, DamageType.Magical, abilitySlot, 1, 4, baseCastRange: baseCastRange, affectsTotalAbilityLevelCount: affectsTotalAbilityLevelCount)
        {
        }

        public override string GetAbilityTooltip(DoTariaPlayer dotariaPlayer, PlayerAbility playerAbility) =>
            $"Base damage: {AbilitiesHelper.GenerateCleanSlashedString(InternalGetAbilityDamage, dotariaPlayer, this)}\n" +
            $"Range: {AbilitiesHelper.GenerateCleanSlashedString(InternalGetCastRange, dotariaPlayer, this)}\n" +
            $"Bonus per stack: {AbilitiesHelper.GenerateCleanSlashedString(GetDamageIncreasePerStack, dotariaPlayer, this)}\n" + 
            $"Stack duration: {AbilitiesHelper.GenerateCleanSlashedString(GetStackDuration, dotariaPlayer, this)}";


        public override bool CastAbility(DoTariaPlayer dotariaPlayer, PlayerAbility playerAbility, bool casterIsLocalPlayer)
        {
            Texture2D texture = typeof(ShadowrazeProjectile).GetTexture();
            Vector2 spawnPosition = new Vector2(dotariaPlayer.player.position.X, dotariaPlayer.player.position.Y + dotariaPlayer.player.height + 25) - 
                                    new Vector2((-InternalGetCastRange(dotariaPlayer, playerAbility) * dotariaPlayer.player.direction) * DoTariaMath.TERRARIA_RANGE_RATIO, texture.Height / (ShadowrazeProjectile.FRAME_COUNT * 2f));

            for (int i = 0; i < 10; i++)
            {
                bool left = Main.rand.NextBool();

                Dust.NewDust(spawnPosition - new Vector2(37.5f, -60), 65, 15, ModContent.DustType<ShadowTrail>(), 1f * (left ? -1 : 1), -1f, 0, new Color(255, 255, 255), 3f);
            }

            if (casterIsLocalPlayer)
                Projectile.NewProjectile(spawnPosition, Vector2.Zero, ModContent.ProjectileType<ShadowrazeProjectile>(), (int)InternalGetAbilityDamage(dotariaPlayer, playerAbility), 0, dotariaPlayer.player.whoAmI);

            return true;
        }


        public float GetDamageIncreasePerStack(DoTariaPlayer dotariaPlayer, PlayerAbility playerAbility) => 40 + playerAbility.Level * 10;

        public float GetStackDuration(DoTariaPlayer dotariaPlayer, PlayerAbility playerAbility) => 8;


        // TODO Add support for talents.
        public override float GetCooldown(DoTariaPlayer dotariaPlayer, PlayerAbility playerAbility) => 10;

        public override float GetManaCost(DoTariaPlayer dotariaPlayer, PlayerAbility playerAbility) => 90 * Statistics.TERRARIA_MANA_RATIO;

        public override float GetAbilityDamage(DoTariaPlayer dotariaPlayer, PlayerAbility playerAbility) => 20 + 70 * playerAbility.Level;

        public override void OnAbilityLeveledUp(DoTariaPlayer dotariaPlayer, PlayerAbility playerAbility)
        {
            for (int i = 0; i < shadowrazes.Length; i++)
                if (shadowrazes[i] != this)
                    dotariaPlayer.AcquireOrLevelUp(shadowrazes[i], false);
        }
    }
}
