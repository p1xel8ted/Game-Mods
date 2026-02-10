// Decompiled with JetBrains decompiler
// Type: MMPause_ToggleCamEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
