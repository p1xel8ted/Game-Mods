// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIDemonMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.FollowerSelect;

#nullable disable
namespace Lamb.UI;

public class UIDemonMenuController : UIFollowerSelectBase<DemonFollowerItem>
{
  public override bool AllowsVoting => false;

  public override DemonFollowerItem PrefabTemplate()
  {
    return MonoSingleton<UIManager>.Instance.DemonFollowerItemTemplate;
  }
}
