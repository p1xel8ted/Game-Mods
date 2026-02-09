// Decompiled with JetBrains decompiler
// Type: UIPopupList
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Popup List")]
public class UIPopupList : UIWidgetContainer
{
  public static UIPopupList current;
  public static GameObject mChild;
  public static float mFadeOutComplete;
  public const float animSpeed = 0.15f;
  public UIAtlas atlas;
  public UIFont bitmapFont;
  public Font trueTypeFont;
  public int fontSize = 16 /*0x10*/;
  public FontStyle fontStyle;
  public string backgroundSprite;
  public string highlightSprite;
  public UnityEngine.Sprite background2DSprite;
  public UnityEngine.Sprite highlight2DSprite;
  public UIPopupList.Position position;
  public UIPopupList.Selection selection;
  public NGUIText.Alignment alignment = NGUIText.Alignment.Left;
  public List<string> items = new List<string>();
  public List<object> itemData = new List<object>();
  public Vector2 padding = (Vector2) new Vector3(4f, 4f);
  public Color textColor = Color.white;
  public Color backgroundColor = Color.white;
  public Color highlightColor = new Color(0.882352948f, 0.784313738f, 0.5882353f, 1f);
  public bool isAnimated = true;
  public bool isLocalized;
  public UILabel.Modifier textModifier;
  public bool separatePanel = true;
  public int overlap;
  public UIPopupList.OpenOn openOn;
  public List<EventDelegate> onChange = new List<EventDelegate>();
  [SerializeField]
  [HideInInspector]
  public string mSelectedItem;
  [SerializeField]
  [HideInInspector]
  public UIPanel mPanel;
  [SerializeField]
  [HideInInspector]
  public UIBasicSprite mBackground;
  [HideInInspector]
  [SerializeField]
  public UIBasicSprite mHighlight;
  [HideInInspector]
  [SerializeField]
  public UILabel mHighlightedLabel;
  [HideInInspector]
  [SerializeField]
  public List<UILabel> mLabelList = new List<UILabel>();
  [SerializeField]
  [HideInInspector]
  public float mBgBorder;
  [Tooltip("Whether the selection will be persistent even after the popup list is closed. By default the selection is cleared when the popup is closed so that the same selection can be chosen again the next time the popup list is opened. If enabled, the selection will persist, but selecting the same choice in succession will not result in the onChange notification being triggered more than once.")]
  public bool keepValue;
  [NonSerialized]
  public GameObject mSelection;
  [NonSerialized]
  public int mOpenFrame;
  [SerializeField]
  [HideInInspector]
  public GameObject eventReceiver;
  [SerializeField]
  [HideInInspector]
  public string functionName = "OnSelectionChange";
  [HideInInspector]
  [SerializeField]
  public float textScale;
  [SerializeField]
  [HideInInspector]
  public UIFont font;
  [SerializeField]
  [HideInInspector]
  public UILabel textLabel;
  [NonSerialized]
  public Vector3 startingPosition;
  public UIPopupList.LegacyEvent mLegacyEvent;
  [NonSerialized]
  public bool mExecuting;
  public bool mUseDynamicFont;
  [NonSerialized]
  public bool mStarted;
  public bool mTweening;
  public GameObject source;

  public UnityEngine.Object ambigiousFont
  {
    get
    {
      if ((UnityEngine.Object) this.trueTypeFont != (UnityEngine.Object) null)
        return (UnityEngine.Object) this.trueTypeFont;
      return (UnityEngine.Object) this.bitmapFont != (UnityEngine.Object) null ? (UnityEngine.Object) this.bitmapFont : (UnityEngine.Object) this.font;
    }
    set
    {
      switch (value)
      {
        case Font _:
          this.trueTypeFont = value as Font;
          this.bitmapFont = (UIFont) null;
          this.font = (UIFont) null;
          break;
        case UIFont _:
          this.bitmapFont = value as UIFont;
          this.trueTypeFont = (Font) null;
          this.font = (UIFont) null;
          break;
      }
    }
  }

  [Obsolete("Use EventDelegate.Add(popup.onChange, YourCallback) instead, and UIPopupList.current.value to determine the state")]
  public UIPopupList.LegacyEvent onSelectionChange
  {
    get => this.mLegacyEvent;
    set => this.mLegacyEvent = value;
  }

  public static bool isOpen
  {
    get
    {
      if (!((UnityEngine.Object) UIPopupList.current != (UnityEngine.Object) null))
        return false;
      return (UnityEngine.Object) UIPopupList.mChild != (UnityEngine.Object) null || (double) UIPopupList.mFadeOutComplete > (double) Time.unscaledTime;
    }
  }

  public virtual string value
  {
    get => this.mSelectedItem;
    set => this.Set(value);
  }

  public virtual object data
  {
    get
    {
      int index = this.items.IndexOf(this.mSelectedItem);
      return index <= -1 || index >= this.itemData.Count ? (object) null : this.itemData[index];
    }
  }

  public bool isColliderEnabled
  {
    get
    {
      Collider component1 = this.GetComponent<Collider>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        return component1.enabled;
      Collider2D component2 = this.GetComponent<Collider2D>();
      return (UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.enabled;
    }
  }

  public bool isValid
  {
    get => (UnityEngine.Object) this.bitmapFont != (UnityEngine.Object) null || (UnityEngine.Object) this.trueTypeFont != (UnityEngine.Object) null;
  }

  public int activeFontSize
  {
    get
    {
      return !((UnityEngine.Object) this.trueTypeFont != (UnityEngine.Object) null) && !((UnityEngine.Object) this.bitmapFont == (UnityEngine.Object) null) ? this.bitmapFont.defaultSize : this.fontSize;
    }
  }

  public float activeFontScale
  {
    get
    {
      return !((UnityEngine.Object) this.trueTypeFont != (UnityEngine.Object) null) && !((UnityEngine.Object) this.bitmapFont == (UnityEngine.Object) null) ? (float) this.fontSize / (float) this.bitmapFont.defaultSize : 1f;
    }
  }

  public void Set(string value, bool notify = true)
  {
    if (!(this.mSelectedItem != value))
      return;
    this.mSelectedItem = value;
    if (this.mSelectedItem == null)
      return;
    if (notify && this.mSelectedItem != null)
      this.TriggerCallbacks();
    if (this.keepValue)
      return;
    this.mSelectedItem = (string) null;
  }

  public virtual void Clear()
  {
    this.items.Clear();
    this.itemData.Clear();
  }

  public virtual void AddItem(string text)
  {
    this.items.Add(text);
    this.itemData.Add((object) text);
  }

  public virtual void AddItem(string text, object data)
  {
    this.items.Add(text);
    this.itemData.Add(data);
  }

  public virtual void RemoveItem(string text)
  {
    int index = this.items.IndexOf(text);
    if (index == -1)
      return;
    this.items.RemoveAt(index);
    this.itemData.RemoveAt(index);
  }

  public virtual void RemoveItemByData(object data)
  {
    int index = this.itemData.IndexOf(data);
    if (index == -1)
      return;
    this.items.RemoveAt(index);
    this.itemData.RemoveAt(index);
  }

  public void TriggerCallbacks()
  {
    if (this.mExecuting)
      return;
    this.mExecuting = true;
    UIPopupList current = UIPopupList.current;
    UIPopupList.current = this;
    if (this.mLegacyEvent != null)
      this.mLegacyEvent(this.mSelectedItem);
    if (EventDelegate.IsValid(this.onChange))
      EventDelegate.Execute(this.onChange);
    else if ((UnityEngine.Object) this.eventReceiver != (UnityEngine.Object) null && !string.IsNullOrEmpty(this.functionName))
      this.eventReceiver.SendMessage(this.functionName, (object) this.mSelectedItem, SendMessageOptions.DontRequireReceiver);
    UIPopupList.current = current;
    this.mExecuting = false;
  }

  public virtual void OnEnable()
  {
    if (EventDelegate.IsValid(this.onChange))
    {
      this.eventReceiver = (GameObject) null;
      this.functionName = (string) null;
    }
    if ((UnityEngine.Object) this.font != (UnityEngine.Object) null)
    {
      if (this.font.isDynamic)
      {
        this.trueTypeFont = this.font.dynamicFont;
        this.fontStyle = this.font.dynamicFontStyle;
        this.mUseDynamicFont = true;
      }
      else if ((UnityEngine.Object) this.bitmapFont == (UnityEngine.Object) null)
      {
        this.bitmapFont = this.font;
        this.mUseDynamicFont = false;
      }
      this.font = (UIFont) null;
    }
    if ((double) this.textScale != 0.0)
    {
      this.fontSize = (UnityEngine.Object) this.bitmapFont != (UnityEngine.Object) null ? Mathf.RoundToInt((float) this.bitmapFont.defaultSize * this.textScale) : 16 /*0x10*/;
      this.textScale = 0.0f;
    }
    if (!((UnityEngine.Object) this.trueTypeFont == (UnityEngine.Object) null) || !((UnityEngine.Object) this.bitmapFont != (UnityEngine.Object) null) || !this.bitmapFont.isDynamic)
      return;
    this.trueTypeFont = this.bitmapFont.dynamicFont;
    this.bitmapFont = (UIFont) null;
  }

  public virtual void OnValidate()
  {
    Font trueTypeFont = this.trueTypeFont;
    UIFont bitmapFont = this.bitmapFont;
    this.bitmapFont = (UIFont) null;
    this.trueTypeFont = (Font) null;
    if ((UnityEngine.Object) trueTypeFont != (UnityEngine.Object) null && ((UnityEngine.Object) bitmapFont == (UnityEngine.Object) null || !this.mUseDynamicFont))
    {
      this.bitmapFont = (UIFont) null;
      this.trueTypeFont = trueTypeFont;
      this.mUseDynamicFont = true;
    }
    else if ((UnityEngine.Object) bitmapFont != (UnityEngine.Object) null)
    {
      if (bitmapFont.isDynamic)
      {
        this.trueTypeFont = bitmapFont.dynamicFont;
        this.fontStyle = bitmapFont.dynamicFontStyle;
        this.fontSize = bitmapFont.defaultSize;
        this.mUseDynamicFont = true;
      }
      else
      {
        this.bitmapFont = bitmapFont;
        this.mUseDynamicFont = false;
      }
    }
    else
    {
      this.trueTypeFont = trueTypeFont;
      this.mUseDynamicFont = true;
    }
  }

  public virtual void Start()
  {
    if (this.mStarted)
      return;
    this.mStarted = true;
    if (this.keepValue)
    {
      string mSelectedItem = this.mSelectedItem;
      this.mSelectedItem = (string) null;
      this.value = mSelectedItem;
    }
    else
      this.mSelectedItem = (string) null;
    if (!((UnityEngine.Object) this.textLabel != (UnityEngine.Object) null))
      return;
    EventDelegate.Add(this.onChange, new EventDelegate.Callback(this.textLabel.SetCurrentSelection));
    this.textLabel = (UILabel) null;
  }

  public virtual void OnLocalize()
  {
    if (!this.isLocalized)
      return;
    this.TriggerCallbacks();
  }

  public virtual void Highlight(UILabel lbl, bool instant)
  {
    if (!((UnityEngine.Object) this.mHighlight != (UnityEngine.Object) null))
      return;
    this.mHighlightedLabel = lbl;
    Vector3 highlightPosition = this.GetHighlightPosition();
    if (!instant && this.isAnimated)
    {
      TweenPosition.Begin(this.mHighlight.gameObject, 0.1f, highlightPosition).method = UITweener.Method.EaseOut;
      if (this.mTweening)
        return;
      this.mTweening = true;
      this.StartCoroutine("UpdateTweenPosition");
    }
    else
      this.mHighlight.cachedTransform.localPosition = highlightPosition;
  }

  public virtual Vector3 GetHighlightPosition()
  {
    if ((UnityEngine.Object) this.mHighlightedLabel == (UnityEngine.Object) null || (UnityEngine.Object) this.mHighlight == (UnityEngine.Object) null)
      return Vector3.zero;
    Vector4 border = this.mHighlight.border;
    float num = (UnityEngine.Object) this.atlas != (UnityEngine.Object) null ? this.atlas.pixelSize : 1f;
    return this.mHighlightedLabel.cachedTransform.localPosition + new Vector3(-(border.x * num), border.w * num, 1f);
  }

  public virtual IEnumerator UpdateTweenPosition()
  {
    if ((UnityEngine.Object) this.mHighlight != (UnityEngine.Object) null && (UnityEngine.Object) this.mHighlightedLabel != (UnityEngine.Object) null)
    {
      TweenPosition tp = this.mHighlight.GetComponent<TweenPosition>();
      while ((UnityEngine.Object) tp != (UnityEngine.Object) null && tp.enabled)
      {
        tp.to = this.GetHighlightPosition();
        yield return (object) null;
      }
      tp = (TweenPosition) null;
    }
    this.mTweening = false;
  }

  public virtual void OnItemHover(GameObject go, bool isOver)
  {
    if (!isOver)
      return;
    this.Highlight(go.GetComponent<UILabel>(), false);
  }

  public virtual void OnItemPress(GameObject go, bool isPressed)
  {
    if (!isPressed || this.selection != UIPopupList.Selection.OnPress)
      return;
    this.OnItemClick(go);
  }

  public virtual void OnItemClick(GameObject go)
  {
    this.Select(go.GetComponent<UILabel>(), true);
    this.value = go.GetComponent<UIEventListener>().parameter as string;
    UIPlaySound[] components = this.GetComponents<UIPlaySound>();
    int index = 0;
    for (int length = components.Length; index < length; ++index)
    {
      UIPlaySound uiPlaySound = components[index];
      if (uiPlaySound.trigger == UIPlaySound.Trigger.OnClick)
        NGUITools.PlaySound(uiPlaySound.audioClip, uiPlaySound.volume, 1f);
    }
    this.CloseSelf();
  }

  public void Select(UILabel lbl, bool instant) => this.Highlight(lbl, instant);

  public virtual void OnNavigate(KeyCode key)
  {
    if (!this.enabled || !((UnityEngine.Object) UIPopupList.current == (UnityEngine.Object) this))
      return;
    int num1 = this.mLabelList.IndexOf(this.mHighlightedLabel);
    if (num1 == -1)
      num1 = 0;
    int num2;
    switch (key)
    {
      case KeyCode.UpArrow:
        if (num1 <= 0)
          break;
        this.Select(this.mLabelList[num2 = num1 - 1], false);
        break;
      case KeyCode.DownArrow:
        if (num1 + 1 >= this.mLabelList.Count)
          break;
        this.Select(this.mLabelList[num2 = num1 + 1], false);
        break;
    }
  }

  public virtual void OnKey(KeyCode key)
  {
    if (!this.enabled || !((UnityEngine.Object) UIPopupList.current == (UnityEngine.Object) this) || key != UICamera.current.cancelKey0 && key != UICamera.current.cancelKey1)
      return;
    this.OnSelect(false);
  }

  public virtual void OnDisable() => this.CloseSelf();

  public virtual void OnSelect(bool isSelected)
  {
    if (isSelected)
      return;
    this.CloseSelf();
  }

  public static void Close()
  {
    if (!((UnityEngine.Object) UIPopupList.current != (UnityEngine.Object) null))
      return;
    UIPopupList.current.CloseSelf();
    UIPopupList.current = (UIPopupList) null;
  }

  public virtual void CloseSelf()
  {
    if (!((UnityEngine.Object) UIPopupList.mChild != (UnityEngine.Object) null) || !((UnityEngine.Object) UIPopupList.current == (UnityEngine.Object) this))
      return;
    this.StopCoroutine("CloseIfUnselected");
    this.mSelection = (GameObject) null;
    this.mLabelList.Clear();
    if (this.isAnimated)
    {
      UIWidget[] componentsInChildren1 = UIPopupList.mChild.GetComponentsInChildren<UIWidget>();
      int index1 = 0;
      for (int length = componentsInChildren1.Length; index1 < length; ++index1)
      {
        UIWidget uiWidget = componentsInChildren1[index1];
        TweenColor.Begin(uiWidget.gameObject, 0.15f, uiWidget.color with
        {
          a = 0.0f
        }).method = UITweener.Method.EaseOut;
      }
      Collider[] componentsInChildren2 = UIPopupList.mChild.GetComponentsInChildren<Collider>();
      int index2 = 0;
      for (int length = componentsInChildren2.Length; index2 < length; ++index2)
        componentsInChildren2[index2].enabled = false;
      UnityEngine.Object.Destroy((UnityEngine.Object) UIPopupList.mChild, 0.15f);
      UIPopupList.mFadeOutComplete = Time.unscaledTime + Mathf.Max(0.1f, 0.15f);
    }
    else
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) UIPopupList.mChild);
      UIPopupList.mFadeOutComplete = Time.unscaledTime + 0.1f;
    }
    this.mBackground = (UIBasicSprite) null;
    this.mHighlight = (UIBasicSprite) null;
    UIPopupList.mChild = (GameObject) null;
    UIPopupList.current = (UIPopupList) null;
  }

  public virtual void AnimateColor(UIWidget widget)
  {
    Color color = widget.color;
    widget.color = new Color(color.r, color.g, color.b, 0.0f);
    TweenColor.Begin(widget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
  }

  public virtual void AnimatePosition(UIWidget widget, bool placeAbove, float bottom)
  {
    Vector3 localPosition = widget.cachedTransform.localPosition;
    Vector3 vector3 = placeAbove ? new Vector3(localPosition.x, bottom, localPosition.z) : new Vector3(localPosition.x, 0.0f, localPosition.z);
    widget.cachedTransform.localPosition = vector3;
    TweenPosition.Begin(widget.gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
  }

  public virtual void AnimateScale(UIWidget widget, bool placeAbove, float bottom)
  {
    GameObject gameObject = widget.gameObject;
    Transform cachedTransform = widget.cachedTransform;
    float num = (float) ((double) this.activeFontSize * (double) this.activeFontScale + (double) this.mBgBorder * 2.0);
    cachedTransform.localScale = new Vector3(1f, num / (float) widget.height, 1f);
    TweenScale.Begin(gameObject, 0.15f, Vector3.one).method = UITweener.Method.EaseOut;
    if (!placeAbove)
      return;
    Vector3 localPosition = cachedTransform.localPosition;
    cachedTransform.localPosition = new Vector3(localPosition.x, localPosition.y - (float) widget.height + num, localPosition.z);
    TweenPosition.Begin(gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
  }

  public void Animate(UIWidget widget, bool placeAbove, float bottom)
  {
    this.AnimateColor(widget);
    this.AnimatePosition(widget, placeAbove, bottom);
  }

  public virtual void OnClick()
  {
    if (this.mOpenFrame == Time.frameCount)
      return;
    if ((UnityEngine.Object) UIPopupList.mChild == (UnityEngine.Object) null)
    {
      if (this.openOn == UIPopupList.OpenOn.DoubleClick || this.openOn == UIPopupList.OpenOn.Manual || this.openOn == UIPopupList.OpenOn.RightClick && UICamera.currentTouchID != -2)
        return;
      this.Show();
    }
    else
    {
      if (!((UnityEngine.Object) this.mHighlightedLabel != (UnityEngine.Object) null))
        return;
      this.OnItemPress(this.mHighlightedLabel.gameObject, true);
    }
  }

  public virtual void OnDoubleClick()
  {
    if (this.openOn != UIPopupList.OpenOn.DoubleClick)
      return;
    this.Show();
  }

  public IEnumerator CloseIfUnselected()
  {
    do
    {
      yield return (object) null;
    }
    while (!((UnityEngine.Object) UICamera.selectedObject != (UnityEngine.Object) this.mSelection));
    this.CloseSelf();
  }

  public virtual void Show()
  {
    if (this.enabled && NGUITools.GetActive(this.gameObject) && (UnityEngine.Object) UIPopupList.mChild == (UnityEngine.Object) null && this.isValid && this.items.Count > 0)
    {
      this.mLabelList.Clear();
      this.StopCoroutine("CloseIfUnselected");
      UICamera.selectedObject = UICamera.hoveredObject ?? this.gameObject;
      this.mSelection = UICamera.selectedObject;
      this.source = this.mSelection;
      if ((UnityEngine.Object) this.source == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "Popup list needs a source object...");
      }
      else
      {
        this.mOpenFrame = Time.frameCount;
        if ((UnityEngine.Object) this.mPanel == (UnityEngine.Object) null)
        {
          this.mPanel = UIPanel.Find(this.transform);
          if ((UnityEngine.Object) this.mPanel == (UnityEngine.Object) null)
            return;
        }
        UIPopupList.mChild = new GameObject("Drop-down List");
        UIPopupList.mChild.layer = this.gameObject.layer;
        if (this.separatePanel)
        {
          if ((UnityEngine.Object) this.GetComponent<Collider>() != (UnityEngine.Object) null)
            UIPopupList.mChild.AddComponent<Rigidbody>().isKinematic = true;
          else if ((UnityEngine.Object) this.GetComponent<Collider2D>() != (UnityEngine.Object) null)
            UIPopupList.mChild.AddComponent<Rigidbody2D>().isKinematic = true;
          UIPanel uiPanel = UIPopupList.mChild.AddComponent<UIPanel>();
          uiPanel.depth = 1000000;
          uiPanel.sortingOrder = this.mPanel.sortingOrder;
        }
        UIPopupList.current = this;
        Transform relativeTo = this.separatePanel ? ((MonoBehaviour) this.mPanel.GetComponentInParent<UIRoot>() ?? (MonoBehaviour) this.mPanel).transform : this.mPanel.cachedTransform;
        Transform transform = UIPopupList.mChild.transform;
        transform.parent = relativeTo;
        Vector3 vector3_1;
        Vector3 vector3_2;
        if (this.openOn == UIPopupList.OpenOn.Manual && (UnityEngine.Object) this.mSelection != (UnityEngine.Object) this.gameObject)
        {
          this.startingPosition = (Vector3) UICamera.lastEventPosition;
          vector3_1 = relativeTo.InverseTransformPoint(this.mPanel.anchorCamera.ScreenToWorldPoint(this.startingPosition));
          vector3_2 = vector3_1;
          transform.localPosition = vector3_1;
          this.startingPosition = transform.position;
        }
        else
        {
          Bounds relativeWidgetBounds = NGUIMath.CalculateRelativeWidgetBounds(relativeTo, this.transform, false, false);
          vector3_1 = relativeWidgetBounds.min;
          vector3_2 = relativeWidgetBounds.max;
          transform.localPosition = vector3_1;
          this.startingPosition = transform.position;
        }
        this.StartCoroutine("CloseIfUnselected");
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        int nextDepth = this.separatePanel ? 0 : NGUITools.CalculateNextDepth(this.mPanel.gameObject);
        if ((UnityEngine.Object) this.background2DSprite != (UnityEngine.Object) null)
        {
          UI2DSprite ui2Dsprite = UIPopupList.mChild.AddWidget<UI2DSprite>(nextDepth);
          ui2Dsprite.sprite2D = this.background2DSprite;
          this.mBackground = (UIBasicSprite) ui2Dsprite;
        }
        else
        {
          if (!((UnityEngine.Object) this.atlas != (UnityEngine.Object) null))
            return;
          this.mBackground = (UIBasicSprite) UIPopupList.mChild.AddSprite(this.atlas, this.backgroundSprite, nextDepth);
        }
        bool placeAbove = this.position == UIPopupList.Position.Above;
        if (this.position == UIPopupList.Position.Auto)
        {
          UICamera cameraForLayer = UICamera.FindCameraForLayer(this.mSelection.layer);
          if ((UnityEngine.Object) cameraForLayer != (UnityEngine.Object) null)
            placeAbove = (double) cameraForLayer.cachedCamera.WorldToViewportPoint(this.startingPosition).y < 0.5;
        }
        this.mBackground.pivot = UIWidget.Pivot.TopLeft;
        this.mBackground.color = this.backgroundColor;
        Vector4 border = this.mBackground.border;
        this.mBgBorder = border.y;
        this.mBackground.cachedTransform.localPosition = new Vector3(0.0f, placeAbove ? border.y * 2f - (float) this.overlap : (float) this.overlap, 0.0f);
        int num1;
        if ((UnityEngine.Object) this.highlight2DSprite != (UnityEngine.Object) null)
        {
          UI2DSprite ui2Dsprite = UIPopupList.mChild.AddWidget<UI2DSprite>(num1 = nextDepth + 1);
          ui2Dsprite.sprite2D = this.highlight2DSprite;
          this.mHighlight = (UIBasicSprite) ui2Dsprite;
        }
        else
        {
          if (!((UnityEngine.Object) this.atlas != (UnityEngine.Object) null))
            return;
          this.mHighlight = (UIBasicSprite) UIPopupList.mChild.AddSprite(this.atlas, this.highlightSprite, num1 = nextDepth + 1);
        }
        float num2 = 0.0f;
        float num3 = 0.0f;
        if (this.mHighlight.hasBorder)
        {
          num2 = this.mHighlight.border.w;
          num3 = this.mHighlight.border.x;
        }
        this.mHighlight.pivot = UIWidget.Pivot.TopLeft;
        this.mHighlight.color = this.highlightColor;
        float num4 = (float) this.activeFontSize * this.activeFontScale;
        float num5 = num4 + this.padding.y;
        float a = 0.0f;
        float y = placeAbove ? border.y - this.padding.y - (float) this.overlap : -this.padding.y - border.y + (float) this.overlap;
        float f1 = border.y * 2f + this.padding.y;
        List<UILabel> uiLabelList = new List<UILabel>();
        if (!this.items.Contains(this.mSelectedItem))
          this.mSelectedItem = (string) null;
        int index1 = 0;
        for (int count = this.items.Count; index1 < count; ++index1)
        {
          string key = this.items[index1];
          UILabel lbl = UIPopupList.mChild.AddWidget<UILabel>(this.mBackground.depth + 2);
          lbl.name = index1.ToString();
          lbl.pivot = UIWidget.Pivot.TopLeft;
          lbl.bitmapFont = this.bitmapFont;
          lbl.trueTypeFont = this.trueTypeFont;
          lbl.fontSize = this.fontSize;
          lbl.fontStyle = this.fontStyle;
          lbl.text = this.isLocalized ? Localization.Get(key) : key;
          lbl.modifier = this.textModifier;
          lbl.color = this.textColor;
          lbl.cachedTransform.localPosition = new Vector3(border.x + this.padding.x - lbl.pivotOffset.x, y, -1f);
          lbl.overflowMethod = UILabel.Overflow.ResizeFreely;
          lbl.alignment = this.alignment;
          uiLabelList.Add(lbl);
          f1 += num5;
          y -= num5;
          a = Mathf.Max(a, lbl.printedSize.x);
          UIEventListener uiEventListener = UIEventListener.Get(lbl.gameObject);
          uiEventListener.onHover = new UIEventListener.BoolDelegate(this.OnItemHover);
          uiEventListener.onPress = new UIEventListener.BoolDelegate(this.OnItemPress);
          uiEventListener.onClick = new UIEventListener.VoidDelegate(this.OnItemClick);
          uiEventListener.parameter = (object) key;
          if (this.mSelectedItem == key || index1 == 0 && string.IsNullOrEmpty(this.mSelectedItem))
            this.Highlight(lbl, true);
          this.mLabelList.Add(lbl);
        }
        float f2 = Mathf.Max(a, (float) ((double) vector3_2.x - (double) vector3_1.x - ((double) border.x + (double) this.padding.x) * 2.0));
        float x = f2;
        Vector3 vector3_3 = new Vector3(x * 0.5f, (float) (-(double) num4 * 0.5), 0.0f);
        Vector3 vector3_4 = new Vector3(x, num4 + this.padding.y, 1f);
        int index2 = 0;
        for (int count = uiLabelList.Count; index2 < count; ++index2)
        {
          UILabel uiLabel = uiLabelList[index2];
          NGUITools.AddWidgetCollider(uiLabel.gameObject);
          uiLabel.autoResizeBoxCollider = false;
          BoxCollider component1 = uiLabel.GetComponent<BoxCollider>();
          if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
          {
            vector3_3.z = component1.center.z;
            component1.center = vector3_3;
            component1.size = vector3_4;
          }
          else
          {
            BoxCollider2D component2 = uiLabel.GetComponent<BoxCollider2D>();
            component2.offset = (Vector2) vector3_3;
            component2.size = (Vector2) vector3_4;
          }
        }
        int num6 = Mathf.RoundToInt(f2);
        float f3 = f2 + (float) (((double) border.x + (double) this.padding.x) * 2.0);
        float num7 = y - border.y;
        this.mBackground.width = Mathf.RoundToInt(f3);
        this.mBackground.height = Mathf.RoundToInt(f1);
        int index3 = 0;
        for (int count = uiLabelList.Count; index3 < count; ++index3)
        {
          UILabel uiLabel = uiLabelList[index3];
          uiLabel.overflowMethod = UILabel.Overflow.ShrinkContent;
          uiLabel.width = num6;
        }
        float num8 = (UnityEngine.Object) this.atlas != (UnityEngine.Object) null ? 2f * this.atlas.pixelSize : 2f;
        float f4 = (float) ((double) f3 - ((double) border.x + (double) this.padding.x) * 2.0 + (double) num3 * (double) num8);
        float f5 = num4 + num2 * num8;
        this.mHighlight.width = Mathf.RoundToInt(f4);
        this.mHighlight.height = Mathf.RoundToInt(f5);
        if (this.isAnimated)
        {
          this.AnimateColor((UIWidget) this.mBackground);
          if ((double) Time.timeScale == 0.0 || (double) Time.timeScale >= 0.10000000149011612)
          {
            float bottom = num7 + num4;
            this.Animate((UIWidget) this.mHighlight, placeAbove, bottom);
            int index4 = 0;
            for (int count = uiLabelList.Count; index4 < count; ++index4)
              this.Animate((UIWidget) uiLabelList[index4], placeAbove, bottom);
            this.AnimateScale((UIWidget) this.mBackground, placeAbove, bottom);
          }
        }
        if (placeAbove)
        {
          vector3_1.y = vector3_2.y - border.y;
          vector3_2.y = vector3_1.y + (float) this.mBackground.height;
          vector3_2.x = vector3_1.x + (float) this.mBackground.width;
          transform.localPosition = new Vector3(vector3_1.x, vector3_2.y - border.y, vector3_1.z);
        }
        else
        {
          vector3_2.y = vector3_1.y + border.y;
          vector3_1.y = vector3_2.y - (float) this.mBackground.height;
          vector3_2.x = vector3_1.x + (float) this.mBackground.width;
        }
        Transform parent = this.mPanel.cachedTransform.parent;
        if ((UnityEngine.Object) parent != (UnityEngine.Object) null)
        {
          Vector3 position1 = this.mPanel.cachedTransform.TransformPoint(vector3_1);
          Vector3 position2 = this.mPanel.cachedTransform.TransformPoint(vector3_2);
          vector3_1 = parent.InverseTransformPoint(position1);
          vector3_2 = parent.InverseTransformPoint(position2);
        }
        Vector3 vector3_5 = this.mPanel.hasClipping ? Vector3.zero : this.mPanel.CalculateConstrainOffset((Vector2) vector3_1, (Vector2) vector3_2);
        Vector3 vector3_6 = transform.localPosition + vector3_5;
        vector3_6.x = Mathf.Round(vector3_6.x);
        vector3_6.y = Mathf.Round(vector3_6.y);
        transform.localPosition = vector3_6;
      }
    }
    else
      this.OnSelect(false);
  }

  public enum Position
  {
    Auto,
    Above,
    Below,
  }

  public enum Selection
  {
    OnPress,
    OnClick,
  }

  public enum OpenOn
  {
    ClickOrTap,
    RightClick,
    DoubleClick,
    Manual,
  }

  public delegate void LegacyEvent(string val);
}
