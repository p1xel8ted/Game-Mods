// Decompiled with JetBrains decompiler
// Type: FaithCanvasOptimization
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FaithCanvasOptimization : MonoBehaviour
{
  [SerializeField]
  public Canvas canvasToOptimize;
  [SerializeField]
  public GameObject[] gameObjectsToTrack;
  [SerializeField]
  public MonoBehaviour[] callUpdateScriptsByForce;
  public IUpdateManually[] callUpdateScriptsByForceArray;

  public void ActivateCanvas()
  {
    if (!((Object) this.canvasToOptimize != (Object) null))
      return;
    this.canvasToOptimize.gameObject.SetActive(true);
  }
}
