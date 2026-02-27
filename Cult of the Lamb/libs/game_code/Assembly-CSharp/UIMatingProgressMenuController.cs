// Decompiled with JetBrains decompiler
// Type: UIMatingProgressMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Lamb.UI.FollowerSelect;

#nullable disable
public class UIMatingProgressMenuController : UIFollowerSelectBase<MissionaryFollowerItem>
{
  public override bool AllowsVoting => false;

  public override MissionaryFollowerItem PrefabTemplate()
  {
    return MonoSingleton<UIManager>.Instance.MatingFollowerItemTemplate;
  }
}
