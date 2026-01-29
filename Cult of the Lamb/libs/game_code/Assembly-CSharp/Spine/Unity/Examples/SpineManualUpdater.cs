// Decompiled with JetBrains decompiler
// Type: Spine.Unity.Examples.SpineManualUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public float timeOfLastUpdate;
  public float accumulator;
  public bool doLateUpdate;

  public void OnEnable() => this.timeOfLastUpdate = Time.time;

  public void Start()
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

  public void Update()
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

  public void LateUpdate()
  {
    if (!this.doLateUpdate)
      return;
    int index = 0;
    for (int count = this.components.Count; index < count; ++index)
      this.components[index].LateUpdate();
    this.doLateUpdate = false;
  }
}
