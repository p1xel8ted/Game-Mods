// Decompiled with JetBrains decompiler
// Type: ClothingShopKeeperManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class ClothingShopKeeperManager : MonoBehaviour
{
  [SerializeField]
  public Interaction_SimpleConversation introConvo;

  public void Awake()
  {
    if (!DataManager.Instance.RevealedTailor)
      MonoSingleton<UIManager>.Instance.LoadUpgradeTree();
    this.introConvo.gameObject.SetActive(!DataManager.Instance.RevealedTailor);
    if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.TailorSystem) || DataManager.Instance.RevealedTailor)
      return;
    this.introConvo.Entries[3].TermToSpeak = "Conversation_NPC/SilkWorm/Intro/3_Alt";
  }

  public void RevealTailor()
  {
    DataManager.Instance.RevealedTailor = true;
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.TailorSystem))
    {
      GameManager.GetInstance().OnConversationEnd();
      if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Tailor))
        return;
      this.AddObjectives();
    }
    else
      MonoSingleton<UIManager>.Instance.ShowUpgradeTree((System.Action) (() =>
      {
        GameManager.GetInstance().OnConversationEnd();
        MonoSingleton<UIManager>.Instance.UnloadUpgradeTree();
        this.AddObjectives();
      }), UpgradeSystem.Type.TailorSystem, true);
  }

  public void AddObjectives()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_UnlockUpgrade("Objectives/GroupTitles/Tailor", UpgradeSystem.Type.Building_Tailor), true);
    ObjectiveManager.Add((ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/Tailor", StructureBrain.TYPES.TAILOR), true);
  }

  [CompilerGenerated]
  public void \u003CRevealTailor\u003Eb__2_0()
  {
    GameManager.GetInstance().OnConversationEnd();
    MonoSingleton<UIManager>.Instance.UnloadUpgradeTree();
    this.AddObjectives();
  }
}
