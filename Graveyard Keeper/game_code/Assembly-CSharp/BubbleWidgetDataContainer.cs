// Decompiled with JetBrains decompiler
// Type: BubbleWidgetDataContainer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System;
using System.Collections.Generic;

#nullable disable
public class BubbleWidgetDataContainer
{
  public WidgetsBubbleGUI.Alignment alignment;
  public List<BubbleWidgetData> data_list = new List<BubbleWidgetData>();
  public WorldGameObject _linked_wgo;
  public InteractionBubbleGUI _bubble_gui;

  public WorldGameObject linked_wgo => this._linked_wgo;

  public BubbleWidgetDataContainer(WorldGameObject linked_wgo) => this._linked_wgo = linked_wgo;

  public BubbleWidgetDataContainer GetCopy()
  {
    BubbleWidgetDataContainer copy = new BubbleWidgetDataContainer(WidgetsBubbleGUI.Alignment.Center, Array.Empty<BubbleWidgetData>());
    copy.alignment = this.alignment;
    copy._linked_wgo = this._linked_wgo;
    copy._bubble_gui = this._bubble_gui;
    copy.data_list.AddRange((IEnumerable<BubbleWidgetData>) this.data_list);
    return copy;
  }

  public bool has_data => this.data_list.Count > 0;

  public BubbleWidgetDataContainer(
    WidgetsBubbleGUI.Alignment alignment = WidgetsBubbleGUI.Alignment.Center,
    params BubbleWidgetData[] data_params)
  {
    this.alignment = alignment;
    this.data_list = ((IEnumerable<BubbleWidgetData>) data_params).ToList<BubbleWidgetData>();
  }

  public void SetData(params BubbleWidgetData[] data_params)
  {
    this.data_list = ((IEnumerable<BubbleWidgetData>) data_params).ToList<BubbleWidgetData>();
  }

  public void SetData(List<BubbleWidgetData> data_list)
  {
    if (data_list == null)
      this.ClearData();
    else
      this.data_list = data_list;
  }

  public T GetData<T>() where T : BubbleWidgetData
  {
    System.Type type = typeof (T);
    foreach (BubbleWidgetData data in this.data_list)
    {
      if (System.Type.op_Equality(data.GetType(), type))
        return data as T;
    }
    return default (T);
  }

  public void RemoveData<T>() where T : BubbleWidgetData
  {
    System.Type type = typeof (T);
    for (int index = 0; index < this.data_list.Count; ++index)
    {
      if (!System.Type.op_Inequality(this.data_list[index].GetType(), type))
      {
        this.data_list.RemoveAt(index);
        --index;
      }
    }
  }

  public void AddData(params BubbleWidgetData[] data_params)
  {
    foreach (BubbleWidgetData dataParam in data_params)
    {
      if (dataParam != null)
        this.data_list.Add(dataParam);
    }
  }

  public void AddInteractionHint(string hint)
  {
    if (string.IsNullOrEmpty(hint))
      return;
    this.data_list.Add((BubbleWidgetData) new BubbleWidgetTextData(hint, UITextStyles.TextStyle.InteractionHint));
  }

  public void ClearData() => this.data_list.Clear();

  public void SetWGOQualityData(BubbleWidgetTextData data)
  {
    this.RemoveWGOQualityData();
    this.data_list.Add((BubbleWidgetData) data);
  }

  public bool RemoveWGOQualityData() => this.RemoveDataWithID(BubbleWidgetData.WidgetID.Quality);

  public bool RemoveDataWithID(BubbleWidgetData.WidgetID widget_id)
  {
    if (this.data_list.Count > 0)
    {
      int index = this.data_list.Count - 1;
      if (this.data_list[index] is BubbleWidgetTextData data && data.widget_id == widget_id)
      {
        this.data_list.RemoveAt(index);
        return true;
      }
    }
    return false;
  }

  public void Redraw()
  {
    if (this.has_data)
    {
      if ((UnityEngine.Object) this._bubble_gui == (UnityEngine.Object) null)
        this._bubble_gui = InteractionBubbleGUI.Show(this._linked_wgo, this);
      else
        this._bubble_gui.Redraw(true);
    }
    else if ((UnityEngine.Object) this._bubble_gui != (UnityEngine.Object) null)
    {
      this._bubble_gui.DestroyMe();
      this._bubble_gui = (InteractionBubbleGUI) null;
    }
    if (!MainGame.game_started || !((UnityEngine.Object) this._bubble_gui == (UnityEngine.Object) null) || !((UnityEngine.Object) MainGame.me.player.components.character.wgo_hilighted_for_work == (UnityEngine.Object) this._linked_wgo))
      return;
    MainGame.me.player.components.character.wgo_hilighted_for_work = (WorldGameObject) null;
  }

  public InteractionBubbleGUI GetBubbleGUI() => this._bubble_gui;

  public bool DoesContainWidgetDataWithID(BubbleWidgetData.WidgetID widget_id)
  {
    foreach (BubbleWidgetData data in this.data_list)
    {
      if (data.widget_id == widget_id)
        return true;
    }
    return false;
  }

  public void SetWidgetDataWithID(BubbleWidgetData wdata, BubbleWidgetData.WidgetID widget_id)
  {
    int num1 = -1;
    for (int index = 0; index < this.data_list.Count; ++index)
    {
      if (this.data_list[index].widget_id == widget_id)
      {
        num1 = index;
        if (wdata == null)
        {
          this.data_list.RemoveAt(index);
          break;
        }
        wdata.widget_id = widget_id;
        this.data_list[index] = wdata;
        break;
      }
    }
    if (num1 != -1 || wdata == null)
      return;
    wdata.widget_id = widget_id;
    this.AddData(wdata);
    int num2 = this.data_list.Count - 1;
  }
}
