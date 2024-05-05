namespace BiggerBackpack;

public static class UI
{


    internal static bool SlotsCreated { get; set; }


    internal static bool ActionAttached { get; set; }


    internal static void UIHandler_OpenInventory()
    {
        // UpdateNavigationElements();
    }


    internal static void UIHandler_CloseInventory()
    {
        // UpdateNavigationElements();
    }

    // internal static void MakeGridScrollable(GridLayoutGroup existingGridLayoutGroup)
    // {
    //     
    //     items.SetParent(newViewPort.transform, true);
    //     
    //
    //     var existingScrollRect = existingScrollView.GetComponent<ScrollRect>();
    //     // Move the GridLayoutGroup to be a child of ScrollRect
    //     existingGridLayoutGroup.transform.SetParent(newScrollView.transform, true);
    //
    //
    //     var existingScrollbar = GameObject.Find("Player(Clone)/UI/Inventory/Encylopedia/Encylopedia Panels/Crops Panel/Scrollbar Vertical");
    //
    //     var newScrollbar = Object.Instantiate(existingScrollbar, mainParent.transform);
    //     // Link the Scrollbar to the ScrollRect
    //     existingScrollRect.verticalScrollbar = newScrollbar.GetComponent<Scrollbar>();
    //
    //     // LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRectRT);
    // }

//Player(Clone)/UI/Inventory/Items/Slots/EquipItems
    public static void CreateSlots(Inventory inventory, int count)
    {
        var parent = GameObject.Find("Player(Clone)/UI/Inventory/Items/Slots/EquipItems");
        var templateSlot = inventory._slots[20];
        if (templateSlot == null)
        {
            Plugin.LOG.LogError($"Could not find slot for to duplicate. Please report this.");
            return;
        }

        for (var i = 1; i <= count; i++)
        {
            var newSlot = Object.Instantiate(templateSlot, parent.transform);
            if (newSlot == null)
            {
                Plugin.LOG.LogError($"Failed to instantiate Slot ({i}). Please report this.");
                continue;
            }

            newSlot.name = $"Slot ({i})";
            // Patches.GearSlots.Add(newSlot);
            inventory._slots = inventory._slots.Append(newSlot).ToArray();
            inventory.maxSlots++;
        }
    }

}