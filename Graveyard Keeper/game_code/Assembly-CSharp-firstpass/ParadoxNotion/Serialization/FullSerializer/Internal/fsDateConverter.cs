// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsDateConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Globalization;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public class fsDateConverter : fsConverter
{
  public const string DefaultDateTimeFormatString = "o";
  public const string DateTimeOffsetFormatString = "o";

  public string DateTimeFormatString => this.Serializer.Config.CustomDateTimeFormatString ?? "o";

  public override bool CanProcess(Type type)
  {
    return Type.op_Equality(type, typeof (DateTime)) || Type.op_Equality(type, typeof (DateTimeOffset)) || Type.op_Equality(type, typeof (TimeSpan));
  }

  public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
  {
    switch (instance)
    {
      case DateTime dateTime:
        serialized = new fsData(dateTime.ToString(this.DateTimeFormatString));
        return fsResult.Success;
      case DateTimeOffset dateTimeOffset:
        serialized = new fsData(dateTimeOffset.ToString("o"));
        return fsResult.Success;
      case TimeSpan timeSpan:
        serialized = new fsData(timeSpan.ToString());
        return fsResult.Success;
      default:
        throw new InvalidOperationException("FullSerializer Internal Error -- Unexpected serialization type");
    }
  }

  public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
  {
    if (!data.IsString)
      return fsResult.Fail("Date deserialization requires a string, not " + data.Type.ToString());
    if (Type.op_Equality(storageType, typeof (DateTime)))
    {
      DateTime result;
      if (DateTime.TryParse(data.AsString, (IFormatProvider) null, DateTimeStyles.RoundtripKind, out result))
      {
        instance = (object) result;
        return fsResult.Success;
      }
      if (!fsGlobalConfig.AllowInternalExceptions)
        return fsResult.Fail($"Unable to parse {data.AsString} into a DateTime");
      try
      {
        instance = (object) Convert.ToDateTime(data.AsString);
        return fsResult.Success;
      }
      catch (Exception ex)
      {
        return fsResult.Fail($"Unable to parse {data.AsString} into a DateTime; got exception {ex?.ToString()}");
      }
    }
    else
    {
      if (Type.op_Equality(storageType, typeof (DateTimeOffset)))
      {
        DateTimeOffset result;
        if (!DateTimeOffset.TryParse(data.AsString, (IFormatProvider) null, DateTimeStyles.RoundtripKind, out result))
          return fsResult.Fail($"Unable to parse {data.AsString} into a DateTimeOffset");
        instance = (object) result;
        return fsResult.Success;
      }
      if (!Type.op_Equality(storageType, typeof (TimeSpan)))
        throw new InvalidOperationException("FullSerializer Internal Error -- Unexpected deserialization type");
      TimeSpan result1;
      if (!TimeSpan.TryParse(data.AsString, out result1))
        return fsResult.Fail($"Unable to parse {data.AsString} into a TimeSpan");
      instance = (object) result1;
      return fsResult.Success;
    }
  }
}
