// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters.GUIStyleState_DirectConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters;

public class GUIStyleState_DirectConverter : fsDirectConverter<GUIStyleState>
{
  public override fsResult DoSerialize(GUIStyleState model, Dictionary<string, fsData> serialized)
  {
    return fsResult.Success + this.SerializeMember<Texture2D>(serialized, (System.Type) null, "background", model.background) + this.SerializeMember<Color>(serialized, (System.Type) null, "textColor", model.textColor);
  }

  public override fsResult DoDeserialize(Dictionary<string, fsData> data, ref GUIStyleState model)
  {
    fsResult success = fsResult.Success;
    Texture2D background = model.background;
    fsResult fsResult1 = this.DeserializeMember<Texture2D>(data, (System.Type) null, "background", out background);
    fsResult fsResult2 = success + fsResult1;
    model.background = background;
    Color textColor = model.textColor;
    fsResult fsResult3 = this.DeserializeMember<Color>(data, (System.Type) null, "textColor", out textColor);
    fsResult fsResult4 = fsResult2 + fsResult3;
    model.textColor = textColor;
    return fsResult4;
  }

  public override object CreateInstance(fsData data, System.Type storageType)
  {
    return (object) new GUIStyleState();
  }
}
