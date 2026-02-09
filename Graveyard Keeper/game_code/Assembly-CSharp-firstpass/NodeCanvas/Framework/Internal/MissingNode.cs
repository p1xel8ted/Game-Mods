// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Internal.MissingNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using ParadoxNotion.Serialization.FullSerializer;
using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace NodeCanvas.Framework.Internal;

[DoNotList]
[Description("Please resolve the MissingNode issue by either replacing the node or importing the missing node type in the project.")]
public sealed class MissingNode : Node, IMissingRecoverable
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

  public override string name => "<color=#ff6457>* Missing Node *</color>";

  public override Type outConnectionType => (Type) null;

  public override int maxInConnections => 0;

  public override int maxOutConnections => 0;

  public override bool allowAsPrime => false;

  public override Alignment2x2 commentsAlignment => Alignment2x2.Right;

  public override Alignment2x2 iconAlignment => Alignment2x2.Default;
}
