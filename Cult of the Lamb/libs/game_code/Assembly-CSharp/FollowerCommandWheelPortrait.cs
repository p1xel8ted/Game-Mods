// Decompiled with JetBrains decompiler
// Type: FollowerCommandWheelPortrait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerCommandWheelPortrait : BaseMonoBehaviour
{
  public SkeletonGraphic Spine;
  public float ScaleSpeed;

  public void Play(FollowerBrainInfo f)
  {
    this.Spine.Skeleton.SetSkin(f.SkinName);
    WorshipperData.SkinAndData colourData = WorshipperData.Instance.GetColourData(f.SkinName);
    if (colourData != null)
    {
      foreach (WorshipperData.SlotAndColor slotAndColour in colourData.SlotAndColours[Mathf.Clamp(f.SkinColour, 0, colourData.SlotAndColours.Count - 1)].SlotAndColours)
      {
        Slot slot = this.Spine.Skeleton.FindSlot(slotAndColour.Slot);
        if (slot != null)
          slot.SetColor(slotAndColour.color);
      }
    }
    this.transform.localScale = Vector3.one * 2f;
  }

  public void Update()
  {
    this.transform.localScale = Vector3.one * Utils.BounceLerp(1f, this.transform.localScale.x, ref this.ScaleSpeed);
  }

  public IEnumerator ScaleIn()
  {
    FollowerCommandWheelPortrait commandWheelPortrait = this;
    Debug.Log((object) "AA");
    float Progress = 0.0f;
    float Duration = 0.3f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      Debug.Log((object) (float) ((double) Progress / (double) Duration));
      commandWheelPortrait.transform.localScale = Vector3.Lerp(Vector3.one * 1.5f, Vector3.one, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    commandWheelPortrait.transform.localScale = Vector3.one;
  }
}
