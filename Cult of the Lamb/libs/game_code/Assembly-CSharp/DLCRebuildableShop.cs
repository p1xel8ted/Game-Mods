// Decompiled with JetBrains decompiler
// Type: DLCRebuildableShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class DLCRebuildableShop : Interaction
{
  public static System.Action OnRestored;
  [SerializeField]
  public DataManager.Variables requiredVariableToBuild;
  [SerializeField]
  public int cost;
  [Space]
  [SerializeField]
  public int id;
  [SerializeField]
  public GameObject broken;
  [SerializeField]
  public GameObject restored;
  [SerializeField]
  public GameObject canRestoreEffect;
  [SerializeField]
  public ParticleSystem repairParticles;
  [Space]
  [SerializeField]
  public UnityEvent onBuild;

  public int Cost => this.cost;

  public bool canRestore
  {
    get
    {
      return !this.IsRestored && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YEW_HOLY) >= this.cost && this.Interactable;
    }
  }

  public bool IsRestored => DataManager.Instance.ShopsBuilt.Contains(this.id);

  public void Start()
  {
    if (this.IsRestored)
    {
      this.SetRestored();
    }
    else
    {
      if ((UnityEngine.Object) this.repairParticles != (UnityEngine.Object) null)
        this.repairParticles.gameObject.SetActive(Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YEW_HOLY) >= this.cost);
      this.SetBroken();
    }
    this.Interactable = DataManager.Instance.GetVariable(this.requiredVariableToBuild);
  }

  public new void Update()
  {
    base.Update();
    if (!(bool) (UnityEngine.Object) this.canRestoreEffect)
      return;
    this.canRestoreEffect.SetActive(this.canRestore);
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (this.Interactable && !this.IsRestored)
    {
      this.Label = $"{string.Format(ScriptLocalization.Interactions.Repair, (object) "")}: {InventoryItem.CapacityString(InventoryItem.ITEM_TYPE.YEW_HOLY, this.cost)}";
    }
    else
    {
      this.Interactable = false;
      this.Label = "";
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YEW_HOLY) >= this.cost)
    {
      base.OnInteract(state);
      this.StartCoroutine((IEnumerator) this.RepairIE());
    }
    else
      state.GetComponent<PlayerFarming>().indicator.PlayShake();
  }

  public IEnumerator RepairIE()
  {
    DLCRebuildableShop dlcRebuildableShop = this;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
    AudioManager.Instance.PlayOneShot("event:/dlc/env/woolhaven/repair_ghost_building_interact", dlcRebuildableShop.transform.position);
    AudioManager.Instance.PlayAtmos("event:/dlc/music/woolhaven/repair_ghost_building_interact");
    GameManager.GetInstance().OnConversationNew();
    dlcRebuildableShop.playerFarming.GoToAndStop(dlcRebuildableShop.transform.position + Vector3.down * 1f, DisableCollider: true, maxDuration: 3f, forcePositionOnTimeout: true);
    while (dlcRebuildableShop.playerFarming.GoToAndStopping)
      yield return (object) null;
    float increment = 0.5f / (float) dlcRebuildableShop.cost;
    for (int i = 0; i < dlcRebuildableShop.cost; ++i)
    {
      ResourceCustomTarget.Create(dlcRebuildableShop.gameObject, dlcRebuildableShop._playerFarming.transform.position, InventoryItem.ITEM_TYPE.YEW_HOLY, (System.Action) null);
      yield return (object) new WaitForSeconds(increment);
    }
    Inventory.ChangeItemQuantity(231, -dlcRebuildableShop.cost);
    float duration = 3f;
    CameraManager.instance.ShakeCameraForDuration(2f, 2f, duration);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/woolhaven/repair_ghost_building");
    if ((UnityEngine.Object) dlcRebuildableShop.repairParticles != (UnityEngine.Object) null)
    {
      dlcRebuildableShop.repairParticles.gameObject.SetActive(true);
      dlcRebuildableShop.repairParticles.Play();
    }
    DG.Tweening.Sequence sequence1 = DOTween.Sequence();
    Vector3 localPosition = dlcRebuildableShop.transform.localPosition;
    for (int index = 0; index < 15; ++index)
    {
      sequence1.Append((Tween) dlcRebuildableShop.transform.DOLocalMove(localPosition + Vector3.up * (float) ((double) UnityEngine.Random.value * 0.15000000596046448 - 0.30000001192092896), (float) (0.075000002980232239 + (double) UnityEngine.Random.value * 0.05000000074505806)).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad));
      sequence1.Append((Tween) dlcRebuildableShop.transform.DOLocalMove(localPosition + Vector3.down * (float) ((double) UnityEngine.Random.value * 0.15000000596046448 - 0.30000001192092896), (float) (0.075000002980232239 + (double) UnityEngine.Random.value * 0.05000000074505806)).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad));
    }
    sequence1.Append((Tween) dlcRebuildableShop.transform.DOLocalMove(localPosition, 0.05f));
    sequence1.Play<DG.Tweening.Sequence>();
    dlcRebuildableShop.restored.SetActive(true);
    dlcRebuildableShop.restored.transform.localScale = Vector3.zero;
    DG.Tweening.Sequence sequence2 = DOTween.Sequence();
    sequence2.Join((Tween) dlcRebuildableShop.broken.transform.DOScale(Vector3.zero, duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack));
    sequence2.Join((Tween) dlcRebuildableShop.restored.transform.DOScale(Vector3.one, duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
    sequence2.Play<DG.Tweening.Sequence>();
    yield return (object) sequence2.WaitForCompletion();
    dlcRebuildableShop.broken.SetActive(false);
    if ((UnityEngine.Object) dlcRebuildableShop.repairParticles != (UnityEngine.Object) null)
      dlcRebuildableShop.repairParticles.Stop();
    yield return (object) new WaitForSeconds(0.75f);
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    if ((UnityEngine.Object) dlcRebuildableShop.repairParticles != (UnityEngine.Object) null)
      dlcRebuildableShop.repairParticles.gameObject.SetActive(false);
    dlcRebuildableShop.Restore();
    dlcRebuildableShop.onBuild?.Invoke();
  }

  public void SetBroken()
  {
    this.restored.gameObject.SetActive(false);
    this.broken.gameObject.SetActive(true);
  }

  public void SetRestored()
  {
    this.restored.gameObject.SetActive(true);
    this.broken.gameObject.SetActive(false);
  }

  public void Restore()
  {
    if (!DataManager.Instance.ShopsBuilt.Contains(this.id))
      DataManager.Instance.ShopsBuilt.Add(this.id);
    this.SetRestored();
    System.Action onRestored = DLCRebuildableShop.OnRestored;
    if (onRestored != null)
      onRestored();
    this.UpdateCarving();
  }

  public void Break()
  {
    if (DataManager.Instance.ShopsBuilt.Contains(this.id))
      DataManager.Instance.ShopsBuilt.Remove(this.id);
    this.SetBroken();
  }

  public void SetInteractable() => this.Interactable = true;

  public void UpdateCarving()
  {
    foreach (Collider2D componentsInChild in this.restored.GetComponentsInChildren<Collider2D>())
    {
      if ((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) null)
        AstarPath.active.UpdateGraphs(componentsInChild.bounds);
    }
  }
}
