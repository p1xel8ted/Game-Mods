// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters.LayerMask_DirectConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters;

public class LayerMask_DirectConverter : fsDirectConverter<LayerMask>
{
  public override fsResult DoSerialize(LayerMask model, Dictionary<string, fsData> serialized)
  {
    return fsResult.Success + this.SerializeMember<int>(serialized, (System.Type) null, "value", model.value);
  }

  public override fsResult DoDeserialize(Dictionary<string, fsData> data, ref LayerMask model)
  {
    fsResult success = fsResult.Success;
    int num = model.value;
    fsResult fsResult1 = this.DeserializeMember<int>(data, (System.Type) null, "value", out num);
    fsResult fsResult2 = success + fsResult1;
    model.value = num;
    return fsResult2;
  }

  public override object CreateInstance(fsData data, System.Type storageType)
  {
    return (object) new LayerMask();
  }
}
