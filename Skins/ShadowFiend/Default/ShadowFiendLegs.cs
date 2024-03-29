﻿using DoTaria.Dusts;
using DoTaria.Dusts.Heroes.ShadowFiend.Default;
using DoTaria.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DoTaria.Skins.ShadowFiend.Default
{
    [AutoloadEquip(EquipType.Legs)]
    public sealed class ShadowFiendLegs : SkinLegsItem, ISpawnDustOnPlayerPostUpdate
    {
        public ShadowFiendLegs() : base("Shadow Fiend Legs", "", 14, 20, ItemRarityID.White)
        {
        }

        public void SpawnDustOnPlayerPostUpdate(DoTariaPlayer dotariaPlayer)
        {
            Vector2 dustPosition = new Vector2(dotariaPlayer.player.position.X + 2, dotariaPlayer.player.position.Y + 37);

            Dust.NewDust(dustPosition, 18, 10, mod.DustType<ShadowTrail>(), 0f, 0.8f, 0, new Color(255, 255, 255), 2.4f);
        }
    }
}
