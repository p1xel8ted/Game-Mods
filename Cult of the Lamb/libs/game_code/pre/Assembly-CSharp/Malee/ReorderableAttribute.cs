// Decompiled with JetBrains decompiler
// Type: Malee.ReorderableAttribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Malee;

public class ReorderableAttribute : PropertyAttribute
{
  public bool add;
  public bool remove;
  public bool draggable;
  public bool singleLine;
  public bool paginate;
  public bool sortable;
  public int pageSize;
  public string elementNameProperty;
  public string elementNameOverride;
  public string elementIconPath;
  public System.Type surrogateType;
  public string surrogateProperty;

  public ReorderableAttribute()
    : this((string) null)
  {
  }

  public ReorderableAttribute(string elementNameProperty)
    : this(true, true, true, elementNameProperty, (string) null, (string) null)
  {
  }

  public ReorderableAttribute(string elementNameProperty, string elementIconPath)
    : this(true, true, true, elementNameProperty, (string) null, elementIconPath)
  {
  }

  public ReorderableAttribute(
    string elementNameProperty,
    string elementNameOverride,
    string elementIconPath)
    : this(true, true, true, elementNameProperty, elementNameOverride, elementIconPath)
  {
  }

  public ReorderableAttribute(
    bool add,
    bool remove,
    bool draggable,
    string elementNameProperty = null,
    string elementIconPath = null)
    : this(add, remove, draggable, elementNameProperty, (string) null, elementIconPath)
  {
  }

  public ReorderableAttribute(
    bool add,
    bool remove,
    bool draggable,
    string elementNameProperty = null,
    string elementNameOverride = null,
    string elementIconPath = null)
  {
    this.add = add;
    this.remove = remove;
    this.draggable = draggable;
    this.elementNameProperty = elementNameProperty;
    this.elementNameOverride = elementNameOverride;
    this.elementIconPath = elementIconPath;
    this.sortable = true;
  }
}
