// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Mission.UIMissionMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
