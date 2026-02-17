// Decompiled with JetBrains decompiler
// Type: UIMatingProgressMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
