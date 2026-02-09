// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.ThemeSettings
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Rewired.UI.ControlMapper;

[Serializable]
public class ThemeSettings : ScriptableObject
{
  [SerializeField]
  public ThemeSettings.ImageSettings _mainWindowBackground;
  [SerializeField]
  public ThemeSettings.ImageSettings _popupWindowBackground;
  [SerializeField]
  public ThemeSettings.ImageSettings _areaBackground;
  [SerializeField]
  public ThemeSettings.SelectableSettings _selectableSettings;
  [SerializeField]
  public ThemeSettings.SelectableSettings _buttonSettings;
  [SerializeField]
  public ThemeSettings.SelectableSettings _inputGridFieldSettings;
  [SerializeField]
  public ThemeSettings.ScrollbarSettings _scrollbarSettings;
  [SerializeField]
  public ThemeSettings.SliderSettings _sliderSettings;
  [SerializeField]
  public ThemeSettings.ImageSettings _invertToggle;
  [SerializeField]
  public Color _invertToggleDisabledColor;
  [SerializeField]
  public ThemeSettings.ImageSettings _calibrationValueMarker;
  [SerializeField]
  public ThemeSettings.ImageSettings _calibrationRawValueMarker;
  [SerializeField]
  public ThemeSettings.TextSettings _textSettings;
  [SerializeField]
  public ThemeSettings.TextSettings _buttonTextSettings;
  [SerializeField]
  public ThemeSettings.TextSettings _inputGridFieldTextSettings;

  public void Apply(ThemedElement.ElementInfo[] elementInfo)
  {
    if (elementInfo == null)
      return;
    for (int index = 0; index < elementInfo.Length; ++index)
    {
      if (elementInfo[index] != null)
        this.Apply(elementInfo[index].themeClass, elementInfo[index].component);
    }
  }

  public void Apply(string themeClass, Component component)
  {
    if ((UnityEngine.Object) (component as Selectable) != (UnityEngine.Object) null)
      this.Apply(themeClass, (Selectable) component);
    else if ((UnityEngine.Object) (component as Image) != (UnityEngine.Object) null)
      this.Apply(themeClass, (Image) component);
    else if ((UnityEngine.Object) (component as Text) != (UnityEngine.Object) null)
    {
      this.Apply(themeClass, (Text) component);
    }
    else
    {
      if (!((UnityEngine.Object) (component as UIImageHelper) != (UnityEngine.Object) null))
        return;
      this.Apply(themeClass, (UIImageHelper) component);
    }
  }

  public void Apply(string themeClass, Selectable item)
  {
    if ((UnityEngine.Object) item == (UnityEngine.Object) null)
      return;
    ThemeSettings.SelectableSettings_Base selectableSettingsBase;
    if ((UnityEngine.Object) (item as UnityEngine.UI.Button) != (UnityEngine.Object) null)
    {
      switch (themeClass)
      {
        case "inputGridField":
          selectableSettingsBase = (ThemeSettings.SelectableSettings_Base) this._inputGridFieldSettings;
          break;
        default:
          selectableSettingsBase = (ThemeSettings.SelectableSettings_Base) this._buttonSettings;
          break;
      }
    }
    else if ((UnityEngine.Object) (item as Scrollbar) != (UnityEngine.Object) null)
      selectableSettingsBase = (ThemeSettings.SelectableSettings_Base) this._scrollbarSettings;
    else if ((UnityEngine.Object) (item as Slider) != (UnityEngine.Object) null)
      selectableSettingsBase = (ThemeSettings.SelectableSettings_Base) this._sliderSettings;
    else if ((UnityEngine.Object) (item as Toggle) != (UnityEngine.Object) null)
    {
      switch (themeClass)
      {
        case "button":
          selectableSettingsBase = (ThemeSettings.SelectableSettings_Base) this._buttonSettings;
          break;
        default:
          selectableSettingsBase = (ThemeSettings.SelectableSettings_Base) this._selectableSettings;
          break;
      }
    }
    else
      selectableSettingsBase = (ThemeSettings.SelectableSettings_Base) this._selectableSettings;
    selectableSettingsBase.Apply(item);
  }

  public void Apply(string themeClass, Image item)
  {
    if ((UnityEngine.Object) item == (UnityEngine.Object) null || themeClass == null)
      return;
    // ISSUE: reference to a compiler-generated method
    switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(themeClass))
    {
      case 106194061:
        if (!(themeClass == "invertToggleButtonBackground"))
          break;
        this._buttonSettings.imageSettings.CopyTo(item);
        break;
      case 283896133:
        if (!(themeClass == "popupWindow"))
          break;
        this._popupWindowBackground.CopyTo(item);
        break;
      case 2601460036:
        if (!(themeClass == "area"))
          break;
        this._areaBackground.CopyTo(item);
        break;
      case 2822822017:
        if (!(themeClass == "invertToggle"))
          break;
        this._invertToggle.CopyTo(item);
        break;
      case 2998767316:
        if (!(themeClass == "mainWindow"))
          break;
        this._mainWindowBackground.CopyTo(item);
        break;
      case 3490313510:
        if (!(themeClass == "calibrationRawValueMarker"))
          break;
        this._calibrationRawValueMarker.CopyTo(item);
        break;
      case 3776179782:
        if (!(themeClass == "calibrationValueMarker"))
          break;
        this._calibrationValueMarker.CopyTo(item);
        break;
      case 3911450241:
        if (!(themeClass == "invertToggleBackground"))
          break;
        this._inputGridFieldSettings.imageSettings.CopyTo(item);
        break;
    }
  }

  public void Apply(string themeClass, Text item)
  {
    if ((UnityEngine.Object) item == (UnityEngine.Object) null)
      return;
    ThemeSettings.TextSettings textSettings;
    switch (themeClass)
    {
      case "button":
        textSettings = this._buttonTextSettings;
        break;
      case "inputGridField":
        textSettings = this._inputGridFieldTextSettings;
        break;
      default:
        textSettings = this._textSettings;
        break;
    }
    if ((UnityEngine.Object) textSettings.font != (UnityEngine.Object) null)
      item.font = textSettings.font;
    item.color = textSettings.color;
    item.lineSpacing = textSettings.lineSpacing;
    if ((double) textSettings.sizeMultiplier != 1.0)
    {
      item.fontSize = (int) ((double) item.fontSize * (double) textSettings.sizeMultiplier);
      item.resizeTextMaxSize = (int) ((double) item.resizeTextMaxSize * (double) textSettings.sizeMultiplier);
      item.resizeTextMinSize = (int) ((double) item.resizeTextMinSize * (double) textSettings.sizeMultiplier);
    }
    if (textSettings.style == ThemeSettings.FontStyleOverride.Default)
      return;
    item.fontStyle = (FontStyle) (textSettings.style - 1);
  }

  public void Apply(string themeClass, UIImageHelper item)
  {
    if ((UnityEngine.Object) item == (UnityEngine.Object) null)
      return;
    item.SetEnabledStateColor(this._invertToggle.color);
    item.SetDisabledStateColor(this._invertToggleDisabledColor);
    item.Refresh();
  }

  [Serializable]
  public abstract class SelectableSettings_Base
  {
    [SerializeField]
    public Selectable.Transition _transition;
    [SerializeField]
    public ThemeSettings.CustomColorBlock _colors;
    [SerializeField]
    public ThemeSettings.CustomSpriteState _spriteState;
    [SerializeField]
    public ThemeSettings.CustomAnimationTriggers _animationTriggers;

    public Selectable.Transition transition => this._transition;

    public ThemeSettings.CustomColorBlock selectableColors => this._colors;

    public ThemeSettings.CustomSpriteState spriteState => this._spriteState;

    public ThemeSettings.CustomAnimationTriggers animationTriggers => this._animationTriggers;

    public virtual void Apply(Selectable item)
    {
      Selectable.Transition transition = this._transition;
      int num = item.transition != transition ? 1 : 0;
      item.transition = transition;
      ICustomSelectable customSelectable = item as ICustomSelectable;
      switch (transition)
      {
        case Selectable.Transition.ColorTint:
          ThemeSettings.CustomColorBlock colors = this._colors with
          {
            fadeDuration = 0.0f
          };
          item.colors = (ColorBlock) colors;
          colors.fadeDuration = this._colors.fadeDuration;
          item.colors = (ColorBlock) colors;
          if (customSelectable != null)
          {
            customSelectable.disabledHighlightedColor = colors.disabledHighlightedColor;
            break;
          }
          break;
        case Selectable.Transition.SpriteSwap:
          item.spriteState = (SpriteState) this._spriteState;
          if (customSelectable != null)
          {
            customSelectable.disabledHighlightedSprite = this._spriteState.disabledHighlightedSprite;
            break;
          }
          break;
        case Selectable.Transition.Animation:
          item.animationTriggers.disabledTrigger = this._animationTriggers.disabledTrigger;
          item.animationTriggers.highlightedTrigger = this._animationTriggers.highlightedTrigger;
          item.animationTriggers.normalTrigger = this._animationTriggers.normalTrigger;
          item.animationTriggers.pressedTrigger = this._animationTriggers.pressedTrigger;
          if (customSelectable != null)
          {
            customSelectable.disabledHighlightedTrigger = this._animationTriggers.disabledHighlightedTrigger;
            break;
          }
          break;
      }
      if (num == 0)
        return;
      item.targetGraphic.CrossFadeColor(item.targetGraphic.color, 0.0f, true, true);
    }
  }

  [Serializable]
  public class SelectableSettings : ThemeSettings.SelectableSettings_Base
  {
    [SerializeField]
    public ThemeSettings.ImageSettings _imageSettings;

    public ThemeSettings.ImageSettings imageSettings => this._imageSettings;

    public override void Apply(Selectable item)
    {
      if ((UnityEngine.Object) item == (UnityEngine.Object) null)
        return;
      base.Apply(item);
      if (this._imageSettings == null)
        return;
      this._imageSettings.CopyTo(item.targetGraphic as Image);
    }
  }

  [Serializable]
  public class SliderSettings : ThemeSettings.SelectableSettings_Base
  {
    [SerializeField]
    public ThemeSettings.ImageSettings _handleImageSettings;
    [SerializeField]
    public ThemeSettings.ImageSettings _fillImageSettings;
    [SerializeField]
    public ThemeSettings.ImageSettings _backgroundImageSettings;

    public ThemeSettings.ImageSettings handleImageSettings => this._handleImageSettings;

    public ThemeSettings.ImageSettings fillImageSettings => this._fillImageSettings;

    public ThemeSettings.ImageSettings backgroundImageSettings => this._backgroundImageSettings;

    public void Apply(Slider item)
    {
      if ((UnityEngine.Object) item == (UnityEngine.Object) null)
        return;
      if (this._handleImageSettings != null)
        this._handleImageSettings.CopyTo(item.targetGraphic as Image);
      if (this._fillImageSettings != null)
      {
        RectTransform fillRect = item.fillRect;
        if ((UnityEngine.Object) fillRect != (UnityEngine.Object) null)
          this._fillImageSettings.CopyTo(fillRect.GetComponent<Image>());
      }
      if (this._backgroundImageSettings == null)
        return;
      Transform transform = item.transform.Find("Background");
      if (!((UnityEngine.Object) transform != (UnityEngine.Object) null))
        return;
      this._backgroundImageSettings.CopyTo(transform.GetComponent<Image>());
    }

    public override void Apply(Selectable item)
    {
      base.Apply(item);
      this.Apply(item as Slider);
    }
  }

  [Serializable]
  public class ScrollbarSettings : ThemeSettings.SelectableSettings_Base
  {
    [SerializeField]
    public ThemeSettings.ImageSettings _handleImageSettings;
    [SerializeField]
    public ThemeSettings.ImageSettings _backgroundImageSettings;

    public ThemeSettings.ImageSettings handle => this._handleImageSettings;

    public ThemeSettings.ImageSettings background => this._backgroundImageSettings;

    public void Apply(Scrollbar item)
    {
      if ((UnityEngine.Object) item == (UnityEngine.Object) null)
        return;
      if (this._handleImageSettings != null)
        this._handleImageSettings.CopyTo(item.targetGraphic as Image);
      if (this._backgroundImageSettings == null)
        return;
      this._backgroundImageSettings.CopyTo(item.GetComponent<Image>());
    }

    public override void Apply(Selectable item)
    {
      base.Apply(item);
      this.Apply(item as Scrollbar);
    }
  }

  [Serializable]
  public class ImageSettings
  {
    [SerializeField]
    public Color _color = Color.white;
    [SerializeField]
    public UnityEngine.Sprite _sprite;
    [SerializeField]
    public Material _materal;
    [SerializeField]
    public Image.Type _type;
    [SerializeField]
    public bool _preserveAspect;
    [SerializeField]
    public bool _fillCenter;
    [SerializeField]
    public Image.FillMethod _fillMethod;
    [SerializeField]
    public float _fillAmout;
    [SerializeField]
    public bool _fillClockwise;
    [SerializeField]
    public int _fillOrigin;

    public Color color => this._color;

    public UnityEngine.Sprite sprite => this._sprite;

    public Material materal => this._materal;

    public Image.Type type => this._type;

    public bool preserveAspect => this._preserveAspect;

    public bool fillCenter => this._fillCenter;

    public Image.FillMethod fillMethod => this._fillMethod;

    public float fillAmout => this._fillAmout;

    public bool fillClockwise => this._fillClockwise;

    public int fillOrigin => this._fillOrigin;

    public virtual void CopyTo(Image image)
    {
      if ((UnityEngine.Object) image == (UnityEngine.Object) null)
        return;
      image.color = this._color;
      image.sprite = this._sprite;
      image.material = this._materal;
      image.type = this._type;
      image.preserveAspect = this._preserveAspect;
      image.fillCenter = this._fillCenter;
      image.fillMethod = this._fillMethod;
      image.fillAmount = this._fillAmout;
      image.fillClockwise = this._fillClockwise;
      image.fillOrigin = this._fillOrigin;
    }
  }

  [Serializable]
  public struct CustomColorBlock
  {
    [SerializeField]
    public float m_ColorMultiplier;
    [SerializeField]
    public Color m_DisabledColor;
    [SerializeField]
    public float m_FadeDuration;
    [SerializeField]
    public Color m_HighlightedColor;
    [SerializeField]
    public Color m_NormalColor;
    [SerializeField]
    public Color m_PressedColor;
    [SerializeField]
    public Color m_DisabledHighlightedColor;

    public float colorMultiplier
    {
      get => this.m_ColorMultiplier;
      set => this.m_ColorMultiplier = value;
    }

    public Color disabledColor
    {
      get => this.m_DisabledColor;
      set => this.m_DisabledColor = value;
    }

    public float fadeDuration
    {
      get => this.m_FadeDuration;
      set => this.m_FadeDuration = value;
    }

    public Color highlightedColor
    {
      get => this.m_HighlightedColor;
      set => this.m_HighlightedColor = value;
    }

    public Color normalColor
    {
      get => this.m_NormalColor;
      set => this.m_NormalColor = value;
    }

    public Color pressedColor
    {
      get => this.m_PressedColor;
      set => this.m_PressedColor = value;
    }

    public Color disabledHighlightedColor
    {
      get => this.m_DisabledHighlightedColor;
      set => this.m_DisabledHighlightedColor = value;
    }

    public static implicit operator ColorBlock(ThemeSettings.CustomColorBlock item)
    {
      return new ColorBlock()
      {
        colorMultiplier = item.m_ColorMultiplier,
        disabledColor = item.m_DisabledColor,
        fadeDuration = item.m_FadeDuration,
        highlightedColor = item.m_HighlightedColor,
        normalColor = item.m_NormalColor,
        pressedColor = item.m_PressedColor
      };
    }
  }

  [Serializable]
  public struct CustomSpriteState
  {
    [SerializeField]
    public UnityEngine.Sprite m_DisabledSprite;
    [SerializeField]
    public UnityEngine.Sprite m_HighlightedSprite;
    [SerializeField]
    public UnityEngine.Sprite m_PressedSprite;
    [SerializeField]
    public UnityEngine.Sprite m_DisabledHighlightedSprite;

    public UnityEngine.Sprite disabledSprite
    {
      get => this.m_DisabledSprite;
      set => this.m_DisabledSprite = value;
    }

    public UnityEngine.Sprite highlightedSprite
    {
      get => this.m_HighlightedSprite;
      set => this.m_HighlightedSprite = value;
    }

    public UnityEngine.Sprite pressedSprite
    {
      get => this.m_PressedSprite;
      set => this.m_PressedSprite = value;
    }

    public UnityEngine.Sprite disabledHighlightedSprite
    {
      get => this.m_DisabledHighlightedSprite;
      set => this.m_DisabledHighlightedSprite = value;
    }

    public static implicit operator SpriteState(ThemeSettings.CustomSpriteState item)
    {
      return new SpriteState()
      {
        disabledSprite = item.m_DisabledSprite,
        highlightedSprite = item.m_HighlightedSprite,
        pressedSprite = item.m_PressedSprite
      };
    }
  }

  [Serializable]
  public class CustomAnimationTriggers
  {
    [SerializeField]
    public string m_DisabledTrigger;
    [SerializeField]
    public string m_HighlightedTrigger;
    [SerializeField]
    public string m_NormalTrigger;
    [SerializeField]
    public string m_PressedTrigger;
    [SerializeField]
    public string m_DisabledHighlightedTrigger;

    public CustomAnimationTriggers()
    {
      this.m_DisabledTrigger = string.Empty;
      this.m_HighlightedTrigger = string.Empty;
      this.m_NormalTrigger = string.Empty;
      this.m_PressedTrigger = string.Empty;
      this.m_DisabledHighlightedTrigger = string.Empty;
    }

    public string disabledTrigger
    {
      get => this.m_DisabledTrigger;
      set => this.m_DisabledTrigger = value;
    }

    public string highlightedTrigger
    {
      get => this.m_HighlightedTrigger;
      set => this.m_HighlightedTrigger = value;
    }

    public string normalTrigger
    {
      get => this.m_NormalTrigger;
      set => this.m_NormalTrigger = value;
    }

    public string pressedTrigger
    {
      get => this.m_PressedTrigger;
      set => this.m_PressedTrigger = value;
    }

    public string disabledHighlightedTrigger
    {
      get => this.m_DisabledHighlightedTrigger;
      set => this.m_DisabledHighlightedTrigger = value;
    }

    public static implicit operator AnimationTriggers(ThemeSettings.CustomAnimationTriggers item)
    {
      return new AnimationTriggers()
      {
        disabledTrigger = item.m_DisabledTrigger,
        highlightedTrigger = item.m_HighlightedTrigger,
        normalTrigger = item.m_NormalTrigger,
        pressedTrigger = item.m_PressedTrigger
      };
    }
  }

  [Serializable]
  public class TextSettings
  {
    [SerializeField]
    public Color _color = Color.white;
    [SerializeField]
    public Font _font;
    [SerializeField]
    public ThemeSettings.FontStyleOverride _style;
    [SerializeField]
    public float _lineSpacing = 1f;
    [SerializeField]
    public float _sizeMultiplier = 1f;

    public Color color => this._color;

    public Font font => this._font;

    public ThemeSettings.FontStyleOverride style => this._style;

    public float lineSpacing => this._lineSpacing;

    public float sizeMultiplier => this._sizeMultiplier;
  }

  public enum FontStyleOverride
  {
    Default,
    Normal,
    Bold,
    Italic,
    BoldAndItalic,
  }
}
