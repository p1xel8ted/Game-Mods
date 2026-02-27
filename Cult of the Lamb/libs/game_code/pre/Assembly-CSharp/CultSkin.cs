// Decompiled with JetBrains decompiler
// Type: CultSkin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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

  private void Start()
  {
    this.SetRandomHead();
    if (this.removeMask)
      this.SetRemoveMask();
    if (!this.removeSword)
      return;
    this.SetRemoveSword();
  }

  private void SetRemoveSword()
  {
    this.Spine.skeleton.FindSlot("MASK_SKIN").Attachment = (Attachment) null;
  }

  private void SetRemoveMask()
  {
    this.Spine.skeleton.FindSlot("WEAPON_SKIN").Attachment = (Attachment) null;
  }

  private void SetRandomHead()
  {
    if (this.NumberOfHeads <= 0)
      return;
    this.Spine.skeleton.SetAttachment("HEAD_SKIN", "HEAD_" + (object) Random.Range(0, this.NumberOfHeads));
  }

  private void SetSkinByCult()
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
