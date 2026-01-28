// Decompiled with JetBrains decompiler
// Type: PlayAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PlayAnimation : BaseMonoBehaviour
{
  public Animator animator;

  public void Start()
  {
  }

  public void Play() => this.animator.Play("KeyArt");

  public void Update()
  {
    float fixedDeltaTime = Time.fixedDeltaTime;
    Shader.SetGlobalFloat("_GlobalTimeUnscaled", fixedDeltaTime);
    Shader.SetGlobalFloat("_GlobalTimeUnscaled1", fixedDeltaTime);
  }
}
