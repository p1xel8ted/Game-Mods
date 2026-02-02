// Decompiled with JetBrains decompiler
// Type: SteamTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
