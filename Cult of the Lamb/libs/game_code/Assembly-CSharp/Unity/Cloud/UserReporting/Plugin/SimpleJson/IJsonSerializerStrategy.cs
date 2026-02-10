// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.Plugin.SimpleJson.IJsonSerializerStrategy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
