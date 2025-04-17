namespace EasyLiving;

internal class MenuUpdater : MonoBehaviour
{
    private GameObject _menuBox;
    private Image _border;
    private RectTransform _rectTransform;
    private Transform _continueButton;
    //private Transform _playButton;
    private Transform _dlcShopButton;
    private Transform _socialMediaButtons;

    internal void SetContinueNull()
    {
        _continueButton = null;
    }
    
    private void FindMenuObjects()
    {
        _menuBox = MainMenuController.Instance.PCHomeMenu.transform.Find("PlayButtons").gameObject;
       // _menuBox = GameObject.Find("Canvas_Home/[HomeMenu]/[PCHomeMenu]/PlayButtons");
        if (_menuBox)
        {
            _border = _menuBox.GetComponent<Image>();
            _rectTransform = _menuBox.GetComponent<RectTransform>();
            //_playButton = _menuBox.transform.Find("PlayButton");

            Canvas.ForceUpdateCanvases();
            
            _continueButton = _menuBox.transform.Find("ContinueButton");
            _dlcShopButton = _menuBox.transform.Find("DLCShopButton");
            _socialMediaButtons = _menuBox.transform.Find("SocialMediaButtons");
            _dlcShopButton = _menuBox.transform.Find("DLCShopButton");
            _socialMediaButtons = _menuBox.transform.Find("SocialMediaButtons");
        }
    }

    private void Awake()
    {
        FindMenuObjects();
    }
    
    private void Start()
    {
        FindMenuObjects();
    }

    private void OnEnable()
    {
        FindMenuObjects();
    }

    private void LateUpdate()
    {
        _continueButton ??= _menuBox.transform.Find("ContinueButton");
        
        _rectTransform.anchorMin = new Vector2(0.5f, 1); // Top Center
        _rectTransform.anchorMax = new Vector2(0.5f, 1); // Top Center
        _rectTransform.pivot = new Vector2(0.5f, 1);  
            
        _menuBox.transform.localPosition = _menuBox.transform.localPosition with {y = 0};
            
        if (_dlcShopButton)
        {
            _dlcShopButton.gameObject.SetActive(!Plugin.RemoveDlcShopButton.Value);
        }

        if (_socialMediaButtons)
        {
            _socialMediaButtons.gameObject.SetActive(!Plugin.RemoveSocialMediaButtons.Value);
        }

        if (_border)
        {
            _border.enabled = !Plugin.RemoveMenuButtonsBorder.Value;
        }

        var y = _continueButton ? 320 : 290;

        if (Plugin.RemoveDlcShopButton.Value)
        {
            y -= 50;
        }
        if (Plugin.RemoveSocialMediaButtons.Value)
        {
            y -= 50;
        }

        _rectTransform.sizeDelta = _rectTransform.sizeDelta with {y = y};
    }
}