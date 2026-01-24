// Decompiled with JetBrains decompiler
// Type: Interaction_FlowerPotWoolhaven
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_FlowerPotWoolhaven : Interaction
{
  public const int NumberOfFlowerPots = 10;
  public static List<Interaction_FlowerPotWoolhaven> flowerPots = new List<Interaction_FlowerPotWoolhaven>();
  [SerializeField]
  public int ID;
  [SerializeField]
  public int flowersRequired;
  [SerializeField]
  public Material mossRevealMaterial;
  [SerializeField]
  public Material mossMaterial;
  [SerializeField]
  public GameObject flowersContainer;
  [SerializeField]
  public GameObject mossContainer;
  [SerializeField]
  public GameObject cameraTarget;
  [SerializeField]
  public List<MeshRenderer> customMossMeshRenderers;
  public SpriteRenderer[] _mossRenderers;
  public MaterialPropertyBlock _mossMPB;
  public static int RotRevealID = Shader.PropertyToID("_RotReveal");
  public static int MossRevealID = Shader.PropertyToID("_Moss");
  public EventInstance flowerBloomLoopInstance;
  public string flowerBloomLoopSFX = "event:/dlc/env/woolhaven/flowerpot_bloom_loop";

  public void Awake()
  {
    this._mossRenderers = this.mossContainer.GetComponentsInChildren<SpriteRenderer>(true);
    this._mossMPB = new MaterialPropertyBlock();
    foreach (Renderer mossRenderer in this._mossRenderers)
      mossRenderer.sharedMaterial = this.mossRevealMaterial;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    bool flag = DataManager.GetFlowerPotProgress(this.ID) >= this.flowersRequired;
    this.flowersContainer.gameObject.SetActive(flag);
    this.mossContainer.gameObject.SetActive(flag);
    this.SetCustomMossAmount(flag ? 1f : 0.0f);
    if (!flag)
      return;
    this.SetAllRenderersToDefaultMossMaterial();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    AudioManager.Instance.StopLoop(this.flowerBloomLoopInstance);
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (DataManager.GetFlowerPotProgress(this.ID) >= this.flowersRequired)
    {
      this.Interactable = false;
      this.Label = "";
    }
    else
      this.Label = LocalizationManager.GetTranslation("Interactions/DepositFlowers") + $" {DataManager.GetFlowerPotProgress(this.ID)}/{this.flowersRequired}".Colour(Color.gray);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    UIManager instance = MonoSingleton<UIManager>.Instance;
    PlayerFarming playerFarming = this.playerFarming;
    List<InventoryItem.ITEM_TYPE> items = new List<InventoryItem.ITEM_TYPE>();
    items.Add(this.ID % 2 == 0 ? InventoryItem.ITEM_TYPE.FLOWER_PURPLE : InventoryItem.ITEM_TYPE.FLOWER_WHITE);
    ItemSelector.Params parameters = new ItemSelector.Params()
    {
      Key = "plant_flower",
      Context = ItemSelector.Context.Add,
      Offset = new Vector2(0.0f, 100f),
      RequiresDiscovery = false,
      ShowProgressText = true,
      ShowEmpty = true,
      DontCache = true
    };
    UIItemSelectorOverlayController itemSelector = instance.ShowItemSelector(playerFarming, items, parameters, true);
    string label = this.Label;
    itemSelector.setProgressString(label);
    itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem =>
    {
      AudioManager.Instance.PlayOneShot("event:/material/footstep_sand", this.gameObject);
      Inventory.ChangeItemQuantity((int) chosenItem, -1);
      DataManager.IncrementFlowerPotProgress(this.ID);
      itemSelector.setProgressString(this.Label);
      if (DataManager.GetFlowerPotProgress(this.ID) >= this.flowersRequired)
      {
        DataManager.SetFlowerPotAsFull(this.ID);
        this.StartCoroutine((IEnumerator) this.RevealFlowersIE());
        itemSelector.Hide();
      }
      else
        ResourceCustomTarget.Create(this.gameObject, state.transform.position, chosenItem, (System.Action) (() =>
        {
          this.transform.DOKill();
          this.transform.localScale = Vector3.one * 1.25f;
          this.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => { }));
        }));
    });
    UIItemSelectorOverlayController overlayController = itemSelector;
    overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
    {
      bool flag = false;
      foreach (UIItemSelectorOverlayController selectorOverlay in UIItemSelectorOverlayController.SelectorOverlays)
      {
        if ((UnityEngine.Object) selectorOverlay != (UnityEngine.Object) itemSelector && (UnityEngine.Object) selectorOverlay.playerFarming == (UnityEngine.Object) this.playerFarming)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        state.CURRENT_STATE = LetterBox.IsPlaying || MMConversation.isPlaying ? StateMachine.State.InActive : StateMachine.State.Idle;
      itemSelector = (UIItemSelectorOverlayController) null;
      this.Interactable = true;
      this.HasChanged = true;
    });
  }

  public void SetCustomMossAmount(float amount)
  {
    foreach (MeshRenderer mossMeshRenderer in this.customMossMeshRenderers)
    {
      if ((bool) (UnityEngine.Object) mossMeshRenderer && mossMeshRenderer.materials != null && mossMeshRenderer.materials.Length != 0)
        mossMeshRenderer.materials[0].SetFloat(Interaction_FlowerPotWoolhaven.MossRevealID, amount);
    }
  }

  public void PlayRevealFlowers() => this.StartCoroutine((IEnumerator) this.RevealFlowersIE());

  public IEnumerator RevealMossIE()
  {
    Interaction_FlowerPotWoolhaven flowerPotWoolhaven = this;
    flowerPotWoolhaven.mossContainer.gameObject.SetActive(true);
    flowerPotWoolhaven.SetCustomMossAmount(0.0f);
    float duration = 3f;
    AudioManager.Instance.PlayOneShot("event:/Stings/white_eyes", flowerPotWoolhaven.gameObject);
    AudioManager.Instance.PlayOneShot("event:/comic sfx/earth_break_land", flowerPotWoolhaven.gameObject);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.5f, duration);
    float value = 0.0f;
    foreach (MeshRenderer mossMeshRenderer in flowerPotWoolhaven.customMossMeshRenderers)
    {
      if ((bool) (UnityEngine.Object) mossMeshRenderer && mossMeshRenderer.materials != null && mossMeshRenderer.materials.Length != 0)
        mossMeshRenderer.materials[0].DOFloat(1f, Interaction_FlowerPotWoolhaven.MossRevealID, 3f).From<float, float, FloatOptions>(0.0f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear);
    }
    Tween t = (Tween) DOTween.To((DOGetter<float>) (() => value), (DOSetter<float>) (v =>
    {
      value = v;
      foreach (SpriteRenderer mossRenderer in this._mossRenderers)
      {
        if ((bool) (UnityEngine.Object) mossRenderer)
        {
          mossRenderer.material = this.mossRevealMaterial;
          mossRenderer.GetPropertyBlock(this._mossMPB);
          this._mossMPB.SetFloat(Interaction_FlowerPotWoolhaven.RotRevealID, value);
          mossRenderer.SetPropertyBlock(this._mossMPB);
        }
      }
    }), 1f, duration).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear);
    yield return (object) new WaitForSeconds(1f);
    yield return (object) t.WaitForCompletion();
    flowerPotWoolhaven.SetAllRenderersToDefaultMossMaterial();
  }

  public void SetAllRenderersToDefaultMossMaterial()
  {
    foreach (SpriteRenderer mossRenderer in this._mossRenderers)
    {
      if ((bool) (UnityEngine.Object) mossRenderer)
        mossRenderer.sharedMaterial = this.mossMaterial;
    }
  }

  public IEnumerator RevealFlowersIE()
  {
    Interaction_FlowerPotWoolhaven flowerPotWoolhaven = this;
    GameManager.SetGlobalOcclusionActive(false);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/woolhaven/flowerpot_fillup_complete", flowerPotWoolhaven.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(flowerPotWoolhaven.cameraTarget, 6f);
    int num1 = (int) AudioManager.Instance.CurrentMusicInstance.setPaused(true);
    for (int index = 0; index < flowerPotWoolhaven.flowersContainer.transform.childCount; ++index)
      flowerPotWoolhaven.flowersContainer.transform.GetChild(index).gameObject.SetActive(false);
    yield return (object) flowerPotWoolhaven.RevealMossIE();
    flowerPotWoolhaven.flowersContainer.gameObject.SetActive(true);
    flowerPotWoolhaven.flowerBloomLoopInstance = AudioManager.Instance.CreateLoop(flowerPotWoolhaven.flowerBloomLoopSFX, true);
    for (int i = 0; i < flowerPotWoolhaven.flowersContainer.transform.childCount; ++i)
    {
      flowerPotWoolhaven.flowersContainer.transform.GetChild(i).gameObject.SetActive(true);
      flowerPotWoolhaven.flowersContainer.transform.GetChild(i).transform.DOPunchScale(Vector3.one * 0.2f, 0.5f);
      AudioManager.Instance.PlayOneShot("event:/dlc/env/woolhaven/flowerpot_bloom", flowerPotWoolhaven.flowersContainer.transform.GetChild(i).transform.position);
      yield return (object) new WaitForSeconds(0.2f);
    }
    AudioManager.Instance.StopLoop(flowerPotWoolhaven.flowerBloomLoopInstance);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    flowerPotWoolhaven.HasChanged = true;
    int num2 = (int) AudioManager.Instance.CurrentMusicInstance.setPaused(false);
    GameManager.SetGlobalOcclusionActive(true);
  }
}
