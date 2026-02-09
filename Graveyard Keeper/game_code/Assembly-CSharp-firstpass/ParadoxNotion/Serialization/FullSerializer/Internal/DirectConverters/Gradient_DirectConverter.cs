// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters.Gradient_DirectConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters;

public class Gradient_DirectConverter : fsDirectConverter<Gradient>
{
  public override fsResult DoSerialize(Gradient model, Dictionary<string, fsData> serialized)
  {
    return fsResult.Success + this.SerializeMember<GradientAlphaKey[]>(serialized, (System.Type) null, "alphaKeys", model.alphaKeys) + this.SerializeMember<GradientColorKey[]>(serialized, (System.Type) null, "colorKeys", model.colorKeys);
  }

  public override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Gradient model)
  {
    fsResult success = fsResult.Success;
    GradientAlphaKey[] alphaKeys = model.alphaKeys;
    fsResult fsResult1 = this.DeserializeMember<GradientAlphaKey[]>(data, (System.Type) null, "alphaKeys", out alphaKeys);
    fsResult fsResult2 = success + fsResult1;
    model.alphaKeys = alphaKeys;
    GradientColorKey[] colorKeys = model.colorKeys;
    fsResult fsResult3 = this.DeserializeMember<GradientColorKey[]>(data, (System.Type) null, "colorKeys", out colorKeys);
    fsResult fsResult4 = fsResult2 + fsResult3;
    model.colorKeys = colorKeys;
    return fsResult4;
  }

  public override object CreateInstance(fsData data, System.Type storageType)
  {
    return (object) new Gradient();
  }
}
