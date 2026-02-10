// Decompiled with JetBrains decompiler
// Type: LogisticsVisual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Ara;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class LogisticsVisual : MonoBehaviour
{
  public static List<LogisticsVisual> logisticVisuals = new List<LogisticsVisual>();
  [SerializeField]
  public AraTrail trail;
  [SerializeField]
  public AnimationCurve height;
  public float duration;
  public float timer;
  public Vector3 from;
  public Vector3 to;
  public bool trailHidden;

  public static void Spawn(Vector3 from, Vector3 to, float duration, Color color)
  {
    Addressables.InstantiateAsync((object) "Assets/Prefabs/Structures/Other/LogisticsVisual.prefab", from, Quaternion.identity).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      LogisticsVisual component = obj.Result.GetComponent<LogisticsVisual>();
      component.transform.SetParent(BiomeBaseManager.Instance.Room.transform, true);
      component.Configure(from, to, duration, color);
      LogisticsVisual.logisticVisuals.Add(component);
    });
  }

  public static void ClearAll()
  {
    for (int index = LogisticsVisual.logisticVisuals.Count - 1; index >= 0; --index)
      UnityEngine.Object.Destroy((UnityEngine.Object) LogisticsVisual.logisticVisuals[index]);
  }

  public void Configure(Vector3 from, Vector3 to, float duration, Color color)
  {
    this.from = from;
    this.to = to;
    this.duration = duration;
    Gradient gradient = new Gradient()
    {
      colorKeys = new GradientColorKey[1]
      {
        new GradientColorKey(color, 0.0f)
      }
    };
    this.trail.colorOverLength = gradient;
    this.trail.colorOverTime = gradient;
    this.trailHidden = false;
    CheatConsole.OnHideUI += new System.Action(this.HideTrail);
    CheatConsole.OnShowUI += new System.Action(this.ShowTrail);
    PhotoModeManager.OnPhotoModeEnabled += new PhotoModeManager.PhotoEvent(this.HideTrail);
    PhotoModeManager.OnPhotoModeDisabled += new PhotoModeManager.PhotoEvent(this.ShowTrail);
  }

  public void Update()
  {
    if ((double) Time.timeScale == 0.0 && !this.trailHidden)
      this.HideTrail();
    else if ((double) Time.timeScale > 0.0 && this.trailHidden)
      this.ShowTrail();
    this.timer += Time.deltaTime;
    float num = this.timer / this.duration;
    Vector3 vector3 = Vector3.Lerp(this.from, this.to, num);
    vector3.z += this.height.Evaluate(num) * -1f;
    this.transform.position = vector3;
    if ((double) num < 1.5)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void OnDestroy()
  {
    CheatConsole.OnHideUI -= new System.Action(this.HideTrail);
    CheatConsole.OnShowUI -= new System.Action(this.ShowTrail);
    PhotoModeManager.OnPhotoModeEnabled -= new PhotoModeManager.PhotoEvent(this.HideTrail);
    PhotoModeManager.OnPhotoModeDisabled -= new PhotoModeManager.PhotoEvent(this.ShowTrail);
    LogisticsVisual.logisticVisuals.Remove(this);
  }

  public void ShowTrail()
  {
    this.trailHidden = false;
    this.trail.gameObject.SetActive(true);
  }

  public void HideTrail()
  {
    this.trailHidden = true;
    this.trail.gameObject.SetActive(false);
  }
}
