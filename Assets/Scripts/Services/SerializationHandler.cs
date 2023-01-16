using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public abstract class SerializationHandler<T> where T : new()
{
    private SerializationService m_serializationService;

    protected abstract Guid Id { get; }
    protected T Dto { get; set; }

    private string GetFilePath() => Path.Combine(Application.persistentDataPath, Id.ToString());

    protected SerializationHandler(SerializationService serializationService)
    {
        m_serializationService = serializationService;

        m_serializationService.SerializationRequested += OnSerializationRequested;
    }

    private void OnSerializationRequested() => Save();

    public void Save()
    {
        ConvertDto();
        string json = JsonConvert.SerializeObject(Dto, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
        File.WriteAllText(GetFilePath(), json);
        //File.WriteAllText(GetFilePath(), Convert.ToBase64String(Encoding.UTF8.GetBytes(json)));
    }

    protected void Read()
    {
        if (File.Exists(GetFilePath()))
        {
            string json = File.ReadAllText(GetFilePath());
            Dto = JsonConvert.DeserializeObject<T>(json);
            //Dto = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(Convert.FromBase64String(json)));
            ConvertDtoBack(Dto);
        }
    }

    protected abstract void ConvertDto();

    protected abstract void ConvertDtoBack(T dto);
}
