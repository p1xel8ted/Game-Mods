// Decompiled with JetBrains decompiler
// Type: CodeStage.AdvancedFPSCounter.Labels.DrawableLabel
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Text;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace CodeStage.AdvancedFPSCounter.Labels;

public class DrawableLabel
{
  public GameObject container;
  public LabelAnchor anchor;
  public StringBuilder newText;
  public bool dirty;
  public GameObject labelGameObject;
  public RectTransform labelTransform;
  public ContentSizeFitter labelFitter;
  public HorizontalLayoutGroup labelGroup;
  public GameObject uiTextGameObject;
  public UnityEngine.UI.Text uiText;
  public Font font;
  public int fontSize;
  public float lineSpacing;
  public Vector2 pixelOffset;
  public LabelEffect background;
  public Image backgroundImage;
  public LabelEffect shadow;
  public Shadow shadowComponent;
  public LabelEffect outline;
  public Outline outlineComponent;

  public DrawableLabel(
    GameObject container,
    LabelAnchor anchor,
    LabelEffect background,
    LabelEffect shadow,
    LabelEffect outline,
    Font font,
    int fontSize,
    float lineSpacing,
    Vector2 pixelOffset)
  {
    this.container = container;
    this.anchor = anchor;
    this.background = background;
    this.shadow = shadow;
    this.outline = outline;
    this.font = font;
    this.fontSize = fontSize;
    this.lineSpacing = lineSpacing;
    this.pixelOffset = pixelOffset;
    this.NormalizeOffset();
    this.newText = new StringBuilder(1000);
  }

  public void CheckAndUpdate()
  {
    if (this.newText.Length > 0)
    {
      if ((UnityEngine.Object) this.uiText == (UnityEngine.Object) null)
      {
        this.labelGameObject = new GameObject(this.anchor.ToString(), new System.Type[1]
        {
          typeof (RectTransform)
        });
        this.labelTransform = this.labelGameObject.GetComponent<RectTransform>();
        this.labelFitter = this.labelGameObject.AddComponent<ContentSizeFitter>();
        this.labelFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        this.labelFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        this.labelGroup = this.labelGameObject.AddComponent<HorizontalLayoutGroup>();
        this.labelGameObject.layer = this.container.layer;
        this.labelGameObject.tag = this.container.tag;
        this.labelGameObject.transform.SetParent(this.container.transform, false);
        this.uiTextGameObject = new GameObject("Text", new System.Type[1]
        {
          typeof (UnityEngine.UI.Text)
        });
        this.uiTextGameObject.transform.SetParent(this.labelGameObject.transform, false);
        this.uiText = this.uiTextGameObject.GetComponent<UnityEngine.UI.Text>();
        this.uiText.horizontalOverflow = HorizontalWrapMode.Overflow;
        this.uiText.verticalOverflow = VerticalWrapMode.Overflow;
        this.ApplyShadow();
        this.ApplyOutline();
        this.ApplyFont();
        this.uiText.fontSize = this.fontSize;
        this.uiText.lineSpacing = this.lineSpacing;
        this.UpdateTextPosition();
        this.ApplyBackground();
      }
      if (this.dirty)
      {
        this.uiText.text = this.newText.ToString();
        this.ApplyBackground();
        this.dirty = false;
      }
      this.newText.Length = 0;
    }
    else
    {
      if (!((UnityEngine.Object) this.uiText != (UnityEngine.Object) null))
        return;
      this.Clear();
    }
  }

  public void Clear()
  {
    this.newText.Length = 0;
    if ((UnityEngine.Object) this.labelGameObject != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.labelGameObject);
      this.labelGameObject = (GameObject) null;
      this.labelTransform = (RectTransform) null;
      this.uiText = (UnityEngine.UI.Text) null;
    }
    if (!((UnityEngine.Object) this.backgroundImage != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.backgroundImage);
    this.backgroundImage = (Image) null;
  }

  public void Dispose()
  {
    this.Clear();
    this.newText = (StringBuilder) null;
  }

  public void ChangeFont(Font labelsFont)
  {
    this.font = labelsFont;
    this.ApplyFont();
    this.ApplyBackground();
  }

  public void ChangeFontSize(int newSize)
  {
    this.fontSize = newSize;
    if ((UnityEngine.Object) this.uiText != (UnityEngine.Object) null)
      this.uiText.fontSize = this.fontSize;
    this.ApplyBackground();
  }

  public void ChangeOffset(Vector2 newPixelOffset)
  {
    this.pixelOffset = newPixelOffset;
    this.NormalizeOffset();
    if ((UnityEngine.Object) this.labelTransform != (UnityEngine.Object) null)
      this.labelTransform.anchoredPosition = this.pixelOffset;
    this.ApplyBackground();
  }

  public void ChangeLineSpacing(float newValueLineSpacing)
  {
    this.lineSpacing = newValueLineSpacing;
    if ((UnityEngine.Object) this.uiText != (UnityEngine.Object) null)
      this.uiText.lineSpacing = newValueLineSpacing;
    this.ApplyBackground();
  }

  public void ChangeBackground(bool enabled)
  {
    this.background.enabled = enabled;
    this.ApplyBackground();
  }

  public void ChangeBackgroundColor(Color color)
  {
    this.background.color = color;
    this.ApplyBackground();
  }

  public void ChangeBackgroundPadding(int backgroundPadding)
  {
    this.background.padding = backgroundPadding;
    this.ApplyBackground();
  }

  public void ChangeShadow(bool enabled)
  {
    this.shadow.enabled = enabled;
    this.ApplyShadow();
  }

  public void ChangeShadowColor(Color color)
  {
    this.shadow.color = color;
    this.ApplyShadow();
  }

  public void ChangeShadowDistance(Vector2 distance)
  {
    this.shadow.distance = distance;
    this.ApplyShadow();
  }

  public void ChangeOutline(bool enabled)
  {
    this.outline.enabled = enabled;
    this.ApplyOutline();
  }

  public void ChangeOutlineColor(Color color)
  {
    this.outline.color = color;
    this.ApplyOutline();
  }

  public void ChangeOutlineDistance(Vector2 distance)
  {
    this.outline.distance = distance;
    this.ApplyOutline();
  }

  public void UpdateTextPosition()
  {
    this.labelTransform.localRotation = Quaternion.identity;
    this.labelTransform.sizeDelta = Vector2.zero;
    this.labelTransform.anchoredPosition = this.pixelOffset;
    switch (this.anchor)
    {
      case LabelAnchor.UpperLeft:
        this.uiText.alignment = TextAnchor.UpperLeft;
        this.labelTransform.anchorMin = Vector2.up;
        this.labelTransform.anchorMax = Vector2.up;
        this.labelTransform.pivot = new Vector2(0.0f, 1f);
        break;
      case LabelAnchor.UpperRight:
        this.uiText.alignment = TextAnchor.UpperRight;
        this.labelTransform.anchorMin = Vector2.one;
        this.labelTransform.anchorMax = Vector2.one;
        this.labelTransform.pivot = new Vector2(1f, 1f);
        break;
      case LabelAnchor.LowerLeft:
        this.uiText.alignment = TextAnchor.LowerLeft;
        this.labelTransform.anchorMin = Vector2.zero;
        this.labelTransform.anchorMax = Vector2.zero;
        this.labelTransform.pivot = new Vector2(0.0f, 0.0f);
        break;
      case LabelAnchor.LowerRight:
        this.uiText.alignment = TextAnchor.LowerRight;
        this.labelTransform.anchorMin = Vector2.right;
        this.labelTransform.anchorMax = Vector2.right;
        this.labelTransform.pivot = new Vector2(1f, 0.0f);
        break;
      case LabelAnchor.UpperCenter:
        this.uiText.alignment = TextAnchor.UpperCenter;
        this.labelTransform.anchorMin = new Vector2(0.5f, 1f);
        this.labelTransform.anchorMax = new Vector2(0.5f, 1f);
        this.labelTransform.pivot = new Vector2(0.5f, 1f);
        break;
      case LabelAnchor.LowerCenter:
        this.uiText.alignment = TextAnchor.LowerCenter;
        this.labelTransform.anchorMin = new Vector2(0.5f, 0.0f);
        this.labelTransform.anchorMax = new Vector2(0.5f, 0.0f);
        this.labelTransform.pivot = new Vector2(0.5f, 0.0f);
        break;
      default:
        Debug.LogWarning((object) "<b>[AFPSCounter]:</b> Unknown label anchor!", (UnityEngine.Object) this.uiText);
        this.uiText.alignment = TextAnchor.UpperLeft;
        this.labelTransform.anchorMin = Vector2.up;
        this.labelTransform.anchorMax = Vector2.up;
        break;
    }
  }

  public void NormalizeOffset()
  {
    switch (this.anchor)
    {
      case LabelAnchor.UpperLeft:
        this.pixelOffset.y = -this.pixelOffset.y;
        break;
      case LabelAnchor.UpperRight:
        this.pixelOffset.x = -this.pixelOffset.x;
        this.pixelOffset.y = -this.pixelOffset.y;
        break;
      case LabelAnchor.LowerLeft:
        break;
      case LabelAnchor.LowerRight:
        this.pixelOffset.x = -this.pixelOffset.x;
        break;
      case LabelAnchor.UpperCenter:
        this.pixelOffset.y = -this.pixelOffset.y;
        this.pixelOffset.x = 0.0f;
        break;
      case LabelAnchor.LowerCenter:
        this.pixelOffset.x = 0.0f;
        break;
      default:
        this.pixelOffset.y = -this.pixelOffset.y;
        break;
    }
  }

  public void ApplyBackground()
  {
    if ((UnityEngine.Object) this.uiText == (UnityEngine.Object) null)
      return;
    if (this.background.enabled && !(bool) (UnityEngine.Object) this.backgroundImage)
      this.backgroundImage = this.labelGameObject.AddComponent<Image>();
    if (!this.background.enabled && (bool) (UnityEngine.Object) this.backgroundImage)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.backgroundImage);
      this.backgroundImage = (Image) null;
    }
    if (!((UnityEngine.Object) this.backgroundImage != (UnityEngine.Object) null))
      return;
    if (this.backgroundImage.color != this.background.color)
      this.backgroundImage.color = this.background.color;
    if (this.labelGroup.padding.bottom == this.background.padding)
      return;
    this.labelGroup.padding.top = this.background.padding;
    this.labelGroup.padding.left = this.background.padding;
    this.labelGroup.padding.right = this.background.padding;
    this.labelGroup.padding.bottom = this.background.padding;
    this.labelGroup.SetLayoutHorizontal();
  }

  public void ApplyShadow()
  {
    if ((UnityEngine.Object) this.uiText == (UnityEngine.Object) null)
      return;
    if (this.shadow.enabled && !(bool) (UnityEngine.Object) this.shadowComponent)
      this.shadowComponent = this.uiTextGameObject.AddComponent<Shadow>();
    if (!this.shadow.enabled && (bool) (UnityEngine.Object) this.shadowComponent)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.shadowComponent);
    if (!((UnityEngine.Object) this.shadowComponent != (UnityEngine.Object) null))
      return;
    this.shadowComponent.effectColor = this.shadow.color;
    this.shadowComponent.effectDistance = this.shadow.distance;
  }

  public void ApplyOutline()
  {
    if ((UnityEngine.Object) this.uiText == (UnityEngine.Object) null)
      return;
    if (this.outline.enabled && !(bool) (UnityEngine.Object) this.outlineComponent)
      this.outlineComponent = this.uiTextGameObject.AddComponent<Outline>();
    if (!this.outline.enabled && (bool) (UnityEngine.Object) this.outlineComponent)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.outlineComponent);
    if (!((UnityEngine.Object) this.outlineComponent != (UnityEngine.Object) null))
      return;
    this.outlineComponent.effectColor = this.outline.color;
    this.outlineComponent.effectDistance = this.outline.distance;
  }

  public void ApplyFont()
  {
    if ((UnityEngine.Object) this.uiText == (UnityEngine.Object) null)
      return;
    if ((UnityEngine.Object) this.font == (UnityEngine.Object) null)
      this.font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
    this.uiText.font = this.font;
  }
}
