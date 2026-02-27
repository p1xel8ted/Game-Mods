// Decompiled with JetBrains decompiler
// Type: Response
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
