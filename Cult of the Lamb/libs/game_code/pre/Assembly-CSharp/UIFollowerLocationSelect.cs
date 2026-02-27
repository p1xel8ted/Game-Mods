// Decompiled with JetBrains decompiler
// Type: UIFollowerLocationSelect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIFollowerLocationSelect : BaseMonoBehaviour
{
  public UI_NavigatorSimple UINav;
  public Transform BaseContainer;
  public Transform LocationContainer;
  public GameObject IconPrefab;
  private FollowerInformationBox followerInfoBox;
  private GameObject g;
  private FollowerInformationBox icon;

  private void OnEnable()
  {
    this.UINav.OnSelectDown += new System.Action(this.OnSelect);
    this.UINav.OnDefaultSetComplete += new System.Action(this.OnDefaultSetComplete);
    this.UINav.OnChangeSelection += new UI_NavigatorSimple.ChangeSelection(this.OnChangeSelection);
    this.UINav.OnCancelDown += new System.Action(this.OnCancelClose);
  }

  private void OnDisable()
  {
    this.UINav.OnSelectDown -= new System.Action(this.OnSelect);
    this.UINav.OnDefaultSetComplete -= new System.Action(this.OnDefaultSetComplete);
    this.UINav.OnChangeSelection -= new UI_NavigatorSimple.ChangeSelection(this.OnChangeSelection);
    this.UINav.OnCancelDown -= new System.Action(this.OnCancelClose);
  }

  private void Start()
  {
    Time.timeScale = 0.0f;
    this.Populate(FollowerLocation.Base, this.BaseContainer);
    this.Populate(PlayerFarming.Location, this.LocationContainer);
  }

  private void Populate(FollowerLocation Location, Transform Container)
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

  private void OnDefaultSetComplete()
  {
    this.OnChangeSelection(this.UINav.selectable, (Selectable) null);
  }

  private void OnChangeSelection(Selectable NewSelectable, Selectable PrevSelectable)
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

  private void OnCancelClose()
  {
    Time.timeScale = 1f;
    HUD_Manager.Instance.Show();
    this.Close();
  }

  private void Close()
  {
    AudioManager.Instance.PlayOneShot("event:/upgrade_statue/upgrade_statue_close", this.gameObject);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
