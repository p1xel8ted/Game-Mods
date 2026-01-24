namespace Rebirth;

public class MissionItem: CustomMission
{
    public override string InternalName => "REBIRTH_MISSION_1";

    public override InventoryItem.ITEM_TYPE RewardType => Plugin.RebirthItem;

    public override int BaseChance => Plugin.MissionBaseChance.Value;

    public override IntRange RewardRange => new(Plugin.MissionRewardMin.Value, Plugin.MissionRewardMax.Value);
}