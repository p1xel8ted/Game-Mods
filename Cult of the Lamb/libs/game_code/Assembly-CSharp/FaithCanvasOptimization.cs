// Decompiled with JetBrains decompiler
// Type: FaithCanvasOptimization
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
