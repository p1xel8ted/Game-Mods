// Decompiled with JetBrains decompiler
// Type: CorruptedTarotPiece
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class CorruptedTarotPiece : MonoBehaviour
{
  public SkeletonGraphic skeletonGraphic;

  public void OnValidate() => this.UpdateGraphic();

  public void OnEnable() => this.UpdateGraphic();

  public void UpdateGraphic()
  {
    this.skeletonGraphic = this.gameObject.GetComponent<SkeletonGraphic>();
    string slotName = "Card";
    if ((Object) this.skeletonGraphic == (Object) null || this.skeletonGraphic.Skeleton == null)
      return;
    Attachment attachment = this.skeletonGraphic.Skeleton.GetAttachment(slotName, this.gameObject.name);
    if (attachment == null)
    {
      Debug.LogError((object) $"Clip attachment not found: {this.gameObject.name} in slot: {slotName}");
    }
    else
    {
      Slot slot = this.skeletonGraphic.Skeleton.FindSlot(slotName);
      if (slot == null)
        Debug.LogError((object) ("Slot not found: " + slotName));
      else
        slot.Attachment = attachment;
    }
  }
}
