// Decompiled with JetBrains decompiler
// Type: I2.Loc.TranslationJob_WWW
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
