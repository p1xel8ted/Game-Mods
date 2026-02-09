// Decompiled with JetBrains decompiler
// Type: FaithCanvasOptimization
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
