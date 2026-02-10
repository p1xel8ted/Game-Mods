// Decompiled with JetBrains decompiler
// Type: MMTools.Response
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
