// Decompiled with JetBrains decompiler
// Type: IndulgenceShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class IndulgenceShop : MonoBehaviour
{
  [SerializeField]
  public Interaction_IndulgenceShop interactionIndulgenceShop;
  public Coroutine ShopRestarting;

  public void Start()
  {
    this.RefreshDecorations();
    this.CheckVisibility();
  }

  public void CheckVisibility()
  {
    if (DataManager.Instance.TempleLevel == -1)
    {
      this.gameObject.SetActive(false);
      DataManager.Instance.TempleLevel = 0;
    }
    else if (DataManager.Instance.DeathCatBeaten)
      this.gameObject.SetActive(false);
    else
      this.gameObject.SetActive(true);
  }

  public void RefreshDecorations()
  {
    for (int index = 0; index < this.interactionIndulgenceShop.indulgenceDecorations.Length; ++index)
      this.interactionIndulgenceShop.indulgenceDecorations[index].gameObject.SetActive(index < DataManager.Instance.TempleLevel);
  }

  public void BoughtIndulgence()
  {
    ++DataManager.Instance.TempleLevel;
    this.RefreshDecorations();
    if (this.ShopRestarting != null)
      this.StopCoroutine(this.ShopRestarting);
    this.ShopRestarting = this.StartCoroutine((IEnumerator) this.RestartShop());
  }

  public IEnumerator RestartShop()
  {
    Debug.Log((object) "Restarting!");
    yield return (object) new WaitForSeconds(2f);
    GameManager.GetInstance().OnConversationEnd();
    PlayerFarming.Instance._state.CURRENT_STATE = StateMachine.State.Idle;
    this.interactionIndulgenceShop.Interactable = true;
    Debug.Log((object) "Restarted!!");
  }

  public void OnEnable()
  {
    this.interactionIndulgenceShop.BoughtIndulgenceCallback += new System.Action(this.BoughtIndulgence);
    this.CheckVisibility();
    this.RefreshDecorations();
  }

  public void OnDisable()
  {
    this.interactionIndulgenceShop.BoughtIndulgenceCallback -= new System.Action(this.BoughtIndulgence);
  }
}
