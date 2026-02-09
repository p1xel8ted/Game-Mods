// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Internal.MissingCondition
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using ParadoxNotion.Serialization.FullSerializer;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace NodeCanvas.Framework.Internal;

[DoNotList]
[Description("Please resolve the MissingTask issue by either replacing the task or importing the missing task type in the project")]
public class MissingCondition : ConditionTask, IMissingRecoverable
{
  [CompilerGenerated]
  public string \u003CmissingType\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CrecoveryState\u003Ek__BackingField;

  [fsProperty]
  public string missingType
  {
    get => this.\u003CmissingType\u003Ek__BackingField;
    set => this.\u003CmissingType\u003Ek__BackingField = value;
  }

  [fsProperty]
  public string recoveryState
  {
    get => this.\u003CrecoveryState\u003Ek__BackingField;
    set => this.\u003CrecoveryState\u003Ek__BackingField = value;
  }

  public override string info
  {
    get
    {
      return $"<color=#ff6457>* {((IEnumerable<string>) this.missingType.Split('.')).Last<string>()} *</color>";
    }
  }
}
