// Decompiled with JetBrains decompiler
// Type: WinterConvoSetup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WinterConvoSetup : MonoBehaviour
{
  [SerializeField]
  public Interaction_SimpleConversation _normalConversation;
  [SerializeField]
  public Interaction_SimpleConversation _winterConversation;
  [SerializeField]
  public DataManager.Variables m_DoneFlag;

  public bool IsWinter => DataManager.Instance.CurrentSeason == SeasonsManager.Season.Winter;

  public void Awake()
  {
    DataManager.Instance.GetVariable(this.m_DoneFlag);
    if (!this.IsWinter || !((Object) this._winterConversation != (Object) null))
      return;
    this._normalConversation?.gameObject.SetActive(false);
    this._winterConversation.gameObject.SetActive(true);
  }
}
