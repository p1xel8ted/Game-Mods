// Decompiled with JetBrains decompiler
// Type: MMPause_ToggleCamEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class MMPause_ToggleCamEffects : BaseMonoBehaviour
{
  public List<GameObject> Disable;
  public TextMeshProUGUI Text;

  public void OnEnable()
  {
    this.Disable = new List<GameObject>();
    this.Disable.Add(GameObject.Find("Shadow_Camera_RenderText"));
    this.Disable.Add(GameObject.Find("Lighting_Camera_RenderText"));
    this.Disable.Add(GameObject.Find("Scenery_Camera_RenderText"));
    this.Disable.Add(GameObject.Find("RoomEffects_Julian"));
    this.Text.text = "Cam Effects: " + this.Disable[0].activeSelf.ToString();
  }

  public void ToggleCamEffects()
  {
    bool activeSelf = this.Disable[0].activeSelf;
    foreach (GameObject gameObject in this.Disable)
      gameObject.SetActive(!activeSelf);
    this.Text.text = "Cam Effects: " + this.Disable[0].activeSelf.ToString();
  }
}
