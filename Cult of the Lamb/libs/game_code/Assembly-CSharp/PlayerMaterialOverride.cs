// Decompiled with JetBrains decompiler
// Type: PlayerMaterialOverride
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class PlayerMaterialOverride : MonoBehaviour
{
  [SerializeField]
  public SkeletonRendererCustomMaterials skeletonRendererCustomMaterials;

  public void OnEnable()
  {
    if (SceneManager.GetActiveScene().name == "Woolhaven Intro")
      this.skeletonRendererCustomMaterials.enabled = true;
    else
      this.skeletonRendererCustomMaterials.enabled = false;
  }
}
