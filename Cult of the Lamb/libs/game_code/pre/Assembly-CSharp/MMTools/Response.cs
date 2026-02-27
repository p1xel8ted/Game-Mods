// Decompiled with JetBrains decompiler
// Type: MMTools.Response
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace MMTools;

[Serializable]
public class Response
{
  public Action ActionCallBack;
  [HideInInspector]
  public string text;
  [TermsPopup("")]
  public string Term;
  public UnityEvent EventCallBack;

  public Response(string text, Action Callback, string Term)
  {
    this.text = text;
    this.ActionCallBack = Callback;
    this.Term = Term;
  }
}
