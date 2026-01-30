// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FollowerInteractionWheel.CommandItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;

#nullable disable
namespace Lamb.UI.FollowerInteractionWheel;

public class CommandItem
{
  public FollowerCommands Command;
  public FollowerCommands SubCommand;
  public List<CommandItem> SubCommands;

  public virtual string GetTitle(Follower follower)
  {
    return LocalizationManager.GetTranslation($"FollowerInteractions/{this.Command}");
  }

  public virtual string GetDescription(Follower follower)
  {
    return LocalizationManager.GetTranslation($"FollowerInteractions/{this.Command}/Description");
  }

  public virtual string GetLockedDescription(Follower follower)
  {
    return LocalizationManager.GetTranslation($"FollowerInteractions/{this.Command}/NotAvailable");
  }

  public virtual bool IsAvailable(Follower follower) => true;

  public virtual bool IsAvailable(InventoryItem.ITEM_TYPE resourceType)
  {
    return resourceType > InventoryItem.ITEM_TYPE.NONE;
  }

  public virtual string GetIcon() => FontImageNames.IconForCommand(this.Command);

  public virtual string GetSubIcon()
  {
    return this.SubCommand == FollowerCommands.None ? "" : FontImageNames.IconForCommand(this.SubCommand);
  }
}
