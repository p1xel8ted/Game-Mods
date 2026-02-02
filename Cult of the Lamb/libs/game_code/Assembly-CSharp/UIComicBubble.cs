// Decompiled with JetBrains decompiler
// Type: UIComicBubble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[ExecuteInEditMode]
public class UIComicBubble : MonoBehaviour
{
  [SerializeField]
  public Image bubble;
  [SerializeField]
  public TMP_Text text;
  [HideInInspector]
  public string Term;
  [HideInInspector]
  public float Width = 200f;
  [HideInInspector]
  public float Height = -1f;
  [HideInInspector]
  public UIComicBubble.BubbleVariant Variant;
  [HideInInspector]
  public float FontSize = 24f;
  [HideInInspector]
  public float Rotation;
  [HideInInspector]
  public UIComicBubble.BubbleLanguageData[] BubbleData;

  public Image BubbleBackground => this.bubble;

  public void OnEnable()
  {
    if (this.BubbleData == null || this.BubbleData.Length == 0)
    {
      this.BubbleData = new UIComicBubble.BubbleLanguageData[UIComicMenuController.LanguageCodes.Count];
      for (int index = 0; index < this.BubbleData.Length; ++index)
      {
        this.BubbleData[index] = new UIComicBubble.BubbleLanguageData();
        this.BubbleData[index].FontSize = this.FontSize;
        this.BubbleData[index].Width = this.Width;
        this.BubbleData[index].Height = this.Height;
        this.BubbleData[index].Rotation = this.Rotation;
      }
    }
    this.ForceUpdateBubble();
  }

  public void ForceUpdateBubble()
  {
    List<UIComicBubble.BubbleLanguageData> list;
    if (this.BubbleData.Length <= 10)
    {
      for (; this.BubbleData.Length < UIComicMenuController.LanguageCodes.Count; this.BubbleData = list.ToArray())
      {
        UIComicBubble.BubbleLanguageData bubbleLanguageData = new UIComicBubble.BubbleLanguageData();
        bubbleLanguageData.FontSize = this.BubbleData[1].FontSize;
        bubbleLanguageData.Width = this.BubbleData[1].Width;
        bubbleLanguageData.Height = this.BubbleData[1].Height;
        bubbleLanguageData.Rotation = this.BubbleData[1].Rotation;
        list = ((IEnumerable<UIComicBubble.BubbleLanguageData>) this.BubbleData).ToList<UIComicBubble.BubbleLanguageData>();
        list.Add(bubbleLanguageData);
      }
    }
    this.text.text = LocalizationManager.GetTranslation(this.Term)?.ToUpper();
    UIComicBubble.BubbleLanguageData bubbleLanguageData1 = this.BubbleData[UIComicMenuController.LanguageCodes.IndexOf(LocalizationManager.CurrentLanguageCode)];
    this.FontSize = bubbleLanguageData1.FontSize;
    this.Width = bubbleLanguageData1.Width;
    this.Height = bubbleLanguageData1.Height;
    this.Rotation = bubbleLanguageData1.Rotation;
  }

  public void SetText(string term, float fontSize, float rotation, float width, float height)
  {
    this.Term = term;
    this.text.text = string.IsNullOrEmpty(term) ? "" : LocalizationManager.GetTranslation(term).ToUpper();
    this.text.isRightToLeftText = LocalizeIntegration.IsArabic();
    this.text.fontSize = fontSize;
    this.FontSize = fontSize;
    this.Rotation = rotation;
    this.Width = width;
    this.Height = (double) height == -1.0 ? ((RectTransform) this.text.transform).sizeDelta.y : height;
    this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, this.Rotation);
    ((RectTransform) this.text.transform).sizeDelta = new Vector2(width, height);
    VerticalLayoutGroup component1 = this.GetComponent<VerticalLayoutGroup>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      component1.childControlHeight = false;
    ContentSizeFitter component2 = this.text.GetComponent<ContentSizeFitter>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      component2.enabled = false;
    UIComicBubble.BubbleLanguageData bubbleLanguageData = this.BubbleData[UIComicMenuController.LanguageCodes.IndexOf(LocalizationManager.CurrentLanguageCode)];
    bubbleLanguageData.FontSize = fontSize;
    bubbleLanguageData.Width = this.Width;
    bubbleLanguageData.Height = this.Height;
    bubbleLanguageData.Rotation = this.Rotation;
  }

  public enum BubbleVariant
  {
    Default,
    WhiteBlackOutline,
    BlackWhiteOutline,
    White,
    EmptyBlackText,
    EmptyWhiteText,
  }

  [Serializable]
  public class BubbleLanguageData
  {
    public float FontSize = 24f;
    public float Width = 200f;
    public float Height = -1f;
    public float Rotation;
  }
}
