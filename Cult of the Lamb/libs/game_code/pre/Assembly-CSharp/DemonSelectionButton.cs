// Decompiled with JetBrains decompiler
// Type: DemonSelectionButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private TMP_Text titleText;
  [SerializeField]
  private TMP_Text descriptionText;
  private int demonType;
  private int followerID = -1;

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
