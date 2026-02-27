// Decompiled with JetBrains decompiler
// Type: UINewRecruitSelectable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using Spine.Unity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class UINewRecruitSelectable : 
  BaseMonoBehaviour,
  ISelectHandler,
  IEventSystemHandler,
  IDeselectHandler
{
  public int FormIndex;
  public int SkinVariationIndex;
  public int SkinColour;
  public SkeletonGraphic Spine;
  public MMSelectable Selectable;
  public Image Image;
  public RectTransform rectTransform;
  public GameObject Locked;
  public GameObject NewSkinIcon;
  public GameObject highlightedBorder;
  public GameObject selectedIcon;
  public Material blackWhiteMaterial;
  public bool Random;
  private string skin = "";

  public bool IsLocked { get; private set; }

  public void Show(string skin = "")
  {
    this.Locked.SetActive(false);
    this.Selectable.interactable = true;
    this.highlightedBorder.SetActive(false);
    if ((bool) (Object) this.Spine)
      this.Spine.enabled = this.Selectable.interactable = true;
    this.Image.color = new Color(0.0f, 0.0f, 0.0f, 0.2f);
    this.skin = skin;
  }

  public void Hide()
  {
    this.Locked.SetActive(false);
    this.Spine.enabled = this.Selectable.interactable = false;
    this.Image.color = new Color(1f, 1f, 1f, 0.0f);
  }

  public void Lock()
  {
    this.Locked.SetActive(true);
    this.highlightedBorder.SetActive(false);
    if ((bool) (Object) this.Spine)
    {
      this.Spine.material = this.blackWhiteMaterial;
      this.Spine.material.SetFloat("_GrayscaleLerpFade", 1f);
    }
    this.IsLocked = true;
  }

  public void OnSelect(BaseEventData eventData)
  {
    this.highlightedBorder.SetActive(true);
    if (this.IsLocked)
      return;
    this.Image.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
  }

  public void OnDeselect(BaseEventData eventData)
  {
    this.highlightedBorder.SetActive(false);
    if (this.IsLocked)
      return;
    this.Image.color = new Color(0.0f, 0.0f, 0.0f, 0.2f);
  }

  public void SetSelected() => this.selectedIcon.SetActive(true);

  public void SetUnselected() => this.selectedIcon.SetActive(false);
}
