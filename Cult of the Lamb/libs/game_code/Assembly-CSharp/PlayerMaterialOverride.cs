// Decompiled with JetBrains decompiler
// Type: PlayerMaterialOverride
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
