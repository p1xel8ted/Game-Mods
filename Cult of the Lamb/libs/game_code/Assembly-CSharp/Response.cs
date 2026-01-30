// Decompiled with JetBrains decompiler
// Type: Response
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Response
{
  public System.Action Callback;
  public string text;

  public Response(string text, System.Action Callback)
  {
    this.text = text;
    this.Callback = Callback;
  }
}
