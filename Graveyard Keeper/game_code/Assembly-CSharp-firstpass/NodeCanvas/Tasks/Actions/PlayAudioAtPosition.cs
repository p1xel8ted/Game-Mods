// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.PlayAudioAtPosition
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Audio")]
public class PlayAudioAtPosition : ActionTask<Transform>
{
  [RequiredField]
  public BBParameter<AudioClip> audioClip;
  [SliderField(0, 1)]
  public float volume = 1f;
  public bool waitActionFinish;

  public override string info => "PlayAudio " + this.audioClip.ToString();

  public override void OnExecute()
  {
    AudioSource.PlayClipAtPoint(this.audioClip.value, this.agent.position, this.volume);
    if (this.waitActionFinish)
      return;
    this.EndAction();
  }

  public override void OnUpdate()
  {
    if ((double) this.elapsedTime < (double) this.audioClip.value.length)
      return;
    this.EndAction();
  }
}
