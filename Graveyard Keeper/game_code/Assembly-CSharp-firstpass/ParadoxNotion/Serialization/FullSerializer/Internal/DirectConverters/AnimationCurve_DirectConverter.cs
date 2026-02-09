// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters.AnimationCurve_DirectConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters;

public class AnimationCurve_DirectConverter : fsDirectConverter<AnimationCurve>
{
  public override fsResult DoSerialize(AnimationCurve model, Dictionary<string, fsData> serialized)
  {
    return fsResult.Success + this.SerializeMember<Keyframe[]>(serialized, (System.Type) null, "keys", model.keys) + this.SerializeMember<WrapMode>(serialized, (System.Type) null, "preWrapMode", model.preWrapMode) + this.SerializeMember<WrapMode>(serialized, (System.Type) null, "postWrapMode", model.postWrapMode);
  }

  public override fsResult DoDeserialize(Dictionary<string, fsData> data, ref AnimationCurve model)
  {
    fsResult success = fsResult.Success;
    Keyframe[] keys = model.keys;
    fsResult fsResult1 = this.DeserializeMember<Keyframe[]>(data, (System.Type) null, "keys", out keys);
    fsResult fsResult2 = success + fsResult1;
    model.keys = keys;
    WrapMode preWrapMode = model.preWrapMode;
    fsResult fsResult3 = this.DeserializeMember<WrapMode>(data, (System.Type) null, "preWrapMode", out preWrapMode);
    fsResult fsResult4 = fsResult2 + fsResult3;
    model.preWrapMode = preWrapMode;
    WrapMode postWrapMode = model.postWrapMode;
    fsResult fsResult5 = this.DeserializeMember<WrapMode>(data, (System.Type) null, "postWrapMode", out postWrapMode);
    fsResult fsResult6 = fsResult4 + fsResult5;
    model.postWrapMode = postWrapMode;
    return fsResult6;
  }

  public override object CreateInstance(fsData data, System.Type storageType)
  {
    return (object) new AnimationCurve();
  }
}
