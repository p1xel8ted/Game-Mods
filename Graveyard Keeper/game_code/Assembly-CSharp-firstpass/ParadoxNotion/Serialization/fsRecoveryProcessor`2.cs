// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.fsRecoveryProcessor`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion.Serialization.FullSerializer;
using ParadoxNotion.Serialization.FullSerializer.Internal;
using System;
using System.Collections.Generic;

#nullable disable
namespace ParadoxNotion.Serialization;

public class fsRecoveryProcessor<TCanProcess, TMissing> : fsObjectProcessor where TMissing : TCanProcess, IMissingRecoverable
{
  public override bool CanProcess(Type type) => typeof (TCanProcess).RTIsAssignableFrom(type);

  public override void OnBeforeDeserialize(Type storageType, ref fsData data)
  {
    if (!data.IsDictionary)
      return;
    Dictionary<string, fsData> json = data.AsDictionary;
    fsData fsData;
    if (!json.TryGetValue("$type", out fsData))
      return;
    Type type1 = fsTypeCache.GetType(fsData.AsString, storageType);
    if (Type.op_Equality(type1, (Type) null))
    {
      json["missingType"] = new fsData(fsData.AsString);
      json["recoveryState"] = new fsData(data.ToString());
      json["$type"] = new fsData(typeof (TMissing).FullName);
    }
    if (!Type.op_Equality(type1, typeof (TMissing)))
      return;
    Type type2 = fsTypeCache.GetType(json["missingType"].AsString, storageType);
    if (!Type.op_Inequality(type2, (Type) null))
      return;
    Dictionary<string, fsData> asDictionary = fsJsonParser.Parse(json["recoveryState"].AsString).AsDictionary;
    json = json.Concat<KeyValuePair<string, fsData>>(asDictionary.Where<KeyValuePair<string, fsData>>((Func<KeyValuePair<string, fsData>, bool>) (kvp => !json.ContainsKey(kvp.Key)))).ToDictionary<KeyValuePair<string, fsData>, string, fsData>((Func<KeyValuePair<string, fsData>, string>) (c => c.Key), (Func<KeyValuePair<string, fsData>, fsData>) (c => c.Value));
    json["$type"] = new fsData(type2.FullName);
    data = new fsData(json);
  }
}
