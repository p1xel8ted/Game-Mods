// Decompiled with JetBrains decompiler
// Type: NodeCanvas.DialogueTrees.DialogueActor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace NodeCanvas.DialogueTrees;

[AddComponentMenu("NodeCanvas/Dialogue Actor")]
public class DialogueActor : MonoBehaviour, IDialogueActor
{
  [SerializeField]
  public string _name;
  [SerializeField]
  public Texture2D _portrait;
  [SerializeField]
  public Color _dialogueColor = Color.white;
  [SerializeField]
  public Vector3 _dialogueOffset;
  public Sprite _portraitSprite;

  public new string name => this._name;

  public Texture2D portrait => this._portrait;

  public Sprite portraitSprite
  {
    get
    {
      if ((Object) this._portraitSprite == (Object) null && (Object) this.portrait != (Object) null)
        this._portraitSprite = Sprite.Create(this.portrait, new Rect(0.0f, 0.0f, (float) this.portrait.width, (float) this.portrait.height), new Vector2(0.5f, 0.5f));
      return this._portraitSprite;
    }
  }

  public Color dialogueColor => this._dialogueColor;

  public Vector3 dialoguePosition => this.transform.TransformPoint(this._dialogueOffset);

  [SpecialName]
  Transform IDialogueActor.get_transform() => this.transform;
}
