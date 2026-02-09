// Decompiled with JetBrains decompiler
// Type: NodeCanvas.DialogueTrees.IDialogueActor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace NodeCanvas.DialogueTrees;

public interface IDialogueActor
{
  string name { get; }

  Texture2D portrait { get; }

  Sprite portraitSprite { get; }

  Color dialogueColor { get; }

  Vector3 dialoguePosition { get; }

  Transform transform { get; }
}
