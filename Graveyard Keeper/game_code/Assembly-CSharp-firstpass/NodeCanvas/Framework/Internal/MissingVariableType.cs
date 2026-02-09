// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Internal.MissingVariableType
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Serialization;
using ParadoxNotion.Serialization.FullSerializer;
using System.Runtime.CompilerServices;

#nullable disable
namespace NodeCanvas.Framework.Internal;

public class MissingVariableType : Variable<object>, IMissingRecoverable
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
}
