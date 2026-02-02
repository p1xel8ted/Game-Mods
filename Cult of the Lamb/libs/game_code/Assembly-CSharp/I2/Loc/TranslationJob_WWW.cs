// Decompiled with JetBrains decompiler
// Type: I2.Loc.TranslationJob_WWW
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
