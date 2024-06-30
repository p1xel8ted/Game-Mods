using UnityEngine.EventSystems;

namespace MoreJewelry;

public class CustomUI : MonoBehaviour
{
    private static string bgTextureData =
        "iVBORw0KGgoAAAANSUhEUgAAAOAAAAF/CAYAAABQasXDAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAJMElEQVR4nO3cMW/c5h3HcQpxAWepvQYuMmiIjaDp4MGLp4w24BcRZMniIZOHvoJMHrKoQ+AXYeAyevLiwUNdGIoHD0GMrHanAEWg6k/qOVEU5TvJV/za6+ezSOLx7jjwC5IPH+rSnY8/7o4cdJux08H22mgnl9qH3n9w7dQaz59d6W7eerfyk9p633/3ZpMbCP+VqpXztjF12Ep1slMBHrQPbJ4+edn/vP3llW4d9QX1/vsP+g/uvrh6tYNt8+Lt23PF1xwdmA57+ny57KiVg0vjFduH3rx17eiNL7u//u2btb5k98aiu3773uF79voNhW105+t7y319HdXD8dnl0Nf4YHciwN0bHx1+8N3uxx8e93/XG+9+ttc9fPTpcp3X+7/3RY+XteVdt+h/n74G2+Dbr37ufnq6OLGvj1+rXqqh6bKm4q33P392/L4TAZZaoVZsEY61+OoUsz68hVbL64vbsooYts3DR4vlPt72+VLLqolqYxph0+KbWgY4nH4Ob5xGWF/QtOu7FuFUbdjr/dPxwraY7vfjJtr13tg0vvH1Yx/g8Tnpu2W9LcI6hz1rUGVu+dwGwLZYNcA4fr3GQqbx1QHquLc3Q4CtyHbUO45ob+UG/fmPf+j+8c9/rb2BsC2m+/6cGkNp6vS0Guu64Qzx6ZOZa8A6AtY5bjuvXbUB624IbJN19v3WTx0JzxqYvNSdU/vidZaLkm1w1j4/9/p59/lzB3geqzYc/t/NBjh3+ikmWK11Mh0XGd+2GzsV4DrXfsD5tAgXr04uPxVgjdSYzwmb1eaRTs2Ogk4jdPoJ5zMeHW3xzc2Qmb0GHK8oPriYcYRz8ZXlTJjx9JhhsumgPkCEcH7jgZjx3NEyzIYZzYSpBfWYxXTF9kEihPVN7wdWU+0pinbAW86EGWZxd0fPOC1mJ1kDFzd+Uujm/vF0zwqw/jfFQS2oiddl8eqbfgKpWxLwYdo0tApvmBd6Yn71znIQpkI7a6gU+HDjuwvtv0b8R6eiAe8nQAgSIAQJEIJWBtguFl/4T4OwcSsD/G33kw64mMuvf33v605BIUiAECRACBIgBAkQggQIQQKEoLUD/MufPJYE6/r7L+vNXHEEhCABQpAAIUiAECRACBIgBAkQggQIQQKEIAFCkAAhSIAQJEAIEiAECRCCBAhBAoQgAUKQACFIgBAkQAgSIAQJEIIECEEChCABQpAAIUiAECRACBIgBAkQggQIQQKEIAFCkAAhSIAQJEAIEiAECRCCBAhBAoQgAUKQACFIgBAkQAgSIAQJEIIECEEChCABQpAAIUiAECRACBIgBAkQggQIQQKEIAFCkAAhSIAQJEAIEiAECRCCBAhBAoQgAUKQACFIgBAkQAgSIAQJEIIECEEChCABQpAAIUiAECRACBIgBAkQggQIQQKEIAFCkAAhSIAQJEAIEiAECRCCBAhBAoQgAUKQACFIgBAkQAgSIAQJEIIECEEChCABQpAAIUiAECRACBIgBAkQggQIQQKEIAFCkAAhSIAQJEAIEiAECRCCBAhBAoQgAUKQACFIgBAkQAgSIAQJEIIECEEChCABQpAAIUiAECRACBIgBAkQggQIQQKEIAFCkAAhSIAQJEAIEiAECRCCBAhBAoQgAUKQACFIgBAkQAgSIAQJEIIECEEChCABQpAAIUiAECRACBIgBAkQggQIQQKEIAFCkAAhSIAQJEAIEiAECRCCBAhBAoQgAUKQACFIgBAkQAgSIAQJEIIECEEChCABQpAAIUiAECRACBIgBAkQggQIQQKEIAFCkAAhSIAQJEAIEiAECRCCBAhBAoQgAUKQACFIgBAkQAgSIAQJEIIECEEChCABQpAAIUiAECRACBIgBAkQggQIQQKEIAFCkAAhSIAQJEAIEiAECRCCBAhBAoQgAUKQACFIgBAkQAgSIAQJEIIECEEChCABQpAAIUiAECRACBIgBAkQggQIQQKEIAFCkAAhSIAQJEAIEiAECRCCBAhBAoQgAUKQACFIgBAkQAgSIAQJEIIECEEChCABQpAAIUiAECRACBIgBAkQggQIQQKEIAFCkAAhSIAQJEAIEiAECRCCBAhBAoQgAUKQACFIgBAkQAgSIAQJEIIECEEChCABQpAAIUiAECRACBIgBAkQggQIQQKEIAFCkAAhSIAQJEAIEiAECRCCBAhBAoQgAUKQACFIgBAkQAgSIAQJEIIECEEChCABQpAAIUiAECRACBIgBAkQggQIQQKEIAFCkAAhSIAQJEAIEiAECRCCBAhBAoQgAUKQACFIgBAkQAgSIAQJEIIECEEChCABQpAAIUiAECRACBIgBAkQggQIQQKEIAFCkAAhSIAQJEAIEiAELQN88fZt//P77950d77ugA2rtkprrVSAB/XL/QfXDsO71/30dNHd/Wyvf/G33U864OKqoW+/+vnwt73u4aNPu+u373Y//vC4xXjQHwErvufPrnS7Nxbd6/3f+xWHNwGbUE1VW1236Fu7/2A4IvYB1oKbt94d1nmvX2FYEdiUamr3xkf9EfDm/uO+ua47CrDiG6sVy+XXvy6XvRr9Drzf5cnframmmnv6ZMUo6BdXr3bAhxkPukzNBljXf+KDzaiWqqm6Dpw6FaD4YPNahItXJ5efCrBGRGt0RoSwOXUaWm1NnQqwLhZFCJvT4psOxJTZa8C5FYGLO6spc0EhaHYQpmnDp05F4fzGtx9aV7ODMMNd+Zoas3fiQrHmhrZ5oRf5Utg25z0YLV5908/9bKqn219+fvTXaCZMDbpM46uJ2WUc1XgD5mKbu9cB22JujvT7mqiGqqUW4TDA+bL/eeZMmHF8ZRxVu084Htlp89zqtZrrBttrbzmxuu377Y5BNTFtpUwjHFsGOBwW352Kb3rvoj0pMR5WbRtSr9Uhdu5+B/yvq9DG8ZXxbbvp2d+4g3GErbVy6gg4jq++qN40Nb2QPFp6dPTbWz54CNtm2McXJ870rt/uZh9ir+UVXIt13FZzIsAajHn+bPi9rguHwZnTh805/SNN+8O6Rk3ZRnWKWUGN9/VVhobeLQc6pyrAncMj1kE9IHhR7XnCuUfuYZsMg5XH+/y6pusetbLTjoB9hKff9qYfqVntxHo7HWyvgyGeC7Ux1nfyb/w5mjB20f/1AAAAAElFTkSuQmCC";

    private static Sprite bgSprite;

    private static string titleTextureData = "iVBORw0KGgoAAAANSUhEUgAAAHAAAAAVCAYAAACe2WqiAAABBklEQVRoBe2aMQrCQBBFZ4NNEE/gAXKKgG2OIWhpYSWCpSDWll5A8AC2QjxE0lgIOYFY6spEFoxzgf34p8ju/jSP/7ZLnG8mXnReA5Hk3m75wGnAVYfMZ3neEtdlKavlA4f+D0nXm758+0rm45uoOB3Ki/9GBEfqTN0linytnvGTk7DTQHDWCuy84QGqAQqE0mVhKdB2ApVQIJQuC0uBthOohAKhdFlYCrSdQCUUCKXLwlKg7QQqoUAoXRaWAm0nUAkFQumysBRoO4FKOgL1WxMn7gZ+HfUC7ml/lmI6kuMlJFxjbUBdhXFFmurezxbDkHEFaGC3bZTSBYF6+PwbozsOQgNOId8BxTDZWfVzgwAAAABJRU5ErkJggg==";
    private static Sprite titleSprite;

    protected ScrollRect _scrollRect;

    public void ClearContent()
    {
        foreach (Transform child in _scrollRect.content.transform)
        {
            Destroy(child.gameObject);
        }
    }

    internal static GameObject CreateScrollView(Transform parent)
    {
        ScrollRect scrollRect = new GameObject(Const.GridContainerName).AddComponent<ScrollRect>();
        scrollRect.gameObject.layer = LayerMask.NameToLayer("UI");
        scrollRect.transform.SetParent(parent);
        scrollRect.transform.localScale = Vector3.one;
        scrollRect.transform.localPosition = Vector3.zero;
        scrollRect.scrollSensitivity = 5f;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;

        GameObject viewport = new GameObject("Viewport");
        viewport.layer = LayerMask.NameToLayer("UI");
        RectTransform viewportTransform = viewport.AddComponent<RectTransform>();
        viewportTransform.SetParent(scrollRect.transform);
        viewportTransform.localScale = Vector3.one;
        viewportTransform.localPosition = Vector3.zero;
        viewportTransform.sizeDelta = new Vector2(100, 165);

        var viewportImage = viewport.AddComponent<Image>();
        viewportImage.rectTransform.sizeDelta = new Vector2(100, 165);
        viewportImage.color = new Color(0, 0, 0, 0); // Transparent color for the viewport image
        var mask = viewport.AddComponent<Mask>();
        mask.showMaskGraphic = false;

        GameObject content = new GameObject("Content");
        content.layer = LayerMask.NameToLayer("UI");
        RectTransform contentTransform = content.AddComponent<RectTransform>();
        contentTransform.SetParent(viewportTransform);
        contentTransform.localScale = Vector3.one;
        contentTransform.localPosition = Vector3.zero;
        contentTransform.sizeDelta = new Vector2(100, 165);

        var gridLayoutGroup = content.AddComponent<GridLayoutGroup>();
        gridLayoutGroup.cellSize = new Vector2(32, 32);
        gridLayoutGroup.spacing = new Vector2(10, 10);
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = 2;
        gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        gridLayoutGroup.padding = new RectOffset(5, 0, 20, 0);
        var fitter = content.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        scrollRect.content = contentTransform;
        scrollRect.viewport = viewportTransform;
        scrollRect.horizontal = false;
        scrollRect.vertical = true;

        return content;
    }

    internal static GameObject CreateBg()
    {
        if (!bgSprite)
        {
            var bgTexture = CreateTexture(bgTextureData);
            bgSprite = Sprite.Create(bgTexture, new Rect(0, 0, bgTexture.width, bgTexture.height), new Vector2(0.5f, 0.5f), 24);
        }

        var go = new GameObject("Background");
        var bg = go.AddComponent<Image>();
        bg.sprite = bgSprite;
        bg.gameObject.layer = LayerMask.NameToLayer("UI");
        RectTransform transform = bg.rectTransform;
        transform.SetParent(GameObject.Find(Const.PlayerInventoryPath).transform);
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.sizeDelta = new Vector2(100, 165);

        return go;
    }


    internal static Transform CreateTitle(Transform parent)
    {
        if (!titleSprite)
        {
            var tex = CreateTexture(titleTextureData);
            titleSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 24);
        }

        Image title = new GameObject("Title").AddComponent<Image>();
        title.sprite = titleSprite;
        title.gameObject.layer = LayerMask.NameToLayer("UI");
        title.transform.SetParent(parent);
        title.preserveAspect = true;
        RectTransform transform = title.rectTransform;
        transform.localScale = Vector3.one;
        transform.localPosition = new Vector3(0, 177, 0);
        transform.localRotation = Quaternion.identity;
        return title.transform;
    }

    internal static TextMeshProUGUI CreateTitleText(Transform parent, string name)
    {
        TextMeshProUGUI titleText = CreateText(parent, "Title text");
        titleText.enableAutoSizing = true;
        titleText.gameObject.layer = LayerMask.NameToLayer("UI");
        titleText.rectTransform.sizeDelta = new Vector2(80, 30);
        titleText.transform.localScale = Vector3.one;
        titleText.transform.localPosition = new Vector3(0.0f, 0f, 0.0f);
        titleText.transform.localRotation = Quaternion.identity;
        titleText.alignment = TextAlignmentOptions.Midline;
        titleText.fontSizeMax = 30f;
        titleText.fontSizeMin = 10f;
        titleText.fontSize = 18f;
        titleText.text = name;
        return titleText;
    }

    internal static TextMeshProUGUI CreateText(Transform parent, string name)
    {
        var baseText = UIHandler.Instance.endOfDayScreen.coinsTotalTMP;
        var text = Instantiate(baseText, parent);
        text.alignment = TextAlignmentOptions.Left;
        text.gameObject.layer = LayerMask.NameToLayer("UI");
        text.name = name;
        return text;
    }

    protected Button CreateExitButton(Transform parent)
    {
        var buttonTransform = Instantiate(Traverse.Create(UIHandler.Instance).Field("_inventoryUI").GetValue<GameObject>().transform.Find("Inventory/ExitButton"), parent);
        buttonTransform.localPosition = new Vector3(110, 190, 0);
        return buttonTransform.gameObject.GetComponent<Button>();
    }

    protected static Texture2D CreateTexture(string data)
    {
        var texture = new Texture2D(1, 1);
        texture.LoadImage(Convert.FromBase64String(data));
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.wrapModeU = TextureWrapMode.Clamp;
        texture.wrapModeV = TextureWrapMode.Clamp;
        texture.wrapModeW = TextureWrapMode.Clamp;
        return texture;
    }
}