namespace GerrysJunkTrunk;

public partial class Plugin
{
    private static void UpdateShippingBox(CraftDefinition sbCraft, WorldGameObject shippingBoxInstance = null)
    {
        if (!InternalShippingBoxBuilt.Value || _shippingBox != null) return;

        _shippingBox = shippingBoxInstance ? shippingBoxInstance : FindObjectsOfType<WorldGameObject>(true)
            .FirstOrDefault(x => string.Equals(x.custom_tag, ShippingBoxTag));

        if (_shippingBox == null)
        {
            if (Debug.Value)
            {
                Log.LogInfo("UpdateShippingBox: No Shipping Box Found!");
            }
            InternalShippingBoxBuilt.Value = false;
            sbCraft.hidden = false;
        }
        else
        {
            if (Debug.Value)
            {
                Log.LogInfo($"UpdateShippingBox: Found Shipping Box at {_shippingBox.pos3}");
            }
            InternalShippingBoxBuilt.Value = true;
            _shippingBox.data.drop_zone_id = ShippingBoxTag;

            var invSize = UnlockedShippingBoxExpansion() ? LargeInvSize : SmallInvSize;
            _shippingBox.data.SetInventorySize(invSize);

            sbCraft.hidden = true;
        }
    }

}