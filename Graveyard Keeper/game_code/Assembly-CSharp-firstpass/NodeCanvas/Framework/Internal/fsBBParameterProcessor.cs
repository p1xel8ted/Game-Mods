// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Internal.fsBBParameterProcessor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using ParadoxNotion.Serialization;
using ParadoxNotion.Serialization.FullSerializer;
using System;
using System.Collections.Generic;

#nullable disable
namespace NodeCanvas.Framework.Internal;

public class fsBBParameterProcessor : fsRecoveryProcessor<BBParameter, MissingBBParameterType>
{
  public override void OnBeforeDeserializeAfterInstanceCreation(
    Type storageType,
    object instance,
    ref fsData data)
  {
    if (data.IsDictionary)
    {
      Dictionary<string, fsData> asDictionary = data.AsDictionary;
      if (asDictionary.Count == 0 || asDictionary.ContainsKey("_value") || asDictionary.ContainsKey("_name"))
        return;
    }
    if (!(instance is BBParameter bbParameter))
      return;
    Type varType = bbParameter.varType;
    fsSerializer fsSerializer = new fsSerializer();
    object result = (object) null;
    if (!fsSerializer.TryDeserialize(data, varType, ref result).Succeeded || result == null || !varType.RTIsAssignableFrom(result.GetType()))
      return;
    bbParameter.value = result;
    fsSerializer.TrySerialize(storageType, instance, out data);
  }
}
