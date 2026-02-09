// Decompiled with JetBrains decompiler
// Type: Steamworks.CallbackIdentities
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

public class CallbackIdentities
{
  public static int GetCallbackIdentity(Type callbackStruct)
  {
    object[] customAttributes = callbackStruct.GetCustomAttributes(typeof (CallbackIdentityAttribute), false);
    int index = 0;
    if (index < customAttributes.Length)
      return ((CallbackIdentityAttribute) customAttributes[index]).Identity;
    throw new Exception("Callback number not found for struct " + callbackStruct?.ToString());
  }
}
