namespace Rebirth;

public sealed class RebirthItem : CustomInventoryItem
{
    public override Sprite InventoryIcon { get; } = TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "assets", "rebirth_item.png"));
    public override Sprite Sprite { get; } = TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "assets", "rebirth_item.png"));


    public override string LocalizedDescription()
    {
        return "A special token obtained while on crusades that are used as currency to Rebirth followers.";
    }

    public override string InternalName => "REBIRTH_ITEM";

    public override bool AddItemToDungeonChests => true;
    public override int DungeonChestSpawnChance => 5;
    public override int DungeonChestMinAmount => 4;
    public override int DungeonChestMaxAmount => 7;
    
    public override Vector3 LocalScale { get; } = new(0.5f, 0.5f, 0.5f);
    public override InventoryItem.ITEM_TYPE ItemPickUpToImitate => InventoryItem.ITEM_TYPE.BLACK_GOLD;
    public override CustomItemManager.ItemRarity Rarity => CustomItemManager.ItemRarity.RARE;
    public override bool AddItemToOfferingShrine => true;

    public override bool CanBeRefined => true;
    public override InventoryItem.ITEM_TYPE RefineryInput => InventoryItem.ITEM_TYPE.BONE;
    public override int RefineryInputQty => 15;
    
    public override float CustomRefineryDuration => 256f;

    public override string LocalizedName() => "Rebirth Token";

    public override string LocalizedLore() => "Said to be dropped by Death herself.";


    public override InventoryItem.ITEM_CATEGORIES ItemCategory => InventoryItem.ITEM_CATEGORIES.COINS;
    public override bool IsCurrency => true;
}