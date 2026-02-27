// Decompiled with JetBrains decompiler
// Type: HUD_SoulsBlack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class HUD_SoulsBlack : BaseMonoBehaviour
{
  public TextMeshProUGUI Count;
  public TextMeshProUGUI Delta;
  private int _currentCount;
  private int _currentdelta;
  private float Delay;
  private float FadeDelay;
  private Color DeltaColor;
  private Color DeltaFade;

  private int CurrentCount
  {
    get => this._currentCount;
    set
    {
      this._currentCount = value;
      this.Count.text = "<sprite name=\"icon_blackSoul\"> " + this._currentCount.ToString();
    }
  }

  public int CurrentDelta
  {
    get => this._currentdelta;
    set
    {
      this._currentdelta = value;
      this.Delta.text = (this._currentdelta > 0 ? (object) "+" : (object) "").ToString() + (object) this._currentdelta;
    }
  }

  private void OnEnable()
  {
    PlayerFarming.OnGetBlackSoul += new PlayerFarming.GetBlackSoulAction(this.OnGetSoul);
    this.CurrentCount = Inventory.BlackSouls;
    this.Delta.text = "";
    this.DeltaColor = this.Delta.color;
    this.DeltaFade = this.Delta.color - new Color(0.0f, 0.0f, 0.0f, 1f);
  }

  private void OnResetBlackSouls(int DeltaValue)
  {
    this.CurrentDelta = 0;
    this.CurrentCount = Inventory.BlackSouls;
    this.Delta.text = "";
  }

  private void OnDisable()
  {
    PlayerFarming.OnGetBlackSoul -= new PlayerFarming.GetBlackSoulAction(this.OnGetSoul);
  }

  private void OnGetSoul(int DeltaValue)
  {
    this.CurrentDelta += DeltaValue;
    this.FadeDelay = 1f;
    this.Delta.color = this.DeltaColor;
    this.Delta.text = (this.CurrentDelta > 0 ? (object) "+" : (object) "").ToString() + (object) this.CurrentDelta;
    this.Delay = 1f;
  }

  private void Update()
  {
    if (this.CurrentDelta != 0)
    {
      if ((double) (this.Delay -= Time.deltaTime) >= 0.0)
        return;
      if (this.CurrentDelta > 0)
      {
        --this.CurrentDelta;
        ++this.CurrentCount;
      }
      else if (this.CurrentDelta < 0)
      {
        ++this.CurrentDelta;
        --this.CurrentCount;
      }
      this.Delay = 0.05f;
    }
    else
    {
      if ((double) (this.FadeDelay -= Time.deltaTime) >= 0.0)
        return;
      this.Delta.color = Color.Lerp(this.Delta.color, this.DeltaFade, 2f * Time.deltaTime);
    }
  }
}
