namespace GerrysJunkTrunk;

public partial class Plugin
{
    private void Update()
    {
        if (!MainGame.game_started) return;

        _techCount = MainGame.me.save.unlocked_techs.Count;
        if (_techCount > _oldTechCount)
        {
            _oldTechCount = _techCount;
            CheckShippingBox();
        }

        if (InternalShowIntroMessage.Value)
        {
            ShowIntroMessage();
            InternalShowIntroMessage.Value = false;
        }

        if (!UnlockedShippingBox()) return;
        var sbCraft = GameBalance.me.GetData<ObjectCraftDefinition>(ShippingBoxId);
        if (InternalShippingBoxBuilt.Value && _shippingBox == null)
        {
            _shippingBox = FindObjectsOfType<WorldGameObject>(true)
                .FirstOrDefault(x => string.Equals(x.custom_tag, ShippingBoxTag));
            if (_shippingBox == null)
            {
                Log.LogWarning("Update: No Shipping Box Found!");
                InternalShippingBoxBuilt.Value = false;
                sbCraft.hidden = false;
            }
            else
            {
                Log.LogWarning($"Update: Found Shipping Box at {_shippingBox.pos3}");
                InternalShippingBoxBuilt.Value = true;
                _shippingBox.data.drop_zone_id = ShippingBoxTag;

                var invSize = SmallInvSize;
                if (UnlockedShippingBoxExpansion())
                {
                    invSize = LargeInvSize;
                }

                _shippingBox.data.SetInventorySize(invSize);


                sbCraft.hidden = true;
            }
        }
    }

    private static void UpdateShippingBox(CraftDefinition sbCraft, WorldGameObject shippingBoxInstance = null)
    {
        if (!InternalShippingBoxBuilt.Value || _shippingBox != null) return;

        _shippingBox = shippingBoxInstance ? shippingBoxInstance : FindObjectsOfType<WorldGameObject>(true)
            .FirstOrDefault(x => string.Equals(x.custom_tag, ShippingBoxTag));

        if (_shippingBox == null)
        {
            Log.LogWarning("UpdateShippingBox: No Shipping Box Found!");
            InternalShippingBoxBuilt.Value = false;
            sbCraft.hidden = false;
        }
        else
        {
            Log.LogWarning($"UpdateShippingBox: Found Shipping Box at {_shippingBox.pos3}");
            InternalShippingBoxBuilt.Value = true;
            _shippingBox.data.drop_zone_id = ShippingBoxTag;

            var invSize = UnlockedShippingBoxExpansion() ? LargeInvSize : SmallInvSize;
            _shippingBox.data.SetInventorySize(invSize);

            sbCraft.hidden = true;
        }
    }

}