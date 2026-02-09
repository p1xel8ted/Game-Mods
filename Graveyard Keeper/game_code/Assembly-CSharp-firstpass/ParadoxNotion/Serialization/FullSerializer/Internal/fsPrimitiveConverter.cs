// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsPrimitiveConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public class fsPrimitiveConverter : fsConverter
{
  public override bool CanProcess(Type type)
  {
    return type.Resolve().IsPrimitive || Type.op_Equality(type, typeof (string)) || Type.op_Equality(type, typeof (Decimal));
  }

  public override bool RequestCycleSupport(Type storageType) => false;

  public override bool RequestInheritanceSupport(Type storageType) => false;

  public static bool UseBool(Type type) => Type.op_Equality(type, typeof (bool));

  public static bool UseInt64(Type type)
  {
    return Type.op_Equality(type, typeof (sbyte)) || Type.op_Equality(type, typeof (byte)) || Type.op_Equality(type, typeof (short)) || Type.op_Equality(type, typeof (ushort)) || Type.op_Equality(type, typeof (int)) || Type.op_Equality(type, typeof (uint)) || Type.op_Equality(type, typeof (long)) || Type.op_Equality(type, typeof (ulong));
  }

  public static bool UseDouble(Type type)
  {
    return Type.op_Equality(type, typeof (float)) || Type.op_Equality(type, typeof (double)) || Type.op_Equality(type, typeof (Decimal));
  }

  public static bool UseString(Type type)
  {
    return Type.op_Equality(type, typeof (string)) || Type.op_Equality(type, typeof (char));
  }

  public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
  {
    Type type = instance.GetType();
    if (this.Serializer.Config.Serialize64BitIntegerAsString && (Type.op_Equality(type, typeof (long)) || Type.op_Equality(type, typeof (ulong))))
    {
      serialized = new fsData((string) Convert.ChangeType(instance, typeof (string)));
      return fsResult.Success;
    }
    if (fsPrimitiveConverter.UseBool(type))
    {
      serialized = new fsData((bool) instance);
      return fsResult.Success;
    }
    if (fsPrimitiveConverter.UseInt64(type))
    {
      serialized = new fsData((long) Convert.ChangeType(instance, typeof (long)));
      return fsResult.Success;
    }
    if (fsPrimitiveConverter.UseDouble(type))
    {
      if (Type.op_Equality(instance.GetType(), typeof (float)) && (double) (float) instance != -3.4028234663852886E+38 && (double) (float) instance != 3.4028234663852886E+38 && !float.IsInfinity((float) instance) && !float.IsNaN((float) instance))
      {
        serialized = new fsData((double) (Decimal) (float) instance);
        return fsResult.Success;
      }
      serialized = new fsData((double) Convert.ChangeType(instance, typeof (double)));
      return fsResult.Success;
    }
    if (fsPrimitiveConverter.UseString(type))
    {
      serialized = new fsData((string) Convert.ChangeType(instance, typeof (string)));
      return fsResult.Success;
    }
    serialized = (fsData) null;
    return fsResult.Fail("Unhandled primitive type " + instance.GetType()?.ToString());
  }

  public override fsResult TryDeserialize(fsData storage, ref object instance, Type storageType)
  {
    fsResult success = fsResult.Success;
    if (fsPrimitiveConverter.UseBool(storageType))
    {
      fsResult fsResult;
      if ((fsResult = success + this.CheckType(storage, fsDataType.Boolean)).Succeeded)
        instance = (object) storage.AsBool;
      return fsResult;
    }
    if (fsPrimitiveConverter.UseDouble(storageType) || fsPrimitiveConverter.UseInt64(storageType))
    {
      if (storage.IsDouble)
        instance = !Type.op_Equality(storageType, typeof (float)) ? Convert.ChangeType((object) storage.AsDouble, storageType) : (object) (float) storage.AsDouble;
      else if (storage.IsInt64)
        instance = !Type.op_Equality(storageType, typeof (int)) ? Convert.ChangeType((object) storage.AsInt64, storageType) : (object) (int) storage.AsInt64;
      else if (this.Serializer.Config.Serialize64BitIntegerAsString && storage.IsString && (Type.op_Equality(storageType, typeof (long)) || Type.op_Equality(storageType, typeof (ulong))))
        instance = Convert.ChangeType((object) storage.AsString, storageType);
      else
        return fsResult.Fail($"{this.GetType().Name} expected number but got {storage.Type.ToString()} in {storage?.ToString()}");
      return fsResult.Success;
    }
    if (!fsPrimitiveConverter.UseString(storageType))
      return fsResult.Fail($"{this.GetType().Name}: Bad data; expected bool, number, string, but got {storage?.ToString()}");
    fsResult fsResult1;
    if ((fsResult1 = success + this.CheckType(storage, fsDataType.String)).Succeeded)
      instance = !Type.op_Equality(storageType, typeof (char)) ? (object) storage.AsString : (object) storage.AsString[0];
    return fsResult1;
  }
}
