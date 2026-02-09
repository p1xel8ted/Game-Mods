// Decompiled with JetBrains decompiler
// Type: NodeCanvas.DialogueTrees.Statement
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.DialogueTrees;

[Serializable]
public class Statement : IStatement
{
  [SerializeField]
  public string _text = string.Empty;
  [SerializeField]
  public AudioClip _audio;
  [SerializeField]
  public string _meta = string.Empty;

  public string text
  {
    get => this._text;
    set => this._text = value;
  }

  public AudioClip audio
  {
    get => this._audio;
    set => this._audio = value;
  }

  public string meta
  {
    get => this._meta;
    set => this._meta = value;
  }

  public Statement()
  {
  }

  public Statement(string text) => this.text = text;

  public Statement(string text, AudioClip audio)
  {
    this.text = text;
    this.audio = audio;
  }

  public Statement(string text, AudioClip audio, string meta)
  {
    this.text = text;
    this.audio = audio;
    this.meta = meta;
  }

  public Statement BlackboardReplace(IBlackboard bb)
  {
    string text = this.text;
    int startIndex1;
    for (int startIndex2 = 0; (startIndex1 = text.IndexOf('[', startIndex2)) != -1; startIndex2 = startIndex1 + 1)
    {
      int length = text.Substring(startIndex1 + 1).IndexOf(']');
      string varName = text.Substring(startIndex1 + 1, length);
      string oldValue = text.Substring(startIndex1, length + 2);
      object obj = (object) null;
      if (bb != null)
      {
        Variable variable = bb.GetVariable(varName, typeof (object));
        if (variable != null)
          obj = variable.value;
      }
      if (varName.Contains("/"))
      {
        GlobalBlackboard globalBlackboard = GlobalBlackboard.Find(((IEnumerable<string>) varName.Split('/')).First<string>());
        if ((UnityEngine.Object) globalBlackboard != (UnityEngine.Object) null)
        {
          Variable variable = globalBlackboard.GetVariable(((IEnumerable<string>) varName.Split('/')).Last<string>(), typeof (object));
          if (variable != null)
            obj = variable.value;
        }
      }
      text = text.Replace(oldValue, obj != null ? obj.ToString() : oldValue);
    }
    return new Statement(text, this.audio, this.meta);
  }

  public override string ToString() => this.text;
}
