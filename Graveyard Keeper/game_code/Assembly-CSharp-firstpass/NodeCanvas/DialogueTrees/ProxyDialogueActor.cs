// Decompiled with JetBrains decompiler
// Type: NodeCanvas.DialogueTrees.ProxyDialogueActor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace NodeCanvas.DialogueTrees;

[Serializable]
public class ProxyDialogueActor : IDialogueActor
{
  public string _name;
  public Transform _transform;

  public string name => this._name;

  public Texture2D portrait => (Texture2D) null;

  public Sprite portraitSprite => (Sprite) null;

  public Color dialogueColor => Color.white;

  public Vector3 dialoguePosition => Vector3.zero;

  public Transform transform => this._transform;

  public ProxyDialogueActor(string name, Transform transform)
  {
    this._name = name;
    this._transform = transform;
  }
}
