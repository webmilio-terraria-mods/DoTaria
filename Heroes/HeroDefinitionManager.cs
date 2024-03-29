﻿using DoTaria.Heroes.Abaddon;
using DoTaria.Heroes.Invoker;
using DoTaria.Heroes.ShadowFiend;
using DoTaria.Heroes.Terrorblade;
using DoTaria.Managers;

namespace DoTaria.Heroes
{
    public sealed class HeroDefinitionManager : SingletonManager<HeroDefinitionManager, HeroDefinition>
    {
        internal override void DefaultInitialize()
        {
            Abaddon = Add(new AbaddonHero()) as AbaddonHero;

            Invoker = Add(new InvokerHero()) as InvokerHero;

            ShadowFiend = Add(new ShadowFiendHero()) as ShadowFiendHero;

            Terrorblade = Add(new TerrorbladeHero()) as TerrorbladeHero;


            for (int i = 0; i < byIndex.Count; i++)
                AverageBaseMovementSpeed += byIndex[i].BaseMovementSpeed;

            AverageBaseMovementSpeed /= byIndex.Count;

            base.DefaultInitialize();
        }


        public AbaddonHero Abaddon { get; private set; }

        public InvokerHero Invoker { get; private set; }

        public ShadowFiendHero ShadowFiend { get; private set; }

        public TerrorbladeHero Terrorblade { get; private set; }


        public float AverageBaseMovementSpeed { get; private set; }
    }
}
