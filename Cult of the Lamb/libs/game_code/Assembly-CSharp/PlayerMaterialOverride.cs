// Decompiled with JetBrains decompiler
// Type: PlayerMaterialOverride
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
