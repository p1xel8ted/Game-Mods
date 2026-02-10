// Decompiled with JetBrains decompiler
// Type: PlayerStartPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections;
using UnityEngine;

#nullable disable
public class PlayerStartPosition : BaseMonoBehaviour
{
  public bool HideHud;
  public bool AnimateCameraIn;
  public float Progress;
  public float Duration = 4f;

  public void OnEnable()
  {
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.OnBiomeChangeRoom);
  }

  public void Start()
  {
    if (!this.HideHud)
      return;
    HUD_Manager.Instance.Hide(true, 0);
  }

  public void OnDisable()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.OnBiomeChangeRoom);
  }

  public void OnBiomeChangeRoom()
  {
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((Object) withTag != (Object) null))
      return;
    if (this.AnimateCameraIn)
      this.StartCoroutine((IEnumerator) this.AnimateCameraInRoutine());
    withTag.transform.position = this.transform.transform.position;
    withTag.GetComponent<StateMachine>().facingAngle = -85f;
    withTag.GetComponentInChildren<SimpleSpineAnimator>().Animate("intro/idle", 0, true);
  }

  public IEnumerator AnimateCameraInRoutine()
  {
    this.Progress = 0.0f;
    while ((double) this.Progress < (double) this.Duration)
    {
      this.Progress += Time.deltaTime;
      CameraFollowTarget.Instance.targetDistance = Mathf.SmoothStep(4f, 10f, this.Progress / this.Duration);
      yield return (object) null;
    }
  }
}
