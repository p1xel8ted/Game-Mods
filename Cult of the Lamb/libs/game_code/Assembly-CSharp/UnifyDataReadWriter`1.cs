// Decompiled with JetBrains decompiler
// Type: UnifyDataReadWriter`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Data.Serialization;
using MessagePack;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using Unify;
using UnityEngine;

#nullable disable
public class UnifyDataReadWriter<T> : MMDataReadWriterBase<T>
{
  public Thread _thread;
  public static int count;
  public int BufferSize;

  static UnifyDataReadWriter() => Debug.Log((object) "UnifyDataReadWrtier: Static constructor");

  public UnifyDataReadWriter()
  {
    ++UnifyDataReadWriter<T>.count;
    Debug.Log((object) ("UnifyDataReadWriter: Constructor, count: " + UnifyDataReadWriter<T>.count.ToString()));
  }

  void object.Finalize()
  {
    try
    {
      --UnifyDataReadWriter<T>.count;
      Debug.Log((object) ("UnifyDataReadWriter: Destructor, count: " + UnifyDataReadWriter<T>.count.ToString()));
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public override void Write(T data, string filename, bool encrypt, bool backup)
  {
    Debug.Log((object) ("UnifyDataReadWriter - Write File " + filename).Colour(Color.yellow));
    if (!this.SavesAsJson(filename))
      this.WriteMessagePack(data, filename, encrypt, backup);
    else
      this.WriteJson(data, filename, encrypt, backup);
  }

  public void ResetSharedStream(MemoryStream stream)
  {
    stream.SetLength(0L);
    stream.Seek(0L, SeekOrigin.Begin);
  }

  public void SerializeJson(MemoryStream outputStream, T data)
  {
    using (StreamWriter streamWriter = new StreamWriter((Stream) InitUnifyGlobal._serializedMemoryStream, Encoding.UTF8, 1024 /*0x0400*/, true))
    {
      using (JsonTextWriter jsonTextWriter = new JsonTextWriter((TextWriter) streamWriter))
        MMSerialization.JsonSerializer.Serialize((JsonWriter) jsonTextWriter, (object) data);
    }
  }

  public void WriteJson(T data, string filename, bool encrypt, bool backup)
  {
    try
    {
      lock (InitUnifyGlobal._serializedMemoryStream)
      {
        this.ResetSharedStream(InitUnifyGlobal._serializedMemoryStream);
        this.SerializeJson(InitUnifyGlobal._outputMemoryStream, data);
        this._thread = new Thread((ThreadStart) (() =>
        {
          lock (InitUnifyGlobal._serializedMemoryStream)
          {
            UnifyDataReadWriter<T>.ZipJson(InitUnifyGlobal._outputMemoryStream, InitUnifyGlobal._serializedMemoryStream);
            SaveData.PutBytes(filename, InitUnifyGlobal._outputMemoryStream.GetBuffer(), (int) InitUnifyGlobal._outputMemoryStream.Length);
            UnifyComponent.Instance.MainThreadEnqueue((System.Action) (() =>
            {
              System.Action onWriteCompleted = this.OnWriteCompleted;
              if (onWriteCompleted == null)
                return;
              onWriteCompleted();
            }));
          }
        }));
        this._thread.Name = "Json Zip";
        this._thread.Start();
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex.Message.Colour(Color.red));
      Action<MMReadWriteError> onWriteError = this.OnWriteError;
      if (onWriteError == null)
        return;
      onWriteError(new MMReadWriteError(ex.Message));
    }
  }

  public void WriteMessagePack(T data, string filename, bool encrypt, bool backup)
  {
    string msgpackFilename = filename + "MP";
    try
    {
      this._thread = new Thread((ThreadStart) (() =>
      {
        Debug.Log((object) ("UnifyDataReadWriter - SaveMessagePack - Starting for " + msgpackFilename).Colour(Color.cyan));
        lock (InitUnifyGlobal._serializedMemoryStream)
        {
          this.ResetSharedStream(InitUnifyGlobal._serializedMemoryStream);
          InitUnifyGlobal._serializedMemoryStream.WriteByte((byte) 77);
          InitUnifyGlobal._serializedMemoryStream.WriteByte((byte) 80 /*0x50*/);
          MessagePackSerializer.Serialize<T>((Stream) InitUnifyGlobal._serializedMemoryStream, data, MPSerialization.options);
          SaveData.PutBytes(msgpackFilename, InitUnifyGlobal._serializedMemoryStream.GetBuffer(), (int) InitUnifyGlobal._serializedMemoryStream.Length);
          UnifyComponent.Instance.MainThreadEnqueue((System.Action) (() =>
          {
            System.Action onWriteCompleted = this.OnWriteCompleted;
            if (onWriteCompleted == null)
              return;
            onWriteCompleted();
          }));
        }
      }));
      this._thread.Name = "MessagePack Serialization";
      this._thread.Start();
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex.Message.Colour(Color.red));
      Action<MMReadWriteError> onWriteError = this.OnWriteError;
      if (onWriteError != null)
        onWriteError(new MMReadWriteError(ex.Message));
      throw;
    }
  }

  public override void Read(string filename)
  {
    Debug.Log((object) ("UnifyDataReadWriter - Read File " + filename).Colour(Color.yellow));
    bool flag1 = false;
    bool flag2 = false;
    try
    {
      if (!this.SavesAsJson(filename))
      {
        if (SaveData.Exists(filename + "MP"))
        {
          try
          {
            Debug.Log((object) "UnifyDataReadWriter - Read File - Found MP file, attempting to load...".Colour(Color.yellow));
            this.DoLoad(filename + "MP");
            Debug.Log((object) "UnifyDataReadWriter - Read File - Successfully loaded MP file!".Colour(Color.yellow));
            return;
          }
          catch (Exception ex)
          {
            Debug.Log((object) $"UnifyDataReadWriter - Failed to load file: {ex.Message}.".Colour(Color.red));
            Unify.PlayerPrefs.SetInt("MarkedForDelete" + filename, 0);
            flag1 = true;
          }
        }
      }
      if (SaveData.Exists(filename))
      {
        try
        {
          Debug.Log((object) "UnifyDataReadWriter - Read File - Found file, attempting to load...".Colour(Color.yellow));
          this.DoLoad(filename);
          Debug.Log((object) "UnifyDataReadWriter - Read File - Successfully loaded file!".Colour(Color.yellow));
        }
        catch (Exception ex)
        {
          Debug.Log((object) $"UnifyDataReadWriter - Failed to load file: {ex.Message}.".Colour(Color.red));
          throw;
        }
      }
      else
      {
        bool flag3 = true;
        if (flag1 && flag3 | flag2)
        {
          Action<MMReadWriteError> onReadError = this.OnReadError;
          if (onReadError == null)
            return;
          onReadError(new MMReadWriteError("MP FailedLoad & No Json or json Failed Load"));
        }
        else
        {
          Debug.Log((object) "UnifyDataReadWriter - Read File - File did not exist, creating new.".Colour(Color.yellow));
          System.Action onCreateDefault = this.OnCreateDefault;
          if (onCreateDefault == null)
            return;
          onCreateDefault();
        }
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ("UnifyDataReadWriter - Unhandled exception: " + ex.Message).Colour(Color.red));
      Action<MMReadWriteError> onReadError = this.OnReadError;
      if (onReadError != null)
        onReadError(new MMReadWriteError(ex.Message));
      throw;
    }
  }

  public void DoLoad(string filename)
  {
    byte[] bytes = SaveData.GetBytes(filename);
    char ch1 = Convert.ToChar(bytes[0]);
    char ch2 = Convert.ToChar(bytes[1]);
    T obj;
    if (ch1 == 'Z' && ch2 == 'P' || ch1 == 'Z' && ch2 == 'B')
    {
      using (MemoryStream memoryStream = new MemoryStream(bytes))
      {
        memoryStream.Seek(2L, SeekOrigin.Begin);
        using (GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionMode.Decompress, true))
        {
          using (StreamReader reader1 = new StreamReader((Stream) gzipStream))
          {
            using (JsonTextReader reader2 = new JsonTextReader((TextReader) reader1))
              obj = MMSerialization.JsonSerializer.Deserialize<T>((JsonReader) reader2);
          }
        }
      }
    }
    else if (ch1 == 'M' && ch2 == 'P')
    {
      Debug.Log((object) "UnifyDataReadWriter - LOADING MESSAGE PACK");
      using (MemoryStream memoryStream = new MemoryStream(bytes))
      {
        memoryStream.Seek(2L, SeekOrigin.Begin);
        obj = MessagePackSerializer.Deserialize<T>((Stream) memoryStream, MPSerialization.options);
      }
    }
    else
    {
      using (MemoryStream memoryStream = new MemoryStream(bytes))
        obj = (T) new XmlSerializer(typeof (T)).Deserialize((Stream) memoryStream);
    }
    Action<T> onReadCompleted = this.OnReadCompleted;
    if (onReadCompleted == null)
      return;
    onReadCompleted(obj);
  }

  public override void Delete(string filename)
  {
    Debug.Log((object) ("UnifyDataReadWriter - Delete File " + filename).Colour(Color.yellow));
    bool flag = false;
    if (SaveData.Exists(filename))
    {
      if (filename.Contains("slot_"))
      {
        Unify.PlayerPrefs.SetInt("MarkedForDelete" + filename, 1);
        Debug.Log((object) ("UnifyDataReadWriter - Marked Deleted " + filename).Colour(Color.yellow));
      }
      else
      {
        Debug.Log((object) ("UnifyDataReadWriter - Deletion Successful " + filename).Colour(Color.yellow));
        SaveData.Delete(filename);
      }
      flag = true;
    }
    if (SaveData.Exists(filename + "MP"))
    {
      Debug.Log((object) $"UnifyDataReadWriter - Deletion Successful {filename}MP".Colour(Color.yellow));
      SaveData.Delete(filename + "MP");
      flag = true;
    }
    if (!flag)
      return;
    System.Action deletionComplete = this.OnDeletionComplete;
    if (deletionComplete == null)
      return;
    deletionComplete();
  }

  public override bool FileExists(string filename)
  {
    Debug.Log((object) $"UnifyDataReadWriter - File Exists? {filename}MP / {filename}".Colour(Color.yellow));
    if (this.SavesAsJson(filename))
    {
      bool flag = SaveData.Exists(filename);
      Debug.Log((object) $"UnifyDataReadWriter - {filename} File {(flag ? "Exists" : "does not Exist")}".Colour(flag ? Color.green : Color.yellow));
      return flag;
    }
    if (SaveData.Exists(filename + "MP"))
    {
      Debug.Log((object) $"UnifyDataReadWriter - {filename}MP File Exists".Colour(Color.green));
      return true;
    }
    Debug.Log((object) $"UnifyDataReadWriter - {filename}MP File does not Exist".Colour(Color.yellow));
    if (SaveData.Exists(filename))
    {
      Debug.Log((object) $"UnifyDataReadWriter - {filename} File Exists".Colour(Color.green));
      if (Unify.PlayerPrefs.GetInt("MarkedForDelete" + filename, 0) != 1)
        return true;
      Debug.Log((object) $"UnifyDataReadWriter - {filename} Marked Delete".Colour(Color.green));
      return false;
    }
    Debug.Log((object) $"UnifyDataReadWriter - {filename} File does not Exist".Colour(Color.yellow));
    return false;
  }

  public static void ZipJson(MemoryStream mso, MemoryStream data)
  {
    if (mso.Length != 0L)
    {
      mso.SetLength(0L);
      mso.Seek(0L, SeekOrigin.Begin);
    }
    mso.WriteByte((byte) 90);
    mso.WriteByte((byte) 66);
    using (GZipStream destination = new GZipStream((Stream) mso, CompressionMode.Compress))
    {
      data.Seek(0L, SeekOrigin.Begin);
      data.CopyTo((Stream) destination);
    }
  }

  public void WriteScreenshotFile(T data, string filename)
  {
    Debug.Log((object) ("UnifyDataReadWriter - Write File " + filename).Colour(Color.yellow));
    if (this._thread != null && this._thread.IsAlive)
    {
      Debug.Log((object) "UnifyDataReadWriter - Write thread is already running!".Colour(Color.red));
    }
    else
    {
      try
      {
        lock (InitUnifyGlobal._serializedMemoryStream)
        {
          this._thread = new Thread((ThreadStart) (() =>
          {
            lock (InitUnifyGlobal._serializedMemoryStream)
            {
              InitUnifyGlobal._serializedMemoryStream.SetLength(0L);
              InitUnifyGlobal._serializedMemoryStream.Seek(0L, SeekOrigin.Begin);
              float delayBetweenSaves = SessionManager.instance.delayBetweenSaves;
              SessionManager.instance.delayBetweenSaves = -1f;
              new BinaryFormatter().Serialize((Stream) InitUnifyGlobal._serializedMemoryStream, (object) data);
              SaveData.PutBytes(filename, InitUnifyGlobal._serializedMemoryStream.GetBuffer(), (int) InitUnifyGlobal._serializedMemoryStream.Length);
              this.BufferSize = (int) InitUnifyGlobal._serializedMemoryStream.Length;
              SessionManager.instance.delayBetweenSaves = delayBetweenSaves;
              UnifyComponent.Instance.MainThreadEnqueue((System.Action) (() =>
              {
                System.Action onWriteCompleted = this.OnWriteCompleted;
                if (onWriteCompleted == null)
                  return;
                onWriteCompleted();
              }));
            }
          }));
          this._thread.Name = "ScreenShot Save";
          this._thread.Start();
        }
      }
      catch (Exception ex)
      {
        Debug.Log((object) ex.Message.Colour(Color.red));
        Action<MMReadWriteError> onWriteError = this.OnWriteError;
        if (onWriteError != null)
          onWriteError(new MMReadWriteError(ex.Message));
        throw;
      }
    }
  }

  public void ReadScreenshotFile(string filename)
  {
    Debug.Log((object) ("UnifyDataReadWriter - Read File " + filename).Colour(Color.yellow));
    try
    {
      if (this.FileExists(filename))
      {
        Debug.Log((object) "UnifyDataReadWriter - Read File - Successfully read file!".Colour(Color.yellow));
        SaveData.GetBytes(filename);
        lock (InitUnifyGlobal._serializedMemoryStream)
        {
          InitUnifyGlobal._serializedMemoryStream.Seek(0L, SeekOrigin.Begin);
          byte[] bytes = SaveData.GetBytes(filename);
          InitUnifyGlobal._serializedMemoryStream.Write(bytes, 0, bytes.Length);
          BinaryFormatter binaryFormatter = new BinaryFormatter();
          InitUnifyGlobal._serializedMemoryStream.Seek(0L, SeekOrigin.Begin);
          MemoryStream serializedMemoryStream = InitUnifyGlobal._serializedMemoryStream;
          T obj = (T) binaryFormatter.Deserialize((Stream) serializedMemoryStream);
          this.BufferSize = bytes.Length;
          Action<T> onReadCompleted = this.OnReadCompleted;
          if (onReadCompleted == null)
            return;
          onReadCompleted(obj);
        }
      }
      else
      {
        Debug.Log((object) "UnifyDataReadWriter - Read File - File did not exist, creating new".Colour(Color.yellow));
        Action<MMReadWriteError> onReadError = this.OnReadError;
        if (onReadError == null)
          return;
        onReadError(new MMReadWriteError("File did not exist"));
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex.Message.Colour(Color.red));
      Action<MMReadWriteError> onReadError = this.OnReadError;
      if (onReadError != null)
        onReadError(new MMReadWriteError(ex.Message));
      throw;
    }
  }
}
