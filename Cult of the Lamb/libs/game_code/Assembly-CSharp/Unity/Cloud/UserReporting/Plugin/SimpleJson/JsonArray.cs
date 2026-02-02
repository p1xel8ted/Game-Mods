// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.Plugin.SimpleJson.JsonArray
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace Unity.Cloud.UserReporting.Plugin.SimpleJson;

[GeneratedCode("simple-json", "1.0.0")]
[EditorBrowsable(EditorBrowsableState.Never)]
public class JsonArray : List<object>
{
  public JsonArray()
  {
  }

  public JsonArray(int capacity)
    : base(capacity)
  {
  }

  public override string ToString() => Unity.Cloud.UserReporting.Plugin.SimpleJson.SimpleJson.SerializeObject((object) this) ?? string.Empty;
}
