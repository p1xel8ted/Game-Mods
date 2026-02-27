// Decompiled with JetBrains decompiler
// Type: Response
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
