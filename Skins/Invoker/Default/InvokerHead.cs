﻿using Terraria.ID;
using Terraria.ModLoader;

namespace DoTaria.Skins.Invoker.Default
{
    [AutoloadEquip(EquipType.Head)]
    public sealed class InvokerHead : SkinHeadItem
    {
        public InvokerHead() : base("Invoker Head", "", 24, 24, ItemRarityID.White)
        {
        }
    }
}