// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters.RectOffset_DirectConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters;

public class RectOffset_DirectConverter : fsDirectConverter<RectOffset>
{
  public override fsResult DoSerialize(RectOffset model, Dictionary<string, fsData> serialized)
  {
    return fsResult.Success + this.SerializeMember<int>(serialized, (System.Type) null, "bottom", model.bottom) + this.SerializeMember<int>(serialized, (System.Type) null, "left", model.left) + this.SerializeMember<int>(serialized, (System.Type) null, "right", model.right) + this.SerializeMember<int>(serialized, (System.Type) null, "top", model.top);
  }

  public override fsResult DoDeserialize(Dictionary<string, fsData> data, ref RectOffset model)
  {
    fsResult success = fsResult.Success;
    int bottom = model.bottom;
    fsResult fsResult1 = this.DeserializeMember<int>(data, (System.Type) null, "bottom", out bottom);
    fsResult fsResult2 = success + fsResult1;
    model.bottom = bottom;
    int left = model.left;
    fsResult fsResult3 = this.DeserializeMember<int>(data, (System.Type) null, "left", out left);
    fsResult fsResult4 = fsResult2 + fsResult3;
    model.left = left;
    int right = model.right;
    fsResult fsResult5 = this.DeserializeMember<int>(data, (System.Type) null, "right", out right);
    fsResult fsResult6 = fsResult4 + fsResult5;
    model.right = right;
    int top = model.top;
    fsResult fsResult7 = this.DeserializeMember<int>(data, (System.Type) null, "top", out top);
    fsResult fsResult8 = fsResult6 + fsResult7;
    model.top = top;
    return fsResult8;
  }

  public override object CreateInstance(fsData data, System.Type storageType)
  {
    return (object) new RectOffset();
  }
}
