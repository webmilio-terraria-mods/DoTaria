﻿
using DoTaria.Abilities;
using DoTaria.Enums;
using DoTaria.Helpers;
using DoTaria.Network;
using DoTaria.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DoTaria.Heroes.Abaddon.Abilities.MistCoil
{
    public sealed class MistCoilAbility : AbilityDefinition
    {
        private const string UNLOCALIZED_NAME = AbaddonHero.UNLOCALIZED_NAME + ".mistCoil";

        public MistCoilAbility() : base(UNLOCALIZED_NAME, "Mist Coil",
            AbilityType.Active, AbilityTargetType.TargetUnit, AbilityTargetFaction.Allies & AbilityTargetFaction.Enemies, AbilityTargetUnitType.Living,
            DamageType.Pure, AbilitySlot.First, 1, 4)
        {
        }

        public override string GetAbilityTooltip(DoTariaPlayer dotariaPlayer, PlayerAbility playerAbility) =>
            "Abaddon releases a coil of deathly mist that can damage an enemy unit or heal a friendly unit at the cost of some of Abaddon's health.\n\n" +
            $"Self damage: {AbilitiesHelper.GenerateCleanSlashedString((player, ability) => GetSelfDamage(dotariaPlayer, ability), dotariaPlayer, this)}\n" +
            $"Damage/Heal: {AbilitiesHelper.GenerateCleanSlashedString((player, ability) => InternalGetAbilityDamage(dotariaPlayer, ability), dotariaPlayer, this)}";


        public override bool CastAbility(DoTariaPlayer dotariaPlayer, PlayerAbility playerAbility, bool casterIsLocalPlayer)
        {
            dotariaPlayer.player.Hurt(PlayerDeathReason.ByPlayer(dotariaPlayer.player.whoAmI), (int)GetSelfDamage(dotariaPlayer, playerAbility), 1);

            if (casterIsLocalPlayer)
            {
                EntitiesHelper.GetLocalHoveredEntity(out Player player, out NPC npc);

                if (player == null && npc == null)
                    return false;

                int projectileId = Projectile.NewProjectile(dotariaPlayer.player.position, Vector2.Zero, ModContent.ProjectileType<MistCoilProjectile>(), 0, 0f, dotariaPlayer.player.whoAmI);
                MistCoilProjectile mistCoil = Main.projectile[projectileId].modProjectile as MistCoilProjectile;

                if (player != null) 
                    mistCoil.HomeOntoPlayer = player;
                else if (npc != null)
                    mistCoil.HomeOntoNPC = npc;

                mistCoil.DamageOnContact = (int)InternalGetAbilityDamage(dotariaPlayer, playerAbility);


                NetworkPacketManager.Instance.MistCoilFired.SendPacketToAllClients(dotariaPlayer.player.whoAmI, dotariaPlayer.player.whoAmI, (player != null ? MistCoilFiredPacket.TargetType.Player : MistCoilFiredPacket.TargetType.NPC).ToString(),
                    projectileId, player?.whoAmI ?? NPCsHelper.GetNPCIdFromNPC(npc), (int)mistCoil.DamageOnContact);
            }

            return true;
        }


        public float GetSelfDamage(DoTariaPlayer dotariaPlayer, PlayerAbility playerAbility) => 50 + playerAbility.Level * 25;

        public override float GetAbilityDamage(DoTariaPlayer dotariaPlayer, PlayerAbility playerAbility) => 95 + playerAbility.Level * 45;

        // TODO Add support for talents.
        public override float GetCooldown(DoTariaPlayer dotariaPlayer, PlayerAbility playerAbility) => 4.5f;

        public override float GetManaCost(DoTariaPlayer dotariaPlayer, PlayerAbility playerAbility) => 50;
    }
}
