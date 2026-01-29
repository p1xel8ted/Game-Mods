// Decompiled with JetBrains decompiler
// Type: CultSkin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class CultSkin : BaseMonoBehaviour
{
  public SkeletonAnimation Spine;
  public CultSkin.EnemyType Type;
  public int NumberOfHeads = 18;
  public bool removeMask;
  public bool removeSword;
  public bool overrideRoomManager;

  public void Start()
  {
    this.SetRandomHead();
    if (this.removeMask)
      this.SetRemoveMask();
    if (!this.removeSword)
      return;
    this.SetRemoveSword();
  }

  public void SetRemoveSword()
  {
    this.Spine.skeleton.FindSlot("MASK_SKIN").Attachment = (Attachment) null;
  }

  public void SetRemoveMask()
  {
    this.Spine.skeleton.FindSlot("WEAPON_SKIN").Attachment = (Attachment) null;
  }

  public void SetRandomHead()
  {
    if (this.NumberOfHeads <= 0)
      return;
    this.Spine.skeleton.SetAttachment("HEAD_SKIN", "HEAD_" + Random.Range(0, this.NumberOfHeads).ToString());
  }

  public void SetSkinByCult()
  {
  }

  public enum EnemyType
  {
    Grunt,
    Grunt_Captain,
    Archer,
    Archer_Captain,
    Scamp,
    Shielder,
    Brute,
  }
}
