// Decompiled with JetBrains decompiler
// Type: PlayerStartPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections;
using UnityEngine;

#nullable disable
public class PlayerStartPosition : BaseMonoBehaviour
{
  public bool HideHud;
  public bool AnimateCameraIn;
  private float Progress;
  private float Duration = 4f;

  private void OnEnable()
  {
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.OnBiomeChangeRoom);
  }

  private void Start()
  {
    if (!this.HideHud)
      return;
    HUD_Manager.Instance.Hide(true, 0);
  }

  private void OnDisable()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.OnBiomeChangeRoom);
  }

  private void OnBiomeChangeRoom()
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

  private IEnumerator AnimateCameraInRoutine()
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
