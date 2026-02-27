// Decompiled with JetBrains decompiler
// Type: UIShrineTutorial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class UIShrineTutorial : BaseMonoBehaviour
{
  public TextMeshProUGUI Text;
  private Canvas canvas;
  public Vector3 Offset = new Vector3(0.0f, 0.0f, -2f);
  private float Delay;
  public AnimationCurve bounceCurve;
  private Interaction_PlacementRegion structure;
  private Vector2 Scale;
  private Vector2 ScaleSpeed;
  private int Stone;
  private int Logs;
  private int CurrentStone;
  private int CurrentLog;
  private string sStone;
  private string sLog;
  private bool Activated;

  public void UpdateText(string String, Vector3 Position)
  {
    this.Text.text = String;
    this.transform.position = Camera.main.WorldToScreenPoint(Position + (this.Offset + new Vector3(0.0f, 0.0f, 0.25f * this.bounceCurve.Evaluate((float) ((double) Time.time * 0.5 % 1.0))) * this.canvas.scaleFactor));
  }

  private void OnEnable()
  {
    this.Scale = this.ScaleSpeed = Vector2.zero;
    this.transform.localScale = (Vector3) this.Scale;
    this.canvas = this.GetComponentInParent<Canvas>();
    this.Delay = 0.3f;
    this.structure = Object.FindObjectOfType<Interaction_PlacementRegion>();
    foreach (StructuresData.ItemCost itemCost in StructuresData.GetCost(StructureBrain.TYPES.COOKING_FIRE))
    {
      if (itemCost.CostItem == InventoryItem.ITEM_TYPE.LOG)
        this.Logs = itemCost.CostValue;
      if (itemCost.CostItem == InventoryItem.ITEM_TYPE.STONE)
        this.Stone = itemCost.CostValue;
    }
    StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.Close);
    this.transform.SetAsFirstSibling();
  }

  private void OnDisable()
  {
    StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.Close);
  }

  private void Close(StructuresData structure)
  {
    Debug.Log((object) $"{(object) structure.Type}  {(object) structure.ToBuildType}");
    if (structure.Type != StructureBrain.TYPES.BUILD_SITE || structure.ToBuildType != StructureBrain.TYPES.COOKING_FIRE)
      return;
    Object.Destroy((Object) this.gameObject);
  }

  private void LateUpdate()
  {
    if ((Object) this.structure == (Object) null)
    {
      Object.Destroy((Object) this.gameObject);
    }
    else
    {
      this.CurrentStone = Inventory.GetItemQuantity(2);
      this.sStone = $"<sprite name=\"icon_stone\"> {(this.CurrentStone < this.Stone ? (object) "<color=red>" : (object) "")}{(object) Inventory.GetItemQuantity(2)} {(this.CurrentStone < this.Stone ? (object) "</color>" : (object) "")}/ {this.Stone.ToString()}";
      this.CurrentLog = Inventory.GetItemQuantity(1);
      this.sLog = $"<sprite name=\"icon_wood\"> {(this.CurrentLog < this.Logs ? (object) "<color=red>" : (object) "")}{(object) Inventory.GetItemQuantity(1)} {(this.CurrentLog < this.Logs ? (object) "</color>" : (object) "")}/ {this.Logs.ToString()}";
      this.UpdateText($"{this.sLog}\n {this.sStone}", this.structure.transform.position);
      if (!this.Activated)
      {
        if (!((Object) PlayerFarming.Instance != (Object) null) || (double) Vector3.Distance(this.structure.transform.position, PlayerFarming.Instance.transform.position) >= 8.0)
          return;
        this.Activated = true;
      }
      else if ((double) Time.timeScale <= 0.0 || LetterBox.IsPlaying || HUDManager.isHiding)
      {
        this.Scale = this.ScaleSpeed = Vector2.zero;
        this.transform.localScale = (Vector3) this.Scale;
        this.Delay = 0.5f;
      }
      else
      {
        double num = (double) (this.Delay -= Time.deltaTime);
      }
    }
  }

  private void FixedUpdate()
  {
    if ((double) this.Delay > 0.0)
      return;
    this.ScaleSpeed.x += (float) ((1.0 - (double) this.Scale.x) * 0.5);
    this.Scale.x += (this.ScaleSpeed.x *= 0.6f);
    this.ScaleSpeed.y += (float) ((1.0 - (double) this.Scale.y) * 0.40000000596046448);
    this.Scale.y += (this.ScaleSpeed.y *= 0.5f);
    this.transform.localScale = (Vector3) this.Scale;
  }
}
