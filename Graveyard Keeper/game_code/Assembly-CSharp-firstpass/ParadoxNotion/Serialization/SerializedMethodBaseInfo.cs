// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.SerializedMethodBaseInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Reflection;
using UnityEngine;

#nullable disable
namespace ParadoxNotion.Serialization;

public abstract class SerializedMethodBaseInfo : ISerializationCallbackReceiver
{
  public abstract MethodBase GetBase();

  public abstract bool HasChanged();

  public abstract string GetMethodString();

  public override string ToString() => this.GetMethodString();

  public abstract void OnBeforeSerialize();

  public abstract void OnAfterDeserialize();
}
