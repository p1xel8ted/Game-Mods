namespace BiggerBackpack;

/// <summary>
/// Contains Harmony patches for modifying and extending the functionality of various game methods.
/// </summary>
[Harmony]
[HarmonyPriority(1)]
public static class Patches
{
    [UsedImplicitly] public static List<Slot> GearSlots = [];


[HarmonyPrefix]
[HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.LoadPlayerInventory))]
private static void PlayerInventory_Initialize(PlayerInventory __instance)
{
    if (UI.SlotsCreated)
    {
        Utils.Log("Slots already created. Skipping slot creation etc.");
        return;
    }

    UI.CreateSlots(__instance, 10);

    var slots = GameObject.Find("Player(Clone)/UI/Inventory/Items/Slots");
    var equipItems = GameObject.Find("Player(Clone)/UI/Inventory/Items/Slots/EquipItems");

    var mainParent = GameObject.Find(Const.PlayerItemsPath);

    var existingScrollView = GameObject.Find("Player(Clone)/UI/Inventory/Encylopedia/Encylopedia Panels/Crops Panel/Scroll View");

    var newScrollView = Object.Instantiate(existingScrollView, mainParent.transform);
    newScrollView.name = "PlayerItems";
    var newScrollRect = newScrollView.GetComponent<ScrollRect>();
    

    
    newScrollRect.content = equipItems.GetComponent<RectTransform>();
    var newScrollRectRT = newScrollRect.GetComponent<RectTransform>();
    

    // Ensure the viewport is properly set up for masking
    var newViewPort = newScrollView.transform.Find("Viewport");
    if (newViewPort != null)
    {
        // Add a Mask component if it doesn't exist
        var mask = newViewPort.gameObject.GetComponent<Mask>() ?? newViewPort.gameObject.AddComponent<Mask>();
        mask.showMaskGraphic = false;
    
        // Add an Image component for the mask to work (Mask requires an Image on the same GameObject)
        var image = newViewPort.gameObject.GetComponent<Image>() ?? newViewPort.gameObject.AddComponent<Image>();
        image.color = new Color(0, 0, 0, 0); // Transparent image
    }

    // Clear existing children in the viewport
    for (var i = 0; i < newViewPort.transform.childCount; i++)
    {
        Object.Destroy(newViewPort.transform.GetChild(i).gameObject);
    }
    equipItems.transform.SetParent(newViewPort.transform, true);

    var existingScrollbar = GameObject.Find("Player(Clone)/UI/Inventory/Encylopedia/Encylopedia Panels/Crops Panel/Scrollbar Vertical");
    var newScrollbar = Object.Instantiate(existingScrollbar, mainParent.transform);
    newScrollRect.verticalScrollbar = newScrollbar.GetComponent<Scrollbar>();

    newScrollView.transform.SetParent(slots.transform, true);
    
    newViewPort.GetComponent<RectTransform>().sizeDelta = new Vector2(0, -65f);
    __instance.SetUpInventoryData();
    UI.SlotsCreated = true;
}


    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.EnableMenu))]
    private static void MainMenuController_EnableMenu(ref MainMenuController __instance, ref GameObject menu)
    {
        if (menu == __instance.homeMenu || menu == __instance.newCharacterMenu || menu == __instance.loadCharacterMenu || menu == __instance.singlePlayerMenu || menu == __instance.multiplayerMenu)
        {
            Utils.ResetMod();
        }
    }


}