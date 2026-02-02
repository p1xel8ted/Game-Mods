// Decompiled with JetBrains decompiler
// Type: SpecialBaalAndAym
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class SpecialBaalAndAym : MonoBehaviour
{
  [SerializeField]
  public SkeletonAnimation baalSpine;
  [SerializeField]
  public SkeletonAnimation aymSpine;

  public void Start()
  {
    if ((Object) this.baalSpine != (Object) null)
    {
      foreach (WorshipperData.SlotAndColor slotAndColour in WorshipperData.Instance.GetColourData("Boss Baal").SlotAndColours[0].SlotAndColours)
      {
        Slot slot = this.baalSpine.Skeleton.FindSlot(slotAndColour.Slot);
        if (slot != null)
          slot.SetColor(slotAndColour.color);
      }
    }
    if (!((Object) this.aymSpine != (Object) null))
      return;
    foreach (WorshipperData.SlotAndColor slotAndColour in WorshipperData.Instance.GetColourData("Boss Aym").SlotAndColours[0].SlotAndColours)
    {
      Slot slot = this.aymSpine.Skeleton.FindSlot(slotAndColour.Slot);
      if (slot != null)
        slot.SetColor(slotAndColour.color);
    }
  }
}
