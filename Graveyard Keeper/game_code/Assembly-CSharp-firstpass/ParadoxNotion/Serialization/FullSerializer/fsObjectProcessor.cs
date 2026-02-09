// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.fsObjectProcessor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer;

public abstract class fsObjectProcessor
{
  public virtual bool CanProcess(Type type) => throw new NotImplementedException();

  public virtual void OnBeforeSerialize(Type storageType, object instance)
  {
  }

  public virtual void OnAfterSerialize(Type storageType, object instance, ref fsData data)
  {
  }

  public virtual void OnBeforeDeserialize(Type storageType, ref fsData data)
  {
  }

  public virtual void OnBeforeDeserializeAfterInstanceCreation(
    Type storageType,
    object instance,
    ref fsData data)
  {
  }

  public virtual void OnAfterDeserialize(Type storageType, object instance)
  {
  }
}
