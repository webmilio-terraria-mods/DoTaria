﻿using System;
using System.Collections.Generic;
using DoTaria.UserInterfaces;
using DoTaria.UserInterfaces.Abilities;
using DoTaria.UserInterfaces.HeroSelection;
using DoTaria.UserInterfaces.HeroStatistics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace DoTaria
{
    public sealed partial class DoTariaMod : Mod
    {
        private const int 
            MENU_MODE_HERO_SELECTION = 888,
            MENU_MODE_CHARACTER_SELECTION = 1,
            MENU_MODE_CHARACTER_NAMING = 2;

        private UserInterface 
            _abilitiesInterface,
            _heroStatisticsInterface;


        private void LoadInterfaces()
        {
            HeroSelectionUI = new HeroSelectionUIState(this);


            AbilitiesUI = new AbilitiesUIState(this);
            AbilitiesUI.Activate();

            _abilitiesInterface = new UserInterface();
            _abilitiesInterface.SetState(AbilitiesUI);


            HeroStatisticsUI = new HeroStatisticsUIState(this);
            HeroStatisticsUI.Activate();
            
            _heroStatisticsInterface = new UserInterface();
            _heroStatisticsInterface.SetState(HeroStatisticsUI);


            AbilitiesUI.Visible = true;

            Main.OnTick += UpdateTickInterfaces;
        }

        private void UnloadInterfaces()
        {
            AbilitiesUI.Visible = false;
            AbilitiesUI = null;

            HeroStatisticsUI.Visible = false;
            HeroSelectionUI.Dispose();
            HeroSelectionUI = null;

            Main.OnTick -= UpdateTickInterfaces;
        }


        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int abilitiesBarLayerIndex = layers.FindIndex(l => l.Name.Equals("Vanilla: Resource Bars", StringComparison.CurrentCultureIgnoreCase));

            if (abilitiesBarLayerIndex != -1)
                layers.Insert(abilitiesBarLayerIndex, new DoTariaInterfaceLayer(typeof(HeroStatisticsUIState).FullName, InterfaceScaleType.UI, HeroStatisticsUI, _heroStatisticsInterface));

            if (abilitiesBarLayerIndex != -1)
                layers.Insert(abilitiesBarLayerIndex, new DoTariaInterfaceLayer(typeof(AbilitiesUIState).FullName, InterfaceScaleType.UI, AbilitiesUI, _abilitiesInterface));
        }


        public override void UpdateUI(GameTime gameTime)
        {
            if (_abilitiesInterface != null && AbilitiesUI.Visible)
                AbilitiesUI.Update(gameTime);
        }

        // Taken from BiomesLibrary by TUA-Team.
        private void UpdateTickInterfaces()
        {
            if (PreviousMenuMode != Main.menuMode)
            {
                System.Diagnostics.Debug.WriteLine("Menu mode: " + Main.menuMode);
                PreviousMenuMode = Main.menuMode;
            }

            if (Main.menuMode == MENU_MODE_CHARACTER_SELECTION)
                HeroSelectionUIState.SelectedHero = null;

            if (Main.menuMode == MENU_MODE_CHARACTER_NAMING && PreviousMenuMode != MENU_MODE_HERO_SELECTION && HeroSelectionUIState.SelectedHero == null)
                SetMenuUIState(HeroSelectionUI);
        }

        private static void SetMenuUIState(UIState uiState)
        {
            Main.menuMode = MENU_MODE_HERO_SELECTION;
            Main.MenuUI.SetState(uiState);
        }


        public static int PreviousMenuMode { get; internal set; }

        internal AbilitiesUIState AbilitiesUI { get; private set; }
        internal HeroSelectionUIState HeroSelectionUI { get; private set; }
        internal HeroStatisticsUIState HeroStatisticsUI { get; private set; }
    }
}
