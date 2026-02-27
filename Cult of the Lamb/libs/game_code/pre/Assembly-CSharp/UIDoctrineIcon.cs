// Decompiled with JetBrains decompiler
// Type: UIDoctrineIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class UIDoctrineIcon : 
  BaseMonoBehaviour,
  ISelectHandler,
  IEventSystemHandler,
  IDeselectHandler
{
  public TextMeshProUGUI Name;
  public Image Selected;
  public Image Icon;
  public Image LockIcon;
  public DoctrineUpgradeSystem.DoctrineType Type;
  public bool Locked;

  private void OnEnable() => this.Selected.enabled = false;

  public void Play(DoctrineUpgradeSystem.DoctrineType Type)
  {
    this.Locked = false;
    this.Type = Type;
    this.Name.text = DoctrineUpgradeSystem.GetLocalizedName(Type);
    this.Icon.sprite = DoctrineUpgradeSystem.GetIcon(Type);
    this.LockIcon.enabled = false;
  }

  public void PlayLocked()
  {
    this.Locked = true;
    this.Name.text = "";
    this.Icon.enabled = false;
    this.LockIcon.enabled = true;
  }

  public void OnSelect(BaseEventData eventData)
  {
    this.Selected.enabled = true;
    this.transform.DOScale(Vector3.one * 1.05f, 0.2f);
  }

  public void OnDeselect(BaseEventData eventData)
  {
    this.Selected.enabled = false;
    this.transform.DOScale(Vector3.one, 0.3f);
  }
}
