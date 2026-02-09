// Decompiled with JetBrains decompiler
// Type: MMTools.Response
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
