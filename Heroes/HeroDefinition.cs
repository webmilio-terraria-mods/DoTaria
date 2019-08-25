﻿using DoTaria.Abilities;
using DoTaria.Commons;
using DoTaria.Players;
using System.Collections.Generic;
using DoTaria.Attribute;
using DoTaria.Statistic;
using Terraria;
using Terraria.DataStructures;

namespace DoTaria.Heroes
{
    public abstract class HeroDefinition : IHasUnlocalizedName
    {
        public const string UNLOCALIZED_NAME_PREFIX = "heroes.";

        private readonly List<AbilityDefinition> _abilities;

        protected HeroDefinition(string unlocalizedName, string displayName, Attributes baseAttributes, Attributes gainPerLevel, Statistics baseStatistics, int baseMovementSpeed, params AbilityDefinition[] abilities)
        {
            UnlocalizedName = unlocalizedName;
            DisplayName = displayName;

            BaseAttributes = baseAttributes;
            GainPerLevel = gainPerLevel;

            BaseStatistics = baseStatistics;

            BaseMovementSpeed = baseMovementSpeed;

            _abilities = new List<AbilityDefinition>(abilities);
        }


        internal void InternalOnPlayerEnterWorld(DoTariaPlayer dotariaPlayer)
        {
            VerifyAndApplyBuffs(dotariaPlayer);

            OnPlayerEnterWorld(dotariaPlayer);
        }

        public virtual void OnPlayerEnterWorld(DoTariaPlayer dotariaPlayer)
        {
            VerifyAndApplyBuffs(dotariaPlayer);
        }


        public virtual void VerifyAndApplyBuffs(DoTariaPlayer dotariaPlayer) { }


        public virtual void ModifyWeaponDamage(DoTariaPlayer dotariaPlayer, Item item, ref float add, ref float mult, ref float flat)
        {
            foreach (KeyValuePair<AbilityDefinition, PlayerAbility> kvp in dotariaPlayer.AcquiredAbilities)
                kvp.Key.ModifyWeaponDamage(dotariaPlayer, kvp.Value, item, ref add, ref mult, ref flat);
        }


        public virtual void OnPlayerDeath(DoTariaPlayer dotariaPlayer, double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) { }

        public virtual void OnPlayerKilledNPC(DoTariaPlayer dotariaPlayer, NPC npc) { }


        public virtual void OnPlayerPostHurt(DoTariaPlayer dotariaPlayer, bool pvp, bool quiet, double damage, int hitDirection, bool crit) =>
            dotariaPlayer.ForAllAcquiredAbilities((a, p) => a.OnPlayerPostHurt(dotariaPlayer, p, pvp, quiet, damage, hitDirection, crit));


        /// <summary></summary>
        /// <param name="dotariaPlayer"></param>
        /// <param name="pvp"></param>
        /// <param name="quiet"></param>
        /// <param name="damage"></param>
        /// <param name="hitDirection"></param>
        /// <param name="crit"></param>
        /// <param name="customDamage"></param>
        /// <param name="playSound"></param>
        /// <param name="genGore"></param>
        /// <param name="damageSource"></param>
        /// <returns>Return false to stop the player from taking damage. Default returns true.</returns>
        public virtual bool OnPlayerPreHurt(DoTariaPlayer dotariaPlayer, bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) => true;


        internal void InternalOnPlayerPreUpdateMovement(DoTariaPlayer dotariaPlayer)
        {
            VerifyAndApplyBuffs(dotariaPlayer);

            OnPlayerPreUpdateMovement(dotariaPlayer);
        }

        public virtual void OnPlayerPreUpdateMovement(DoTariaPlayer dotariaPlayer) { }


        public virtual void OnPlayerResetEffects(DoTariaPlayer dotariaPlayer) { }


        public string UnlocalizedName { get; }
        public string DisplayName { get; }

        public Attributes BaseAttributes { get; }
        public Attributes GainPerLevel { get; }

        public Statistics BaseStatistics { get; }

        public int BaseMovementSpeed { get; }

        public IReadOnlyList<AbilityDefinition> Abilities => _abilities.AsReadOnly();
    }
}
