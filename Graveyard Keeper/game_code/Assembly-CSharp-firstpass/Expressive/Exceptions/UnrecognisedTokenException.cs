// Decompiled with JetBrains decompiler
// Type: Expressive.Exceptions.UnrecognisedTokenException
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

#nullable disable
namespace Expressive.Exceptions;

[Serializable]
public sealed class UnrecognisedTokenException : Exception
{
  [CompilerGenerated]
  public string \u003CToken\u003Ek__BackingField;

  public string Token
  {
    get => this.\u003CToken\u003Ek__BackingField;
    set => this.\u003CToken\u003Ek__BackingField = value;
  }

  public UnrecognisedTokenException(string token)
    : base($"Unrecognised token '{token}'")
  {
    this.Token = token;
  }

  [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
  public override void GetObjectData(SerializationInfo info, StreamingContext context)
  {
    base.GetObjectData(info, context);
    info.AddValue("Token", (object) this.Token);
  }
}
