// Decompiled with JetBrains decompiler
// Type: FollowerThoughtObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FollowerThoughtObject : BaseMonoBehaviour
{
  public TextMeshProUGUI Title;
  public TextMeshProUGUI Description;
  public Image FaithEffect;
  public Sprite FaithDoubleDown;
  public Sprite FaithDown;
  public Sprite FaithUp;
  public Sprite FaithDoubleUp;
  public Color FaithDoubleDownColor;
  public Color FaithDownColor;
  public Color FaithUpColor;
  public Color FaithDoubleUpColor;
  public Image Holder;
  public CanvasGroup _CanvasGroup;
  public CanvasGroup _CanvasGroupThought;
  public float ExpiryIndicator;

  public void Init(ThoughtData t)
  {
    if (t != null)
    {
      this._CanvasGroupThought.alpha = 1f;
      this._CanvasGroup.alpha = 1f;
      this._CanvasGroup.interactable = true;
      this.Description.text = FollowerThoughts.GetLocalisedDescription(t.ThoughtType, t.FollowerID);
      this.Title.text = FollowerThoughts.GetLocalisedName(t.ThoughtType, t.FollowerID);
      if (t.Quantity > 1)
      {
        TextMeshProUGUI description = this.Description;
        description.text = $"{description.text} x{t.Quantity.ToString()}";
      }
      if ((double) t.Modifier <= -7.0)
      {
        this.FaithEffect.sprite = this.FaithDoubleDown;
        this.Holder.color = this.FaithDoubleDownColor;
      }
      else if ((double) t.Modifier < 0.0)
      {
        this.FaithEffect.sprite = this.FaithDown;
        this.Holder.color = this.FaithDownColor;
      }
      else if ((double) t.Modifier >= 7.0)
      {
        this.FaithEffect.sprite = this.FaithDoubleUp;
        this.Holder.color = this.FaithDoubleUpColor;
      }
      else
      {
        if ((double) t.Modifier < 0.0)
          return;
        this.FaithEffect.sprite = this.FaithUp;
        this.Holder.color = this.FaithUpColor;
      }
    }
    else
    {
      this._CanvasGroupThought.alpha = 0.0f;
      this._CanvasGroup.alpha = 1f;
      this._CanvasGroup.interactable = false;
    }
  }
}
