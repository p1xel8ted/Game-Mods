// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters.Keyframe_DirectConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters;

public class Keyframe_DirectConverter : fsDirectConverter<Keyframe>
{
  public override fsResult DoSerialize(Keyframe model, Dictionary<string, fsData> serialized)
  {
    return fsResult.Success + this.SerializeMember<float>(serialized, (System.Type) null, "time", model.time) + this.SerializeMember<float>(serialized, (System.Type) null, "value", model.value) + this.SerializeMember<int>(serialized, (System.Type) null, "tangentMode", model.tangentMode) + this.SerializeMember<float>(serialized, (System.Type) null, "inTangent", model.inTangent) + this.SerializeMember<float>(serialized, (System.Type) null, "outTangent", model.outTangent);
  }

  public override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Keyframe model)
  {
    fsResult success = fsResult.Success;
    float time = model.time;
    fsResult fsResult1 = this.DeserializeMember<float>(data, (System.Type) null, "time", out time);
    fsResult fsResult2 = success + fsResult1;
    model.time = time;
    float num = model.value;
    fsResult fsResult3 = this.DeserializeMember<float>(data, (System.Type) null, "value", out num);
    fsResult fsResult4 = fsResult2 + fsResult3;
    model.value = num;
    int tangentMode = model.tangentMode;
    fsResult fsResult5 = this.DeserializeMember<int>(data, (System.Type) null, "tangentMode", out tangentMode);
    fsResult fsResult6 = fsResult4 + fsResult5;
    model.tangentMode = tangentMode;
    float inTangent = model.inTangent;
    fsResult fsResult7 = this.DeserializeMember<float>(data, (System.Type) null, "inTangent", out inTangent);
    fsResult fsResult8 = fsResult6 + fsResult7;
    model.inTangent = inTangent;
    float outTangent = model.outTangent;
    fsResult fsResult9 = this.DeserializeMember<float>(data, (System.Type) null, "outTangent", out outTangent);
    fsResult fsResult10 = fsResult8 + fsResult9;
    model.outTangent = outTangent;
    return fsResult10;
  }

  public override object CreateInstance(fsData data, System.Type storageType)
  {
    return (object) new Keyframe();
  }
}
