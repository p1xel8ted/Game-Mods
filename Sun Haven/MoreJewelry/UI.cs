using Sirenix.Serialization.Utilities;
using Object = UnityEngine.Object;

namespace MoreJewelry;

/// <summary>
/// Provides static methods and properties for managing the user interface (UI) elements.
/// </summary>
/// <remarks>
/// The <c>UI</c> class is responsible for creating, updating, and managing various UI components such as buttons, panels, and popups.
/// It includes methods for setting up navigation elements, updating UI states, and handling user interactions.
/// This class is static and cannot be instantiated.
/// </remarks>
public static class UI
{
    /// <summary>
    /// Gets or privately sets the instance of the left arrow GameObject.
    /// </summary>
    internal static GameObject ToggleArrowInstance { get; private set; }

    private static Button ToggleArrowButton { get; set; }

    /// <summary>
    /// Gets or privately sets the instance of the GearPanel GameObject.
    /// </summary>
    internal static GameObject GearPanel { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the slots have been created.
    /// </summary>
    internal static bool SlotsCreated { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether an action has been attached.
    /// </summary>
    internal static bool ActionAttached { get; set; }

    internal static GameObject OriginalKeepsakeSlot { get; set; }
    internal static GameObject OriginalAmuletSlot { get; set; }
    internal static Transform InventoryTransform { get; set; }

    /// <summary>
    /// Gets or privately sets the Popup component associated with the controller-safe toggle arrow.
    /// </summary>
    internal static Popup ToggleArrowPopup { get; private set; }


    /// <summary>
    /// Sets up the persistent toggle button used to show and hide the jewelry panel.
    /// </summary>
    /// <param name="button">The button to configure.</param>
    private static void SetupButton(Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            var panelVisible = !GearPanel.activeSelf;
            GearPanel.SetActive(panelVisible);
            UpdateToggleArrowVisual(panelVisible);
            Utils.UpdatePlayerPref();
            RefreshPanelLayout();
            UpdateNavigationElements();
        });
    }

    internal static Transform TitlePanel;

    /// <summary>
    /// Initializes the gear panel by finding and configuring the necessary GameObjects.
    /// </summary>
    /// <remarks>
    /// This method performs several operations:
    /// <list type="bullet">
    /// <item><description>Finds and clones the slide-out panel GameObject.</description></item>
    /// <item><description>Adjusts the size and position of the panel.</description></item>
    /// <item><description>Removes unnecessary child GameObjects from the panel.</description></item>
    /// <item><description>Destroys the <c>EncylopdeiaSorting</c> component if it exists.</description></item>
    /// <item><description>Updates the title text and creates toggle arrows.</description></item>
    /// <item><description>Updates the panel visibility and navigation elements.</description></item>
    /// </list>
    /// Logs an error and exits if the necessary GameObjects are not found.
    /// </remarks>
    /// <seealso cref="UpdateTitleText"/>
    /// <seealso cref="CreateToggleArrows"/>
    /// <seealso cref="UpdatePanelVisibility"/>
    /// <seealso cref="UpdateNavigationElements"/>
    /// <seealso cref="Const"/>
    internal static void InitializeGearPanel()
    {
        // if (GearPanel) return;
      
        
        GearPanel = CustomUI.CreateBg();
        GearPanel.name = Const.GearPanelName;
        
        TitlePanel = CustomUI.CreateTitle(GearPanel.transform);
        TitlePanel.localPosition = Const.TitlePosition;
        TitlePanel.localScale = Const.TitleScale;
   

        // Find and destroy the EncyclopediaSorting component as we don't need it.
        var encyclopediaSorting = GearPanel.GetComponentInChildren<EncylopdeiaSorting>();
        if (encyclopediaSorting != null)
        {
            Object.Destroy(encyclopediaSorting);
        }

        UpdateTitleText();
        
        CreateToggleArrows();

        UpdatePanelVisibility();
        UpdateNavigationElements();
    }

    /// <summary>
    /// Called when the player opens the inventory. Linked to <c>UIHandler.OnOpenInventory</c>.
    /// </summary>
    /// <remarks>
    /// Activates the gear panel if it is not null. Logs the action and updates the panel visibility and navigation elements.
    /// <para>Relies on <see cref="GearPanel"/> to reference the gear panel GameObject.</para>
    /// <para>Calls <see cref="UpdatePanelVisibility"/> and <see cref="UpdateNavigationElements"/> for UI updates.</para>
    /// <para>Uses <see cref="Utils.Log"/> for logging.</para>
    /// </remarks>
    internal static void UIHandler_OpenInventory()
    {
        if (GearPanel == null) return;
        Utils.Log("Opening inventory. Activating gear panel.");
        GearPanel.SetActive(true);
        UpdatePanelVisibility();
        UpdateNavigationElements();
    }


    /// <summary>
    /// Called when the player closes the inventory. Linked to <c>UIHandler.OnCloseInventory</c>.
    /// </summary>
    /// <remarks>
    /// Deactivates the gear panel if it is not null. Logs the action and updates the panel visibility and navigation elements.
    /// <para>Relies on <see cref="GearPanel"/> to reference the gear panel GameObject.</para>
    /// <para>Calls <see cref="UpdatePanelVisibility"/> and <see cref="UpdateNavigationElements"/> for UI updates.</para>
    /// <para>Uses <see cref="Utils.Log"/> for logging.</para>
    /// </remarks>
    internal static void UIHandler_CloseInventory()
    {
        if (GearPanel == null) return;
        Utils.Log("Closing inventory. Deactivating gear panel.");
        GearPanel.SetActive(false);
        UpdatePanelVisibility();
        UpdateNavigationElements();
    }


    /// <summary>
    /// Creates or updates a Popup component on the specified parent GameObject.
    /// </summary>
    /// <param name="parent">The parent GameObject on which the Popup component is to be created or updated.</param>
    /// <param name="name">The name to assign to the Popup component.</param>
    /// <param name="content">The content (description) of the Popup.</param>
    /// <param name="title">The title of the Popup.</param>
    /// <returns>The created or updated Popup component.</returns>
    /// <remarks>
    /// If the parent GameObject does not already have a Popup component, this method adds a new Popup component to it.
    /// Otherwise, it updates the existing Popup component. The Popup's enabled state is set based on the <see cref="Plugin.ShowTooltips"/> value.
    /// </remarks>
    private static Popup CreatePopup(GameObject parent, string name, string content, string title)
    {
        var popup = parent.TryAddComponent<Popup>();
        popup.name = name;
        popup.description = content;
        popup.text = title;
        popup.enabled = Plugin.ShowTooltips.Value;
        return popup;
    }

    /// <summary>
    /// Creates and configures toggle arrow instances for navigation.
    /// </summary>
    /// <remarks>
    /// This method performs the following tasks:
    /// <list type="bullet">
    /// <item><description>Finds the left and right arrow GameObjects using paths defined in <see cref="Const"/>.</description></item>
    /// <item><description>Instantiates new arrow instances and attaches them to the specified parent GameObject.</description></item>
    /// <item><description>Configures buttons and popups for the arrow instances.</description></item>
    /// <item><description>Sets the position and names of the arrow instances based on constants from <see cref="Const"/>.</description></item>
    /// <item><description>Sets up button functionality using <see cref="SetupButton"/>.</description></item>
    /// </list>
    /// Logs an error using <see cref="Plugin.LOG"/> if the original left or right arrows are not found.
    /// Assumes that the parent is found dynamically via <see cref="InventoryTransform"/>.
    /// </remarks>
    private static void CreateToggleArrows()
    {
        if (InventoryTransform == null)
        {
            Plugin.LOG.LogError("Inventory transform not cached. Cannot create toggle arrows.");
            return;
        }

        var leftArrow = InventoryTransform.FindFirstChildByName("LeftArrow")?.gameObject;
        var rightArrow = InventoryTransform.FindFirstChildByName("RightArrow")?.gameObject;
        if (leftArrow == null || rightArrow == null)
        {
            Plugin.LOG.LogError("Left or right arrow not found. Please report this.");
            return;
        }

        var parent = InventoryTransform.FindFirstChildByName("Items")?.gameObject;
        if (parent == null)
        {
            Plugin.LOG.LogError("Items parent not found. Cannot create toggle arrows.");
            return;
        }

        // Keep a single arrow GameObject alive so controller selection never points
        // at an inactive button after toggling the panel.
        ToggleArrowInstance = Object.Instantiate(leftArrow, parent.transform);
        ToggleArrowButton = ToggleArrowInstance.GetComponent<Button>();

        // Fix duplicate PSS.UI.Selectable components from the cloned template.
        // Keep only one — multiple Selectables on the same GameObject confuse the input system.
        var pssSelectables = ToggleArrowInstance.GetComponents<PSS.UI.Selectable>();
        for (var i = 1; i < pssSelectables.Length; i++)
        {
            Object.DestroyImmediate(pssSelectables[i]);
        }

        // Add NavigationElement so the game's controller system (MouseVisualManager) tracks this element.
        ToggleArrowInstance.TryAddComponent<NavigationElement>();

        CreatePopups();

        //set the position of the arrows to the bottom left of the parent
        ToggleArrowInstance.transform.localPosition = Const.ArrowPosition;

        //set the name of the arrows
        ToggleArrowInstance.name = Const.MoreJewelryToggleArrow;

        SetupButton(ToggleArrowButton);
        UpdateToggleArrowVisual(PlayerPrefs.GetInt(Const.PlayerPrefKey, 1) == 1);
    }

    /// <summary>
    /// Creates popup components for the left and right arrow instances.
    /// </summary>
    /// <remarks>
    /// This method initializes popups for both the left and right arrow instances using specific content and titles.
    /// It relies on the <see cref="CreatePopup"/> method for creating each popup.
    /// <para>The content and titles for the popups are retrieved using <see cref="Utils.GetLeftArrowPopupContent"/>, 
    /// <see cref="Utils.GetRightArrowPopupContent"/>, and <see cref="Utils.GetTitle"/>.</para>
    /// <para>Assumes that <see cref="ToggleArrowInstance"/> is already instantiated.</para>
    /// </remarks>
    private static void CreatePopups()
    {
        ToggleArrowPopup = CreatePopup(ToggleArrowInstance, $"{ToggleArrowInstance.name}Popup", GetToggleArrowPopupContent(), Utils.GetTitle());
    }

    /// <summary>
    /// Updates the navigation elements for gear slots, arrow navigation, and original armor slots.
    /// </summary>
    /// <remarks>
    /// This method performs several key tasks:
    /// <list type="bullet">
    /// <item><description>Checks if the number of gear slots is as expected and logs an error if not.</description></item>
    /// <item><description>Initializes navigation elements for each gear slot.</description></item>
    /// <item><description>Finds and sets up navigation for the original keepsake and amulet slots.</description></item>
    /// <item><description>Updates arrow navigation elements based on the original armor slots and gear slots.</description></item>
    /// <item><description>Adjusts the left navigation of the original keepsake and amulet slots based on the current panel toggle settings.</description></item>
    /// </list>
    /// <para>Relies on <see cref="Patches.GearSlots"/>, <see cref="Plugin.ShowPanelToggle"/>, <see cref="Plugin.IgnoreToggleWithController"/>, and constants from <see cref="Const"/>.</para>
    /// <para>Uses <see cref="UpdateArrowNavigation"/> and <see cref="UpdateGearSlotNavigation"/> for navigation updates.</para>
    /// <para>Logs errors using <see cref="Plugin.LOG"/> if necessary components are not found or if the gear slots count is unexpected.</para>
    /// </remarks>
    internal static void UpdateNavigationElements()
    {
        if (Patches.GearSlots.Count != 6) return;

        Utils.Log("Updating navigation elements.");

        var gearSelectables = new Selectable[6];
        for (var i = 0; i < Patches.GearSlots.Count; i++)
        {
            gearSelectables[i] = Patches.GearSlots[i].gameObject.GetComponent<Selectable>() ?? Patches.GearSlots[i].gameObject.AddComponent<Selectable>();
            gearSelectables[i].transition = Selectable.Transition.None;
            gearSelectables[i].interactable = true;
        }

        if (OriginalKeepsakeSlot == null || OriginalAmuletSlot == null)
        {
            Plugin.LOG.LogError("Original keepsake/amulet slot references not cached. Skipping navigation update.");
            return;
        }

        var keepsakeSelectable = OriginalKeepsakeSlot.GetComponent<Selectable>() ?? OriginalKeepsakeSlot.AddComponent<Selectable>();
        var amuletSelectable = OriginalAmuletSlot.GetComponent<Selectable>() ?? OriginalAmuletSlot.AddComponent<Selectable>();

        Selectable arrowSelectable = null;
        if (ToggleArrowInstance != null)
        {
            arrowSelectable = ToggleArrowInstance.GetComponent<Selectable>() ?? ToggleArrowInstance.AddComponent<Selectable>();
            arrowSelectable.transition = Selectable.Transition.None;
        }

        // Arrow navigation: left arrow → gear slots, right arrow → original slots
        if (arrowSelectable != null)
        {
            SetExplicitNav(arrowSelectable, right: keepsakeSelectable, left: gearSelectables[3]);
        }

        // Gear slots laid out as:
        // 0  1
        // 2  3
        // 4  5
        var rightNeighbor = Plugin.ShowPanelToggle.Value && !Plugin.IgnoreToggleWithController.Value
            ? arrowSelectable ?? keepsakeSelectable
            : keepsakeSelectable;

        SetExplicitNav(gearSelectables[0], right: gearSelectables[1], down: gearSelectables[2], left: keepsakeSelectable);
        SetExplicitNav(gearSelectables[1], right: rightNeighbor, down: gearSelectables[3], left: gearSelectables[0]);
        SetExplicitNav(gearSelectables[2], right: gearSelectables[3], down: gearSelectables[4], up: gearSelectables[0], left: amuletSelectable);
        SetExplicitNav(gearSelectables[3], right: rightNeighbor, down: gearSelectables[5], left: gearSelectables[2], up: gearSelectables[1]);
        SetExplicitNav(gearSelectables[4], right: gearSelectables[5], up: gearSelectables[2], left: keepsakeSelectable);
        SetExplicitNav(gearSelectables[5], right: rightNeighbor, left: gearSelectables[4], up: gearSelectables[3]);

        // Wire original slots back to gear panel
        if (Plugin.ShowPanelToggle.Value && !Plugin.IgnoreToggleWithController.Value && arrowSelectable != null)
        {
            SetSelectableLeft(keepsakeSelectable, arrowSelectable);
            SetSelectableLeft(amuletSelectable, arrowSelectable);
        }
        else
        {
            SetSelectableLeft(keepsakeSelectable, gearSelectables[1]);
            SetSelectableLeft(amuletSelectable, gearSelectables[3]);
        }
    }

    private static void SetExplicitNav(Selectable selectable, Selectable right = null, Selectable down = null, Selectable left = null, Selectable up = null)
    {
        var nav = new Navigation { mode = Navigation.Mode.Explicit };
        if (right != null) nav.selectOnRight = right;
        if (down != null) nav.selectOnDown = down;
        if (left != null) nav.selectOnLeft = left;
        if (up != null) nav.selectOnUp = up;
        selectable.navigation = nav;
    }

    private static void SetSelectableLeft(Selectable selectable, Selectable left)
    {
        var nav = selectable.navigation;
        nav.selectOnLeft = left;
        selectable.navigation = nav;
    }

    private static string GetToggleArrowPopupContent()
    {
        return GearPanel != null && GearPanel.activeSelf
            ? Utils.GetRightArrowPopupContent()
            : Utils.GetLeftArrowPopupContent();
    }

    private static void UpdateToggleArrowVisual(bool panelVisible)
    {
        if (ToggleArrowInstance != null)
        {
            ToggleArrowInstance.SetActive(Plugin.ShowPanelToggle.Value);
        }

        if (ToggleArrowInstance != null)
        {
            // Left arrow = 270° Z, Right arrow = 90° Z (same sprite, different rotation)
            var zRotation = panelVisible ? 90f : 270f;
            ToggleArrowInstance.transform.localEulerAngles = new Vector3(0f, 0f, zRotation);
        }

        if (ToggleArrowPopup != null)
        {
            ToggleArrowPopup.text = Utils.GetTitle();
            ToggleArrowPopup.description = GetToggleArrowPopupContent();
            ToggleArrowPopup.enabled = Plugin.ShowTooltips.Value;
        }
    }

    private static void RefreshPanelLayout()
    {
        Canvas.ForceUpdateCanvases();

        if (GearPanel != null && GearPanel.TryGetComponent<RectTransform>(out var gearPanelRect))
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(gearPanelRect);
        }

        if (GearPanel?.transform.Find(Const.GridContainerViewportContent) is RectTransform contentRect)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
        }

        if (GearPanel?.transform.Find("SlotGridContainer/Viewport") is RectTransform viewportRect)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(viewportRect);
        }

        Canvas.ForceUpdateCanvases();
    }


    /// <summary>
    /// Updates the visibility of the panel and arrow instances based on the current settings.
    /// </summary>
    /// <remarks>
    /// The method checks the value of <see cref="Plugin.ShowPanelToggle"/> to determine the visibility state.
    /// <para>If <see cref="Plugin.ShowPanelToggle"/> is false, the panel is made visible, and the toggle buttons are hidden. 
    /// The left arrow button's onClick event is invoked if the button is not null.</para>
    /// <para>If <see cref="Plugin.ShowPanelToggle"/> is true, the panel's visibility is set based on the player's preferences stored in PlayerPrefs.
    /// The left and right arrow instances are toggled accordingly.</para>
    /// <para>Uses <see cref="Utils.Log"/> for logging and <see cref="PlayerPrefs.SetInt"/> and <see cref="PlayerPrefs.GetInt"/> for storing and retrieving visibility state.</para>
    /// <para>Relies on <see cref="Const.PlayerPrefKey"/> for the PlayerPrefs key.</para>
    /// Assumes that <see cref="ToggleArrowInstance"/> and <see cref="GearPanel"/> are already instantiated and accessible.
    /// </remarks>
    internal static void UpdatePanelVisibility()
    {
        //toggle is false, so we need to ensure the panel is visible and in the shown location
        if (!Plugin.ShowPanelToggle.Value)
        {
            Utils.Log("ShowPanelToggle is false. Setting panel to visible & hiding toggle button.");
            PlayerPrefs.SetInt(Const.PlayerPrefKey, 0);
            if (GearPanel != null) GearPanel.SetActive(true);
            if (ToggleArrowInstance != null) ToggleArrowInstance.SetActive(false);
        }
        else
        {
            Utils.Log("ShowPanelToggle is true. Setting panel based on settings. Showing toggle button.");
            var panelVisible = PlayerPrefs.GetInt(Const.PlayerPrefKey, 1) == 1;
            GearPanel.SetActive(panelVisible);
            UpdateToggleArrowVisual(panelVisible);
        }

        RefreshPanelLayout();
        UpdatePopupText();
    }

    /// <summary>
    /// Creates a specified number of slots for a given armor type in the inventory.
    /// </summary>
    /// <param name="inventory">The inventory where slots are to be created.</param>
    /// <param name="armorType">The type of armor for which slots are to be created.</param>
    /// <param name="count">The number of slots to create.</param>
    /// <remarks>
    /// This method duplicates a template slot based on the specified armor type and adds the new slots to the inventory.
    /// <para>It first finds a template slot in the inventory that accepts the specified armor type. If no such slot is found, logs an error and returns.</para>
    /// <para>Each new slot is instantiated from the template, named appropriately, and added to both the inventory and the <see cref="Patches.GearSlots"/> list in <see cref="Patches"/>.</para>
    /// <para>If a slot fails to instantiate, an error is logged and the method continues with the next slot.</para>
    /// <para>Uses <see cref="GetOrCreateGridContainer"/> to ensure there is a container for the new slots.</para>
    /// <para>Relies on <see cref="Plugin.LOG"/> for logging errors.</para>
    /// </remarks>
    private static readonly int[] CustomSlotNumbers =
    [
        Const.NewRingSlotOne, Const.NewRingSlotTwo,
        Const.NewKeepsakeSlotOne, Const.NewKeepsakeSlotTwo,
        Const.NewAmuletSlotOne, Const.NewAmuletSlotTwo
    ];

    public static void CreateSlots(Inventory inventory, ArmorType armorType, int count)
    {
        var templateSlot = inventory._slots.FirstOrDefault(slot => slot.acceptableArmorType == armorType);
        if (templateSlot == null)
        {
            Plugin.LOG.LogError($"Could not find slot for {armorType}. Please report this.");
            return;
        }

        var gridContainer = GetOrCreateGridContainer();

        for (var i = 1; i <= count; i++)
        {
            var newSlot = Object.Instantiate(templateSlot, gridContainer.transform);
            if (newSlot == null)
            {
                Plugin.LOG.LogError($"Failed to instantiate {armorType}Slot ({i}). Please report this.");
                continue;
            }

            // Destroy cloned ItemIcon children — they carry stale references to the
            // original slot and corrupt the global CurrentItemIcon drag/drop state.
            // The game creates fresh ItemIcons when items are placed in slots.
            foreach (var itemIcon in newSlot.GetComponentsInChildren<ItemIcon>(true))
            {
                Object.DestroyImmediate(itemIcon.gameObject);
            }

            // The template slot's Selectable.interactable may be false (set by ItemIcon when
            // an item occupies the slot). DestroyImmediate above skips ItemIcon's cleanup that
            // would reset it to true. Empty slots need interactable=true so controller can reach them.
            var slotSelectable = newSlot.GetComponent<Selectable>();
            if (slotSelectable != null) slotSelectable.interactable = true;

            var slotIndex = Patches.GearSlots.Count;
            newSlot.name = $"{armorType}Slot ({i})";
            newSlot.slotNumber = slotIndex < CustomSlotNumbers.Length ? CustomSlotNumbers[slotIndex] : 66 + slotIndex;
            Patches.GearSlots.Add(newSlot);
        }

        RefreshPanelLayout();
    }

    /// <summary>
    /// Retrieves an existing grid container GameObject from the GearPanel or creates a new one if it doesn't exist.
    /// </summary>
    /// <returns>The existing or newly created grid container GameObject.</returns>
    /// <remarks>
    /// Searches for a child GameObject of GearPanel with the name specified in <see cref="Const.GridContainerName"/>.
    /// If such a GameObject is found, it is returned. Otherwise, a new GameObject with that name is created,
    /// configured with a RectTransform and GridLayoutGroup, and then returned.
    /// <para>The GridLayoutGroup is configured with specific cell size, spacing, constraint, constraint count, child alignment, and padding.</para>
    /// Assumes that <see cref="GearPanel"/> is already instantiated and accessible.
    /// </remarks>
    private static GameObject GetOrCreateGridContainer()
    {
        var existingContainer = GearPanel.transform.Find(Const.GridContainerViewportContent);
        if (existingContainer != null)
        {
            return existingContainer.gameObject;
        }

        var newContainer = CustomUI.CreateScrollView(GearPanel.transform);
        return newContainer;


        //
        // var gridContainer = new GameObject(Const.GridContainerName);
        // gridContainer.AddComponent<RectTransform>();
        // gridContainer.transform.SetParent(GearPanel.transform, false);
        //
        // var gridLayout = gridContainer.AddComponent<GridLayoutGroup>();
        // gridLayout.cellSize = new Vector2(32, 32);
        // gridLayout.spacing = new Vector2(10, 10);
        // gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        // gridLayout.constraintCount = 2;
        // gridLayout.childAlignment = TextAnchor.MiddleCenter;
        // gridLayout.padding = new RectOffset(5, 0, 20, 0);
        // gridLayout.gameObject.layer = LayerMask.NameToLayer("UI");
        //
        // return gridContainer;
    }

    /// <summary>
    /// Updates the text and description of the left and right arrow popups.
    /// </summary>
    /// <remarks>
    /// Sets the text of both popups to the title obtained from <see cref="Utils.GetTitle"/>.
    /// Sets the description of the left arrow popup using <see cref="Utils.GetLeftArrowPopupContent"/>
    /// and the right arrow popup using <see cref="Utils.GetRightArrowPopupContent"/>.
    /// Does nothing if either <see cref="LeftArrowPopup"/> or <see cref="RightArrowPopup"/> is null.
    /// </remarks>
    public static void UpdatePopupText()
    {
        if (ToggleArrowPopup == null) return;
        ToggleArrowPopup.text = Utils.GetTitle();
        ToggleArrowPopup.description = GetToggleArrowPopupContent();
    }

    /// <summary>
    /// Updates the title text of the GearPanel.
    /// </summary>
    /// <param name="textOnly">If set to true, only the text is updated; otherwise, font size, position, color, and name are also set.</param>
    /// <remarks>
    /// Retrieves the title text from <see cref="Utils.GetTitle"/> and applies it to the title TextMeshProUGUI component of the GearPanel.
    /// If <paramref name="textOnly"/> is false, also adjusts font size, position, color, and name based on values from the <see cref="Const"/> class.
    /// Does nothing if the title TextMeshProUGUI component is not found in GearPanel's children.
    /// </remarks>
    public static void UpdateTitleText(bool textOnly = false)
    {
        var titleText = CustomUI.CreateTitleText(TitlePanel.transform, Utils.GetTitle());
        if (titleText == null) return;

        if (textOnly)
        {
            titleText.text = Utils.GetTitle();
            return;
        }

        titleText.text = Utils.GetTitle();
        titleText.fontSizeMin = 16;
        titleText.fontSizeMax = 20;
        titleText.fontSize = 16;
        // titleText.transform.localPosition = Const.TitlePosition;
        titleText.color = Const.TitleTextColor;
        titleText.name = Const.TitleTextName;
    }
}
