// Decompiled with JetBrains decompiler
// Type: I2.Loc.TranslationJob_WWW
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine.Networking;

#nullable disable
namespace I2.Loc;

public class TranslationJob_WWW : TranslationJob
{
  public UnityWebRequest www;

  public override void Dispose()
  {
    if (this.www != null)
      this.www.Dispose();
    this.www = (UnityWebRequest) null;
  }
}
