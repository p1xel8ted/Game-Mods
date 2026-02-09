// Decompiled with JetBrains decompiler
// Type: Response
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
