// Decompiled with JetBrains decompiler
// Type: FollowerThoughtObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private float ExpiryIndicator;

  public void Init(ThoughtData t)
  {
    if (t != null)
    {
      this._CanvasGroupThought.alpha = 1f;
      this._CanvasGroup.alpha = 1f;
      this._CanvasGroup.interactable = true;
      this.Description.text = FollowerThoughts.GetLocalisedDescription(t.ThoughtType);
      this.Title.text = FollowerThoughts.GetLocalisedName(t.ThoughtType);
      if (t.Quantity > 1)
      {
        TextMeshProUGUI description = this.Description;
        description.text = $"{description.text} x{(object) t.Quantity}";
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
