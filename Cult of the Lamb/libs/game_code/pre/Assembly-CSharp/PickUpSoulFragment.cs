// Decompiled with JetBrains decompiler
// Type: PickUpSoulFragment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
