// Decompiled with JetBrains decompiler
// Type: FaithCanvasOptimization
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
