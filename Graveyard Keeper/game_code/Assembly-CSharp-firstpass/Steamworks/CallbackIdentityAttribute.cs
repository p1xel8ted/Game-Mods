// Decompiled with JetBrains decompiler
// Type: Steamworks.CallbackIdentityAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace Steamworks;

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
public class CallbackIdentityAttribute : Attribute
{
  [CompilerGenerated]
  public int \u003CIdentity\u003Ek__BackingField;

  public int Identity
  {
    get => this.\u003CIdentity\u003Ek__BackingField;
    set => this.\u003CIdentity\u003Ek__BackingField = value;
  }

  public CallbackIdentityAttribute(int callbackNum) => this.Identity = callbackNum;
}
