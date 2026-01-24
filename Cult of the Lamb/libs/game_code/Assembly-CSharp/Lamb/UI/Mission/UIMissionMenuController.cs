// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Mission.UIMissionMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.FollowerSelect;

#nullable disable
namespace Lamb.UI.Mission;

public class UIMissionMenuController : UIFollowerSelectBase<MissionaryFollowerItem>
{
  public override bool AllowsVoting => false;

  public override MissionaryFollowerItem PrefabTemplate()
  {
    return MonoSingleton<UIManager>.Instance.MissionaryFollowerItemTemplate;
  }
}
