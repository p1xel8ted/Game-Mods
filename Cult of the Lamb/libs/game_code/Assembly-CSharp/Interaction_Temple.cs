// Decompiled with JetBrains decompiler
// Type: Interaction_Temple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_Temple : BaseMonoBehaviour
{
  public GameObject DoorLight;
  public EnterBuilding Entrance;
  public static Interaction_Temple Instance;
  public GameObject ExitPosition;
  public Animator shrineLevelUpAnimator;
  public Structure Structure;

  public void OnEnable()
  {
    Interaction_Temple.Instance = this;
    this.Structure = this.GetComponent<Structure>();
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    this.CheckDoorLight();
    TimeManager.OnNewDayStarted += new System.Action(this.CheckDoorLight);
    UpgradeSystem.OnCoolDownAdded += new System.Action(this.CheckXP);
    Inventory.OnInventoryUpdated += new Inventory.InventoryUpdated(this.CheckXP);
    this.CheckXP();
    this.shrineLevelUpAnimator.gameObject.SetActive(true);
  }

  public void OnDisable()
  {
    UpgradeSystem.OnCoolDownAdded -= new System.Action(this.CheckXP);
    Inventory.OnInventoryUpdated -= new Inventory.InventoryUpdated(this.CheckXP);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    TimeManager.OnNewDayStarted -= new System.Action(this.CheckDoorLight);
  }

  public void CheckXP()
  {
    if (UpgradeSystem.CanAffordDoctrine())
    {
      if (this.shrineLevelUpAnimator.GetCurrentAnimatorStateInfo(0).IsName("Shown"))
        return;
      this.shrineLevelUpAnimator.Play("Show");
    }
    else if (this.shrineLevelUpAnimator.GetCurrentAnimatorStateInfo(0).IsName("Shown"))
      this.shrineLevelUpAnimator.Play("Hide");
    else
      this.shrineLevelUpAnimator.Play("Hidden");
  }

  public void CheckDoorLight()
  {
    this.DoorLight.SetActive(DataManager.Instance.PreviousSermonDayIndex < TimeManager.CurrentDay);
  }

  public void Start()
  {
  }

  public void OnBrainAssigned()
  {
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && (double) Vector3.Distance(this.transform.position + new Vector3(0.0f, 2f), PlayerFarming.Instance.transform.position) <= 3.0)
    {
      BoxCollider2D[] Colliders = this.GetComponentsInChildren<BoxCollider2D>();
      Debug.Log((object) ("DISTANCE: " + Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position).ToString()));
      PlayerFarming.Instance.GoToAndStop(this.transform.position, this.gameObject, true, true, (System.Action) (() =>
      {
        Debug.Log((object) "COMPLETED!");
        Bounds bounds = new Bounds();
        foreach (BoxCollider2D boxCollider2D in Colliders)
        {
          boxCollider2D.enabled = true;
          bounds.Encapsulate(boxCollider2D.bounds);
        }
        AstarPath.active.UpdateGraphs(bounds);
      }));
      this.Entrance.Trigger.AddListener(new UnityAction(BiomeBaseManager.Instance.ActivateChurch));
    }
    else if ((UnityEngine.Object) this.Entrance != (UnityEngine.Object) null && this.Entrance.Trigger != null && (UnityEngine.Object) BiomeBaseManager.Instance != (UnityEngine.Object) null)
      this.Entrance.Trigger.AddListener(new UnityAction(BiomeBaseManager.Instance.ActivateChurch));
    if (this.Structure.Type == StructureBrain.TYPES.TEMPLE)
      DataManager.Instance.HasBuiltTemple1 = true;
    if (this.Structure.Type == StructureBrain.TYPES.TEMPLE_II)
      DataManager.Instance.HasBuiltTemple2 = true;
    if (this.Structure.Type == StructureBrain.TYPES.TEMPLE_III)
      DataManager.Instance.HasBuiltTemple3 = true;
    if (this.Structure.Type != StructureBrain.TYPES.TEMPLE_IV)
      return;
    DataManager.Instance.HasBuiltTemple4 = true;
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + new Vector3(0.0f, 2f), 3f, Color.green);
  }

  public void OnDestroy() => this.Entrance.Trigger.RemoveAllListeners();
}
