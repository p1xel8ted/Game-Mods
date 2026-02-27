// Decompiled with JetBrains decompiler
// Type: BaseMonoBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Reflection;
using UnityEngine;

#nullable disable
public class BaseMonoBehaviour : MonoBehaviour
{
  private void OnDestroy()
  {
    foreach (FieldInfo field in ((object) this).GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    {
      System.Type fieldType = field.FieldType;
      if (typeof (IList).IsAssignableFrom(fieldType) && field.GetValue((object) this) is IList list)
        list.Clear();
      if (typeof (IDictionary).IsAssignableFrom(fieldType) && field.GetValue((object) this) is IDictionary dictionary)
        dictionary.Clear();
      if (!fieldType.IsPrimitive)
        field.SetValue((object) this, (object) null);
    }
  }
}
