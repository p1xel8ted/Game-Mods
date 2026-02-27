// Decompiled with JetBrains decompiler
// Type: SteamTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Steamworks;
using TMPro;
using UnityEngine;

#nullable disable
public class SteamTest : BaseMonoBehaviour
{
  public TextMeshProUGUI Text;

  private void Start()
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

  private void OnEnable()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.UpdateLocalisation);
  }

  private void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.UpdateLocalisation);
  }

  private void UpdateLocalisation()
  {
    this.Text.text = $"{ScriptLocalization.UI_MainMenu.Welcome} {this.name}";
  }
}
