// Decompiled with JetBrains decompiler
// Type: Interaction_WoolyShackTrunk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Interaction_WoolyShackTrunk : Interaction
{
  public Interaction_WoolyShack shack;

  public void Start() => this.shack = this.GetComponentInParent<Interaction_WoolyShack>();

  public override void GetLabel()
  {
    base.GetLabel();
    int num = 0;
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      FollowerBrain brain = FollowerBrain.GetOrCreateBrain(follower);
      if (brain != null && brain.CanFreezeExcludingScarf())
        ++num;
    }
    this.Label = LocalizeIntegration.FormatCurrentMax(this.shack.Brain.Data.Inventory.Count.ToString(), num.ToString()) ?? "";
  }
}
