// Decompiled with JetBrains decompiler
// Type: PickUpSoulFragment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
