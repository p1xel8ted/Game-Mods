// Decompiled with JetBrains decompiler
// Type: I2.Loc.TranslationJob_WWW
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
