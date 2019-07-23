﻿using DoTaria.Abilities;
using DoTaria.Enums;
using DoTaria.Players;

namespace DoTaria.Heroes.ShadowFiend.Abilities
{
    public sealed class PresenceoftheDarkLordAbility : AbilityDefinition
    {
        public const string UNLOCALIZED_NAME = "presenceoftheDarkLord"; 

        public PresenceoftheDarkLordAbility() : base(ShadowFiendHero.UNLOCALIZED_NAME + '.' + UNLOCALIZED_NAME, "Presence of the Dark Lord", AbilityType.Passive, DamageType.None, AbilitySlot.Fifth, 4)
        {
        }


        public override float GetCooldown(DoTariaPlayer dotariaPlayer, PlayerAbility playerAbility) => 0;

        public override float GetManaCost(DoTariaPlayer dotariaPlayer, PlayerAbility playerAbility) => 0;
    }
}