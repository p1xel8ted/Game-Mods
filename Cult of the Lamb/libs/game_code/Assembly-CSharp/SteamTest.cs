// Decompiled with JetBrains decompiler
// Type: SteamTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Steamworks;
using TMPro;
using UnityEngine;

#nullable disable
public class SteamTest : BaseMonoBehaviour
{
  public TextMeshProUGUI Text;

  public void Start()
  {
    if (SteamManager.Initialized)
    {
      string personaName = SteamFriends.GetPersonaName();
      Debug.Log((object) (">>>>>>>>> " + personaName));
      this.Text.text = $"{ScriptLocalization.UI_MainMenu.Welcome} {personaName}";
    }
    else
      this.Text.text = "";
  }

  public void OnEnable()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.UpdateLocalisation);
  }

  public void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.UpdateLocalisation);
  }

  public void UpdateLocalisation()
  {
    this.Text.text = $"{ScriptLocalization.UI_MainMenu.Welcome} {this.name}";
  }
}
