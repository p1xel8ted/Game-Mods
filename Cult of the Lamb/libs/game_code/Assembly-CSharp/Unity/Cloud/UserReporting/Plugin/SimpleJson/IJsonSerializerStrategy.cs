// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.Plugin.SimpleJson.IJsonSerializerStrategy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.CodeDom.Compiler;

#nullable disable
namespace Unity.Cloud.UserReporting.Plugin.SimpleJson;

[GeneratedCode("simple-json", "1.0.0")]
public interface IJsonSerializerStrategy
{
  bool TrySerializeNonPrimitiveObject(object input, out object output);

  object DeserializeObject(object value, Type type);
}
