// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters.Bounds_DirectConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters;

public class Bounds_DirectConverter : fsDirectConverter<Bounds>
{
  public override fsResult DoSerialize(Bounds model, Dictionary<string, fsData> serialized)
  {
    return fsResult.Success + this.SerializeMember<Vector3>(serialized, (System.Type) null, "center", model.center) + this.SerializeMember<Vector3>(serialized, (System.Type) null, "size", model.size);
  }

  public override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Bounds model)
  {
    fsResult success = fsResult.Success;
    Vector3 center = model.center;
    fsResult fsResult1 = this.DeserializeMember<Vector3>(data, (System.Type) null, "center", out center);
    fsResult fsResult2 = success + fsResult1;
    model.center = center;
    Vector3 size = model.size;
    fsResult fsResult3 = this.DeserializeMember<Vector3>(data, (System.Type) null, "size", out size);
    fsResult fsResult4 = fsResult2 + fsResult3;
    model.size = size;
    return fsResult4;
  }

  public override object CreateInstance(fsData data, System.Type storageType)
  {
    return (object) new Bounds();
  }
}
