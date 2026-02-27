// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.Plugin.SimpleJson.JsonArray
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
