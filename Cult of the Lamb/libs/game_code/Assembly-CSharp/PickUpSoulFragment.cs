// Decompiled with JetBrains decompiler
// Type: PickUpSoulFragment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class PickUpSoulFragment : PickUp
{
  public static PickUpSoulFragment.CollectSoulFragment OnCollectSoulFragment;

  public override void PickMeUp()
  {
    base.PickMeUp();
    PickUpSoulFragment.CollectSoulFragment collectSoulFragment = PickUpSoulFragment.OnCollectSoulFragment;
    if (collectSoulFragment == null)
      return;
    collectSoulFragment();
  }

  public delegate void CollectSoulFragment();
}
