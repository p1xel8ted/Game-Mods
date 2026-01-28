// Decompiled with JetBrains decompiler
// Type: PickUpSoulFragment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
