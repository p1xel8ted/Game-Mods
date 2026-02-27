// Decompiled with JetBrains decompiler
// Type: Spine.Unity.Examples.SpineManualUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Spine.Unity.Examples;

public class SpineManualUpdater : BaseMonoBehaviour
{
  [Range(0.0f, 0.125f)]
  [Tooltip("To specify a framerate, type 1/framerate in the inspector text box.")]
  public float timeBetweenFrames = 0.0416666679f;
  public List<SkeletonAnimation> components;
  private float timeOfLastUpdate;
  private float accumulator;
  private bool doLateUpdate;

  private void OnValidate()
  {
    if ((double) this.timeBetweenFrames >= 0.0)
      return;
    this.timeBetweenFrames = 0.0f;
  }

  private void OnEnable() => this.timeOfLastUpdate = Time.time;

  private void Start()
  {
    int index = 0;
    for (int count = this.components.Count; index < count; ++index)
    {
      SkeletonAnimation component = this.components[index];
      component.Initialize(false);
      component.clearStateOnDisable = false;
      component.enabled = false;
      component.Update(0.0f);
      component.LateUpdate();
    }
  }

  private void Update()
  {
    if ((double) this.timeBetweenFrames <= 0.0)
      return;
    this.accumulator += Time.deltaTime;
    bool flag = false;
    if ((double) this.accumulator > (double) this.timeBetweenFrames)
    {
      this.accumulator %= this.timeBetweenFrames;
      flag = true;
    }
    if (!flag)
      return;
    float time = Time.time;
    float deltaTime = time - this.timeOfLastUpdate;
    int index = 0;
    for (int count = this.components.Count; index < count; ++index)
      this.components[index].Update(deltaTime);
    this.doLateUpdate = true;
    this.timeOfLastUpdate = time;
  }

  private void LateUpdate()
  {
    if (!this.doLateUpdate)
      return;
    int index = 0;
    for (int count = this.components.Count; index < count; ++index)
      this.components[index].LateUpdate();
    this.doLateUpdate = false;
  }
}
