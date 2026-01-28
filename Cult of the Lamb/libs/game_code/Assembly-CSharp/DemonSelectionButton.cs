// Decompiled with JetBrains decompiler
// Type: DemonSelectionButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class DemonSelectionButton : 
  BaseMonoBehaviour,
  ISelectHandler,
  IEventSystemHandler,
  IDeselectHandler
{
  [SerializeField]
  public TMP_Text titleText;
  [SerializeField]
  public TMP_Text descriptionText;
  public int demonType;
  public int followerID = -1;

  public void Init(int demonType, int followerID)
  {
    this.demonType = demonType;
    this.followerID = followerID;
  }

  public void OnSelect(BaseEventData eventData)
  {
    if (this.followerID == -1)
      return;
    this.transform.DOScale(Vector3.one * 1.5f, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.titleText.text = DemonModel.GetTitle(this.demonType, this.followerID);
    this.descriptionText.text = DemonModel.GetDescription(this.demonType, FollowerInfo.GetInfoByID(this.followerID));
  }

  public void OnDeselect(BaseEventData eventData)
  {
    this.transform.DOScale(Vector3.one * 1.25f, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }
}
