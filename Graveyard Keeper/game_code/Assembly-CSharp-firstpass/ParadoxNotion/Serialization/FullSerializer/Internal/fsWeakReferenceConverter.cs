// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsWeakReferenceConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public class fsWeakReferenceConverter : fsConverter
{
  public override bool CanProcess(Type type) => Type.op_Equality(type, typeof (WeakReference));

  public override bool RequestCycleSupport(Type storageType) => false;

  public override bool RequestInheritanceSupport(Type storageType) => false;

  public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
  {
    WeakReference weakReference = (WeakReference) instance;
    fsResult success = fsResult.Success;
    serialized = fsData.CreateDictionary();
    fsData data;
    if (weakReference.IsAlive && !(success += this.Serializer.TrySerialize<object>(weakReference.Target, out data)).Failed)
    {
      serialized.AsDictionary["Target"] = data;
      serialized.AsDictionary["TrackResurrection"] = new fsData(weakReference.TrackResurrection);
    }
    return success;
  }

  public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
  {
    fsResult fsResult;
    if ((fsResult = fsResult.Success + this.CheckType(data, fsDataType.Object)).Failed || !data.AsDictionary.ContainsKey("Target"))
      return fsResult;
    fsData data1 = data.AsDictionary["Target"];
    object result = (object) null;
    if ((fsResult += this.Serializer.TryDeserialize(data1, typeof (object), ref result)).Failed)
      return fsResult;
    bool trackResurrection = false;
    if (data.AsDictionary.ContainsKey("TrackResurrection") && data.AsDictionary["TrackResurrection"].IsBool)
      trackResurrection = data.AsDictionary["TrackResurrection"].AsBool;
    instance = (object) new WeakReference(result, trackResurrection);
    return fsResult;
  }

  public override object CreateInstance(fsData data, Type storageType)
  {
    return (object) new WeakReference((object) null);
  }
}
