// Decompiled with JetBrains decompiler
// Type: UIFollowerLocationSelect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIFollowerLocationSelect : BaseMonoBehaviour
{
  public UI_NavigatorSimple UINav;
  public Transform BaseContainer;
  public Transform LocationContainer;
  public GameObject IconPrefab;
  public FollowerInformationBox followerInfoBox;
  public GameObject g;
  public FollowerInformationBox icon;

  public void OnEnable()
  {
    this.UINav.OnSelectDown += new System.Action(this.OnSelect);
    this.UINav.OnDefaultSetComplete += new System.Action(this.OnDefaultSetComplete);
    this.UINav.OnChangeSelection += new UI_NavigatorSimple.ChangeSelection(this.OnChangeSelection);
    this.UINav.OnCancelDown += new System.Action(this.OnCancelClose);
  }

  public void OnDisable()
  {
    this.UINav.OnSelectDown -= new System.Action(this.OnSelect);
    this.UINav.OnDefaultSetComplete -= new System.Action(this.OnDefaultSetComplete);
    this.UINav.OnChangeSelection -= new UI_NavigatorSimple.ChangeSelection(this.OnChangeSelection);
    this.UINav.OnCancelDown -= new System.Action(this.OnCancelClose);
  }

  public void Start()
  {
    Time.timeScale = 0.0f;
    this.Populate(FollowerLocation.Base, this.BaseContainer);
    this.Populate(PlayerFarming.Location, this.LocationContainer);
  }

  public void Populate(FollowerLocation Location, Transform Container)
  {
    foreach (FollowerBrain followerBrain in FollowerManager.FollowerBrainsByHomeLocation(Location))
    {
      this.g = UnityEngine.Object.Instantiate<GameObject>(this.IconPrefab, Container);
      this.g.SetActive(true);
      this.icon = this.g.GetComponent<FollowerInformationBox>();
      this.icon.Configure(followerBrain._directInfoAccess);
      this.icon.followBrain = followerBrain;
      if ((UnityEngine.Object) this.UINav.selectable == (UnityEngine.Object) null)
      {
        this.UINav.startingItem = this.g.GetComponent<Selectable>();
        this.UINav.setDefault();
      }
    }
  }

  public void OnDefaultSetComplete()
  {
    this.OnChangeSelection(this.UINav.selectable, (Selectable) null);
  }

  public void OnChangeSelection(Selectable NewSelectable, Selectable PrevSelectable)
  {
    AudioManager.Instance.PlayOneShot("event:/upgrade_statue/upgrade_statue_scroll", this.gameObject);
  }

  public void OnSelect()
  {
    if ((UnityEngine.Object) this.UINav.selectable == (UnityEngine.Object) null)
      return;
    this.icon = this.UINav.selectable.GetComponent<FollowerInformationBox>();
    if ((UnityEngine.Object) this.icon.transform.parent == (UnityEngine.Object) this.BaseContainer)
    {
      this.icon.transform.parent = this.LocationContainer;
      this.icon.followBrain.SetNewHomeLocation(PlayerFarming.Location);
      this.icon.followBrain.Stats.WorkerBeenGivenOrders = false;
      if (this.icon.followBrain.Info.FollowerRole == FollowerRole.Worshipper)
        this.icon.followBrain.Info.FollowerRole = FollowerRole.Worker;
      this.icon.followBrain.CompleteCurrentTask();
    }
    else
    {
      if (!((UnityEngine.Object) this.icon.transform.parent == (UnityEngine.Object) this.LocationContainer))
        return;
      this.icon.transform.parent = this.BaseContainer;
      this.icon.followBrain.SetNewHomeLocation(FollowerLocation.Base);
      this.icon.followBrain.CompleteCurrentTask();
    }
  }

  public void OnCancelClose()
  {
    Time.timeScale = 1f;
    HUD_Manager.Instance.Show();
    this.Close();
  }

  public void Close()
  {
    AudioManager.Instance.PlayOneShot("event:/upgrade_statue/upgrade_statue_close", this.gameObject);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
