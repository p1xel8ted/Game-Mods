// Decompiled with JetBrains decompiler
// Type: MMPause_ToggleCamEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class MMPause_ToggleCamEffects : BaseMonoBehaviour
{
  public List<GameObject> Disable;
  public TextMeshProUGUI Text;

  private void OnEnable()
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
