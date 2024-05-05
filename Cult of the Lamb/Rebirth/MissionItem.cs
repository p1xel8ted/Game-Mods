namespace Rebirth;

public class MissionItem: CustomMission
{
    public override string InternalName => "REBIRTH_MISSION_1";

    public override InventoryItem.ITEM_TYPE RewardType => Plugin.RebirthItem;

    public override int BaseChance => 50;
    
    public override IntRange RewardRange => new(15, 25);
}