// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters.Rect_DirectConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters;

public class Rect_DirectConverter : fsDirectConverter<Rect>
{
  public override fsResult DoSerialize(Rect model, Dictionary<string, fsData> serialized)
  {
    return fsResult.Success + this.SerializeMember<float>(serialized, (System.Type) null, "xMin", model.xMin) + this.SerializeMember<float>(serialized, (System.Type) null, "yMin", model.yMin) + this.SerializeMember<float>(serialized, (System.Type) null, "xMax", model.xMax) + this.SerializeMember<float>(serialized, (System.Type) null, "yMax", model.yMax);
  }

  public override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Rect model)
  {
    fsResult success = fsResult.Success;
    float xMin = model.xMin;
    fsResult fsResult1 = this.DeserializeMember<float>(data, (System.Type) null, "xMin", out xMin);
    fsResult fsResult2 = success + fsResult1;
    model.xMin = xMin;
    float yMin = model.yMin;
    fsResult fsResult3 = this.DeserializeMember<float>(data, (System.Type) null, "yMin", out yMin);
    fsResult fsResult4 = fsResult2 + fsResult3;
    model.yMin = yMin;
    float xMax = model.xMax;
    fsResult fsResult5 = this.DeserializeMember<float>(data, (System.Type) null, "xMax", out xMax);
    fsResult fsResult6 = fsResult4 + fsResult5;
    model.xMax = xMax;
    float yMax = model.yMax;
    fsResult fsResult7 = this.DeserializeMember<float>(data, (System.Type) null, "yMax", out yMax);
    fsResult fsResult8 = fsResult6 + fsResult7;
    model.yMax = yMax;
    return fsResult8;
  }

  public override object CreateInstance(fsData data, System.Type storageType)
  {
    return (object) new Rect();
  }
}
