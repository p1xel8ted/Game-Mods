// Decompiled with JetBrains decompiler
// Type: HarvestTotem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HarvestTotem : Interaction
{
  public static float EFFECTIVE_DISTANCE = 7f;
  public static Vector3 Centre = new Vector3(0.0f, 0.0f);
  public SpriteRenderer RangeSprite;
  public static List<HarvestTotem> HarvestTotems = new List<HarvestTotem>();
  public GameObject ReceiveSoulPosition;
  public Structure Structure;
  public GameObject DevotionReady;
  public static int currentInstanceIndex = 0;
  public static List<HarvestTotem> instances = new List<HarvestTotem>();
  public static bool isUpdateRunning = false;
  public static object lockObject = new object();
  public int instanceIndex;
  [SerializeField]
  public SpriteXPBar XpBar;
  public string sString;
  public LayerMask playerMask;
  public bool Activating;
  public float Delay;
  public float DistanceToTriggerDeposits = 5f;
  public Color FadeOut = new Color(1f, 1f, 1f, 0.0f);
  public float DistanceRadius = 1f;
  public float Distance = 1f;
  public int FrameIntervalOffset;
  public int UpdateInterval = 2;
  public bool distanceChanged;
  public Vector3 _updatePos;

  public Structures_HarvestTotem StructureBrain => this.Structure.Brain as Structures_HarvestTotem;

  public override void OnEnable()
  {
    base.OnEnable();
    lock (HarvestTotem.lockObject)
    {
      this.instanceIndex = HarvestTotem.instances.Count;
      HarvestTotem.instances.Add(this);
    }
  }

  public override void OnDisable()
  {
    base.OnDisable();
    lock (HarvestTotem.lockObject)
    {
      HarvestTotem.instances.Remove(this);
      for (int index = 0; index < HarvestTotem.instances.Count; ++index)
        HarvestTotem.instances[index].instanceIndex = index;
      if (HarvestTotem.currentInstanceIndex < HarvestTotem.instances.Count)
        return;
      HarvestTotem.currentInstanceIndex = 0;
    }
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    if ((UnityEngine.Object) this.GetComponentInParent<PlacementObject>() == (UnityEngine.Object) null)
      this.RangeSprite.DOColor(this.FadeOut, 0.0f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    HarvestTotem.HarvestTotems.Add(this);
    this.Structure = this.GetComponentInChildren<Structure>();
    DataManager.Instance.ShrineLevel = 1;
    StructureManager.OnStructuresPlaced += new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    HarvestTotem.HarvestTotems.Remove(this);
  }

  public void Start()
  {
    this.RangeSprite.size = new Vector2(HarvestTotem.EFFECTIVE_DISTANCE, HarvestTotem.EFFECTIVE_DISTANCE);
    this.UpdateLocalisation();
    this.ContinuouslyHold = true;
    if ((UnityEngine.Object) this.XpBar != (UnityEngine.Object) null)
      this.XpBar.gameObject.SetActive(false);
    this.ActivateDistance = 2f;
    this.playerMask = (LayerMask) ((int) this.playerMask | 1 << LayerMask.NameToLayer("Player"));
  }

  public override void OnDrawGizmos()
  {
    base.OnDrawGizmos();
    Utils.DrawCircleXY(this.transform.position + HarvestTotem.Centre, HarvestTotem.EFFECTIVE_DISTANCE, Color.green);
    Utils.DrawCircleXY(this.transform.position + HarvestTotem.Centre, 0.5f, Color.red);
  }

  public void OnStructuresPlaced()
  {
    this.UpdateBar();
    StructureManager.OnStructuresPlaced -= new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
  }

  public void OnBrainAssigned()
  {
    if (this.Structure.Type == global::StructureBrain.TYPES.HARVEST_TOTEM)
      return;
    if ((double) this.StructureBrain.Data.LastPrayTime == -1.0)
      this.StructureBrain.Data.LastPrayTime = TimeManager.TotalElapsedGameTime + this.StructureBrain.TimeBetweenSouls;
    Structures_HarvestTotem structureBrain = this.StructureBrain;
    structureBrain.OnSoulsGained = structureBrain.OnSoulsGained + new System.Action<int>(this.OnSoulsGained);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.UpdateBar();
  }

  public new void OnDestroy()
  {
    if (!((UnityEngine.Object) this.Structure != (UnityEngine.Object) null) || this.StructureBrain == null)
      return;
    Structures_HarvestTotem structureBrain = this.StructureBrain;
    structureBrain.OnSoulsGained = structureBrain.OnSoulsGained - new System.Action<int>(this.OnSoulsGained);
  }

  public override void GetLabel()
  {
    if (this.Structure.Type == global::StructureBrain.TYPES.HARVEST_TOTEM)
    {
      this.Label = "";
    }
    else
    {
      this.Interactable = this.StructureBrain != null && this.StructureBrain.SoulCount > 0;
      string str1 = (GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0 ? "<sprite name=\"icon_spirits\">" : "<sprite name=\"icon_blackgold\">";
      if (LocalizeIntegration.IsArabic() && this.StructureBrain != null)
      {
        int num = this.StructureBrain.SoulCount;
        string str2 = LocalizeIntegration.ReverseText(num.ToString());
        num = this.StructureBrain.SoulMax;
        string str3 = LocalizeIntegration.ReverseText(num.ToString());
        string str4;
        if (this.StructureBrain == null)
          str4 = "";
        else
          str4 = $"{this.sString} {str1} x{str3}/{str2}";
        this.Label = str4;
      }
      else
      {
        string str5;
        if (this.StructureBrain == null)
        {
          str5 = "";
        }
        else
        {
          string[] strArray = new string[7]
          {
            this.sString,
            " ",
            str1,
            " x",
            null,
            null,
            null
          };
          int num = this.StructureBrain.SoulCount;
          strArray[4] = num.ToString();
          strArray[5] = "/";
          num = this.StructureBrain.SoulMax;
          strArray[6] = num.ToString();
          str5 = string.Concat(strArray);
        }
        this.Label = str5;
      }
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    this.IndicateHighlighted(this.playerFarming);
    this.Activating = true;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.ReceiveDevotion;
  }

  public void OnSoulsGained(int count) => this.UpdateBar();

  public void UpdateBar()
  {
    if ((UnityEngine.Object) this.XpBar == (UnityEngine.Object) null || this.StructureBrain == null)
      return;
    this.XpBar.UpdateBar(Mathf.Clamp((float) this.StructureBrain.SoulCount / (float) this.StructureBrain.SoulMax, 0.0f, 1f));
    if (!((UnityEngine.Object) this.DevotionReady != (UnityEngine.Object) null))
      return;
    this.DevotionReady.SetActive(this.StructureBrain.SoulCount > 0);
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null)
      return;
    this.Distance = Vector3.Distance(this.transform.position, this.playerFarming.transform.position);
    if (this.Activating && (this.StructureBrain.SoulCount <= 0 || InputManager.Gameplay.GetInteractButtonUp() || (double) this.Distance > (double) this.DistanceToTriggerDeposits))
      this.Activating = false;
    if ((double) (this.Delay -= Time.deltaTime) < 0.0 && this.Activating && this.StructureBrain.SoulCount > 0)
    {
      if ((GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0)
        SoulCustomTarget.Create(this.playerFarming.gameObject, this.ReceiveSoulPosition.transform.position, Color.white, new System.Action(this.GivePlayerSoul));
      else
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, this.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      --this.StructureBrain.SoulCount;
      this.Delay = 0.05f;
      this.UpdateBar();
    }
    if (this.StructureBrain != null && (double) this.StructureBrain.Data.LastPrayTime != -1.0 && (double) TimeManager.TotalElapsedGameTime > (double) this.StructureBrain.Data.LastPrayTime && this.StructureBrain.SoulCount < this.StructureBrain.SoulMax)
    {
      Debug.Log((object) ("ADD to souls count: " + this.StructureBrain.SoulCount.ToString()));
      ++this.StructureBrain.SoulCount;
      this.StructureBrain.Data.LastPrayTime = TimeManager.TotalElapsedGameTime + this.StructureBrain.TimeBetweenSouls;
    }
    if ((Time.frameCount + this.FrameIntervalOffset) % this.UpdateInterval != 0 || (UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null)
      return;
    if (!GameManager.overridePlayerPosition)
    {
      this._updatePos = this.playerFarming.transform.position;
      this.DistanceRadius = 1f;
    }
    else
    {
      this._updatePos = PlacementRegion.Instance.PlacementPosition;
      this.DistanceRadius = HarvestTotem.EFFECTIVE_DISTANCE;
    }
    if ((double) Vector3.Distance(this._updatePos, this.transform.position) < (double) this.DistanceRadius)
    {
      this.RangeSprite.gameObject.SetActive(true);
      this.RangeSprite.DOKill();
      this.RangeSprite.DOColor(StaticColors.OffWhiteColor, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.distanceChanged = true;
    }
    else
    {
      if (!this.distanceChanged)
        return;
      this.RangeSprite.DOKill();
      this.RangeSprite.DOColor(this.FadeOut, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.distanceChanged = false;
    }
  }

  public override void IndicateHighlighted(PlayerFarming playerFarming)
  {
    this.XpBar.gameObject.SetActive(true);
  }

  public override void EndIndicateHighlighted(PlayerFarming playerFarming)
  {
    this.XpBar.gameObject.SetActive(false);
  }

  public void GivePlayerSoul() => this.playerFarming.GetSoul(1);
}
