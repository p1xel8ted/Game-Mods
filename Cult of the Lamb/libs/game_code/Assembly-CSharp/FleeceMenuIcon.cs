// Decompiled with JetBrains decompiler
// Type: FleeceMenuIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class FleeceMenuIcon : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
  public int FleeceNumber;
  public Image LockedIcon;
  public Image FleeceIcon;
  public RectTransform Container;
  public Image FleeceOutline;
  public Image FleeceEquipped;
  public Material normalMaterial;
  public Material bwMaterial;
  public FleeceMenuIcon.States State;

  public void Init()
  {
    this.State = !DataManager.Instance.UnlockedFleeces.Contains(this.FleeceNumber) ? (Inventory.TempleKeys > 0 ? FleeceMenuIcon.States.Available : FleeceMenuIcon.States.Locked) : (DataManager.Instance.PlayerFleece == this.FleeceNumber ? FleeceMenuIcon.States.Equipped : FleeceMenuIcon.States.Unlocked);
    this.FleeceEquipped.enabled = false;
    this.FleeceOutline.enabled = false;
    this.FleeceIcon.material = this.bwMaterial;
    switch (this.State)
    {
      case FleeceMenuIcon.States.Locked:
        this.LockedIcon.enabled = true;
        this.FleeceIcon.color = Color.black;
        break;
      case FleeceMenuIcon.States.Unlocked:
        this.LockedIcon.enabled = false;
        this.FleeceIcon.color = Color.white;
        break;
      case FleeceMenuIcon.States.Available:
        this.LockedIcon.enabled = true;
        this.FleeceIcon.color = Color.black;
        break;
      case FleeceMenuIcon.States.Equipped:
        this.LockedIcon.enabled = false;
        this.FleeceIcon.color = Color.white;
        this.FleeceEquipped.enabled = true;
        break;
    }
  }

  public void OnSelect(BaseEventData eventData)
  {
    this.Container.DOKill();
    this.Container.DOScale(Vector3.one * 1.1f, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.FleeceOutline.enabled = true;
    this.FleeceIcon.material = this.normalMaterial;
  }

  public void OnDeselect(BaseEventData eventData)
  {
    this.Container.DOKill();
    this.Container.DOScale(Vector3.one * 1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.FleeceOutline.enabled = false;
    this.FleeceIcon.material = this.bwMaterial;
  }

  public enum States
  {
    Locked,
    Unlocked,
    Available,
    Equipped,
  }
}
