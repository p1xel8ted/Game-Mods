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
    internal static GameObject LeftArrowInstance { get; private set; }

    /// <summary>
    /// Gets or sets the Button component associated with the LeftArrowInstance.
    /// </summary>
    private static Button LeftArrowInstanceButton { get; set; }

    /// <summary>
    /// Gets or sets the instance of the right arrow GameObject.
    /// </summary>
    private static GameObject RightArrowInstance { get; set; }

    /// <summary>
    /// Gets or sets the Button component associated with the RightArrowInstance.
    /// </summary>
    private static Button RightArrowInstanceButton { get; set; }

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

    /// <summary>
    /// Gets or privately sets the Popup component associated with the LeftArrowInstance.
    /// </summary>
    internal static Popup LeftArrowPopup { get; private set; }

    /// <summary>
    /// Gets or privately sets the Popup component associated with the RightArrowInstance.
    /// </summary>
    internal static Popup RightArrowPopup { get; private set; }


    /// <summary>
    /// Sets up a button with specific behavior on click. When the button is clicked, it shows one GameObject and hides another.
    /// Additionally, it toggles the visibility of the GearPanel based on the state of the LeftArrowInstance and updates player preferences and navigation elements.
    /// </summary>
    /// <param name="button">The button to set up.</param>
    /// <param name="buttonToShow">The GameObject to show when the button is clicked.</param>
    /// <param name="buttonToHide">The GameObject to hide when the button is clicked.</param>
    /// <remarks>
    /// This method removes all existing listeners from the button's onClick event before adding the new behavior.
    /// It assumes that GearPanel and LeftArrowInstance are accessible within the scope of the method.
    /// Calls <see cref="Utils.UpdatePlayerPref"/> to update player preferences and <see cref="UpdateNavigationElements"/> to update navigation elements.
    /// </remarks>
    private static void SetupButton(Button button, GameObject buttonToShow, GameObject buttonToHide)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            buttonToShow.SetActive(true);
            buttonToHide.SetActive(false);
            GearPanel.SetActive(!LeftArrowInstance.activeSelf);
            Utils.UpdatePlayerPref();
            UpdateNavigationElements();
        });
    }

    
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
        var slideOutPanel = GameObject.Find(Const.EncyclopediaPanelPath);
        var slideOutPanelParent = GameObject.Find(Const.PlayerItemsPath);
        if (slideOutPanel == null)
        {
            Plugin.LOG.LogError("'Encyclopedia' panel not found. Please report this.");
            return;
        }
        if (slideOutPanelParent == null)
        {
            Plugin.LOG.LogError("'player item' panel not found. Please report this.");
            return;
        }


        GearPanel = Object.Instantiate(slideOutPanel, slideOutPanelParent.transform);
        GearPanel.name = Const.GearPanelName;

        var rectTransform = GearPanel.GetComponent<RectTransform>();
        var sizeDelta = rectTransform.sizeDelta;

        sizeDelta -= new Vector2(10, 0);
        sizeDelta += new Vector2(0, 10);

        rectTransform.sizeDelta = sizeDelta;


        // Destroy every child except the first one (the title text) as we don't need them
        for (var i = 1; i < GearPanel.transform.childCount; i++)
        {
            Object.Destroy(GearPanel.transform.GetChild(i).gameObject);
        }

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
        var popup = parent.GetComponent<Popup>();
        if (popup == null)
        {
            popup = parent.AddComponent<Popup>();
        }
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
    /// Assumes that the parent GameObject is found using <see cref="Const.PlayerItemsPath"/>.
    /// </remarks>
    private static void CreateToggleArrows()
    {
        var leftArrow = GameObject.Find(Const.LeftArrowPath);
        var rightArrow = GameObject.Find(Const.RightArrowPath);
        if (leftArrow == null || rightArrow == null)
        {
            Plugin.LOG.LogError("Left or right arrow not found. Please report this.");
            return;
        }
        var parent = GameObject.Find(Const.PlayerItemsPath);

        //create new instances of the arrows and attach them to the parent
        LeftArrowInstance = Object.Instantiate(leftArrow, parent.transform);
        RightArrowInstance = Object.Instantiate(rightArrow, parent.transform);

        LeftArrowInstanceButton = LeftArrowInstance.GetComponent<Button>();
        RightArrowInstanceButton = RightArrowInstance.GetComponent<Button>();

        CreatePopups();

        LeftArrowInstance.SetActive(true);
        RightArrowInstance.SetActive(false);

        //set the position of the arrows to the bottom left of the parent
        LeftArrowInstance.transform.localPosition = Const.ArrowPosition;
        RightArrowInstance.transform.localPosition = Const.ArrowPosition;

        //set the name of the arrows
        LeftArrowInstance.name = Const.MoreJewelryLeftArrow;
        RightArrowInstance.name = Const.MoreJewelryRightArrow;

        SetupButton(LeftArrowInstanceButton, RightArrowInstance, LeftArrowInstance);
        SetupButton(RightArrowInstanceButton, LeftArrowInstance, RightArrowInstance);

        LeftArrowInstance.SetActive(false);
        RightArrowInstance.SetActive(true);
    }

    /// <summary>
    /// Creates popup components for the left and right arrow instances.
    /// </summary>
    /// <remarks>
    /// This method initializes popups for both the left and right arrow instances using specific content and titles.
    /// It relies on the <see cref="CreatePopup"/> method for creating each popup.
    /// <para>The content and titles for the popups are retrieved using <see cref="Utils.GetLeftArrowPopupContent"/>, 
    /// <see cref="Utils.GetRightArrowPopupContent"/>, and <see cref="Utils.GetTitle"/>.</para>
    /// <para>Assumes that <see cref="LeftArrowInstance"/> and <see cref="RightArrowInstance"/> are already instantiated.</para>
    /// </remarks>
    private static void CreatePopups()
    {
        LeftArrowPopup = CreatePopup(LeftArrowInstance, $"{LeftArrowInstance.name}Popup", Utils.GetLeftArrowPopupContent(), Utils.GetTitle());
        RightArrowPopup = CreatePopup(RightArrowInstance, $"{RightArrowInstance.name}Popup", Utils.GetRightArrowPopupContent(), Utils.GetTitle());
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
        if (Patches.GearSlots.Count != 6)
        {
            return;
        }

        Utils.Log("Updating navigation elements.");

        // Initialize NavigationElements for GearSlots
        var gearSlotNavs = new NavigationElement[6];
        for (var i = 0; i < Patches.GearSlots.Count; i++)
        {
            gearSlotNavs[i] = Patches.GearSlots[i].gameObject.GetComponent<NavigationElement>() ?? Patches.GearSlots[i].gameObject.AddComponent<NavigationElement>();
        }

        var originalKeepsakeSlot = GameObject.Find(Const.AmorSlotTen).GetComponent<NavigationElement>();
        if (originalKeepsakeSlot == null)
        {
            Plugin.LOG.LogError("Unable to locate original keepsake slot. Skipping navigation element update. Please report this.");
            return;
        }

        var originalAmuletSlot = GameObject.Find(Const.AmorSlotEleven).GetComponent<NavigationElement>();
        if (originalAmuletSlot == null)
        {
            Plugin.LOG.LogError("Unable to locate original amulet slot. Skipping navigation element update. Please report this.");
            return;
        }

        var leftArrowNav = LeftArrowInstance.GetComponents<NavigationElement>();
        var rightArrowNav = RightArrowInstance.GetComponents<NavigationElement>();
        UpdateArrowNavigation(leftArrowNav, originalKeepsakeSlot, gearSlotNavs[3]);
        UpdateArrowNavigation(rightArrowNav, originalKeepsakeSlot, gearSlotNavs[3]);

        // Update navigation for each gear slot
        UpdateGearSlotNavigation(gearSlotNavs, originalKeepsakeSlot, rightArrowNav);

        if (Plugin.ShowPanelToggle.Value)
        {
            // if (Plugin.IgnoreToggleWithController.Value)
            // {
            //     originalKeepsakeSlot.left = gearSlotNavs[3];
            //     originalAmuletSlot.left = gearSlotNavs[5];
            // }
            // else
            // {
            //     originalKeepsakeSlot.left = GearPanel.activeSelf ? rightArrowNav[0] : leftArrowNav[0];
            //     originalAmuletSlot.left = GearPanel.activeSelf ? rightArrowNav[0] : leftArrowNav[0];
            // }
        }
        else
        {
            // originalKeepsakeSlot.left = gearSlotNavs[3];
            // originalAmuletSlot.left = gearSlotNavs[5];
        }
    }

    /// <summary>
    /// Updates the navigation settings for arrow elements.
    /// </summary>
    /// <param name="arrowNavs">A collection of navigation elements representing arrows.</param>
    /// <param name="right">The navigation element to be assigned as the right neighbor.</param>
    /// <param name="left">The navigation element to be assigned as the left neighbor.</param>
    /// <remarks>
    /// Iterates through each arrow navigation element in <paramref name="arrowNavs"/> and sets its right and left neighbors
    /// to the specified <paramref name="right"/> and <paramref name="left"/> navigation elements, respectively.
    /// </remarks>
    private static void UpdateArrowNavigation(IEnumerable<NavigationElement> arrowNavs, NavigationElement right, NavigationElement left)
    {
        // foreach (var arrow in arrowNavs)
        // {
        //     arrow.right = right;
        //     arrow.left = left;
        // }
    }

    /// <summary>
    /// Updates the navigation settings for gear slots based on the current configuration.
    /// </summary>
    /// <param name="gearSlotNavs">A list of navigation elements for the gear slots.</param>
    /// <param name="originalArmorSlot">The original armor slot navigation element.</param>
    /// <param name="rightArrowNav">A list of navigation elements for the right arrow.</param>
    /// <remarks>
    /// This method sets up custom navigation for each gear slot. The navigation is dependent on the state of 
    /// <see cref="Plugin.ShowPanelToggle"/> and <see cref="Plugin.IgnoreToggleWithController"/>.
    /// <para>Navigation is assigned in a grid pattern, considering the provided lists of navigation elements.</para>
    /// Assumes that <paramref name="gearSlotNavs"/>, <paramref name="originalArmorSlot"/>, and <paramref name="rightArrowNav"/> are correctly initialized.
    /// </remarks>
    private static void UpdateGearSlotNavigation(IReadOnlyList<NavigationElement> gearSlotNavs, NavigationElement originalArmorSlot, IReadOnlyList<NavigationElement> rightArrowNav)
    {
        //0  1
        //2  3
        //4  5
        // Assign navigation for gear slots
        gearSlotNavs[0].SetNavigation(gearSlotNavs[1], gearSlotNavs[2], null, null);
        gearSlotNavs[1].SetNavigation(Plugin.ShowPanelToggle.Value ? Plugin.IgnoreToggleWithController.Value ? originalArmorSlot : rightArrowNav[0] : originalArmorSlot, gearSlotNavs[3], gearSlotNavs[0], null);
        gearSlotNavs[2].SetNavigation(gearSlotNavs[3], gearSlotNavs[4], null, gearSlotNavs[0]);
        gearSlotNavs[3].SetNavigation(Plugin.ShowPanelToggle.Value ? Plugin.IgnoreToggleWithController.Value ? originalArmorSlot : rightArrowNav[0] : originalArmorSlot, gearSlotNavs[5], gearSlotNavs[2], gearSlotNavs[1]);
        gearSlotNavs[4].SetNavigation(gearSlotNavs[5], null, null, gearSlotNavs[2]);
        gearSlotNavs[5].SetNavigation(Plugin.ShowPanelToggle.Value ? Plugin.IgnoreToggleWithController.Value ? originalArmorSlot : rightArrowNav[0] : originalArmorSlot, null, gearSlotNavs[4], gearSlotNavs[3]);
    }

    /// <summary>
    /// Sets the navigation properties for a navigation element.
    /// </summary>
    /// <param name="navElement">The navigation element to update.</param>
    /// <param name="right">The navigation element to the right.</param>
    /// <param name="down">The navigation element below.</param>
    /// <param name="left">The navigation element to the left.</param>
    /// <param name="up">The navigation element above.</param>
    /// <remarks>
    /// Adjusts the navigation based on the active state of the gift panel. If the gift panel (found using <see cref="Const.GiftPanelPath"/>)
    /// is active, the navigation element is set to find left and right elements dynamically. Otherwise, it uses the provided navigation elements.
    /// <para>Assumes that the gift panel's active state correctly reflects whether dynamic finding of left and right elements is needed.</para>
    /// </remarks>
    private static void SetNavigation(this NavigationElement navElement, NavigationElement right, NavigationElement down, NavigationElement left, NavigationElement up)
    {
        // var giftPanel = GameObject.Find(Const.GiftPanelPath);
        // if (giftPanel != null && giftPanel.activeSelf)
        // {
        //     navElement.findLeftElement = true;
        //     navElement.findRightElement = true;
        // }
        // else
        // {
        //     navElement.findLeftElement = false;
        //     navElement.findRightElement = false;
        // }
        // navElement.right = right;
        // navElement.down = down;
        // navElement.left = left;
        // navElement.up = up;
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
    /// Assumes that <see cref="LeftArrowInstance"/>, <see cref="RightArrowInstance"/>, and <see cref="GearPanel"/> are already instantiated and accessible.
    /// </remarks>
    internal static void UpdatePanelVisibility()
    {
        //toggle is false, so we need to ensure the panel is visible and in the shown location
        if (!Plugin.ShowPanelToggle.Value)
        {
            if (LeftArrowInstanceButton != null)
            {
                LeftArrowInstanceButton.onClick.Invoke();
            }

            Utils.Log("ShowPanelToggle is false. Setting panel to visible & hiding toggle button.");
            PlayerPrefs.SetInt(Const.PlayerPrefKey, 0);
            LeftArrowInstance.SetActive(false);
            RightArrowInstance.SetActive(false);
        }
        else
        {
            Utils.Log("ShowPanelToggle is true. Setting panel based on settings. Showing toggle button.");
            var panelVisible = PlayerPrefs.GetInt(Const.PlayerPrefKey, 1) == 1;
            GearPanel.SetActive(panelVisible);
            if (panelVisible)
            {
                LeftArrowInstance.SetActive(false);
                RightArrowInstance.SetActive(true);
            }
            else
            {
                LeftArrowInstance.SetActive(true);
                RightArrowInstance.SetActive(false);
            }
        }
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

            newSlot.name = $"{armorType}Slot ({i})";
            Patches.GearSlots.Add(newSlot);
            inventory._slots = inventory._slots.Append(newSlot).ToArray();
            inventory.maxSlots++;
        }
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
        var existingContainer = GearPanel.transform.Find(Const.GridContainerName);
        if (existingContainer != null)
        {
            return existingContainer.gameObject;
        }

        var gridContainer = new GameObject(Const.GridContainerName);
        gridContainer.AddComponent<RectTransform>();
        gridContainer.transform.SetParent(GearPanel.transform, false);

        var gridLayout = gridContainer.AddComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(32, 32);
        gridLayout.spacing = new Vector2(10, 10);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = 2;
        gridLayout.childAlignment = TextAnchor.MiddleCenter;
        gridLayout.padding = new RectOffset(5, 0, 20, 0);

        return gridContainer;
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
        if (LeftArrowPopup == null || RightArrowPopup == null) return;
        LeftArrowPopup.text = Utils.GetTitle();
        RightArrowPopup.text = Utils.GetTitle();
        LeftArrowPopup.description = Utils.GetLeftArrowPopupContent();
        RightArrowPopup.description = Utils.GetRightArrowPopupContent();
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
        var titleText = GearPanel.GetComponentInChildren<TextMeshProUGUI>();
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
        titleText.transform.localPosition = Const.TitlePosition;
        titleText.color = Const.TitleTextColor;
        titleText.name = Const.TitleTextName;
    }
}