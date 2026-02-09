// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.fsDirectConverter`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer;

public abstract class fsDirectConverter<TModel> : fsDirectConverter
{
  public override Type ModelType => typeof (TModel);

  public sealed override fsResult TrySerialize(
    object instance,
    out fsData serialized,
    Type storageType)
  {
    Dictionary<string, fsData> dictionary = new Dictionary<string, fsData>();
    fsResult fsResult = this.DoSerialize((TModel) instance, dictionary);
    serialized = new fsData(dictionary);
    return fsResult;
  }

  public sealed override fsResult TryDeserialize(
    fsData data,
    ref object instance,
    Type storageType)
  {
    fsResult fsResult1;
    if ((fsResult1 = fsResult.Success + this.CheckType(data, fsDataType.Object)).Failed)
      return fsResult1;
    TModel model = (TModel) instance;
    fsResult fsResult2 = fsResult1 + this.DoDeserialize(data.AsDictionary, ref model);
    instance = (object) model;
    return fsResult2;
  }

  public abstract fsResult DoSerialize(TModel model, Dictionary<string, fsData> serialized);

  public abstract fsResult DoDeserialize(Dictionary<string, fsData> data, ref TModel model);
}
