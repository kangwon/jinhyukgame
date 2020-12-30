using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class JsonItem
{
    public string id;
}

class JsonCollection<T> where T : JsonItem
{  
    [System.Serializable]
    class RawData { public List<T> items; }

    Dictionary<string, T> itemDict = new Dictionary<string, T>();
    public JsonCollection(string resourcePath) 
    {        
        TextAsset jsonFile = Resources.Load<TextAsset>(resourcePath);
        List<T> itemList = JsonUtility.FromJson<RawData>(jsonFile.text).items;
        foreach (T item in itemList)
            itemDict.Add(item.id, item);
    }

    public T GetItem(string id) => itemDict[id];
}

class JsonDB
{
    private JsonCollection<Equipment> equipmentCollection;

    private static readonly JsonDB instance = new JsonDB();  
    static JsonDB() {}  
    private JsonDB() 
    {        
        equipmentCollection = new JsonCollection<Equipment>("equipment");
    }
    public static JsonDB Instance  
    {  
        get => instance;
    }

    public static Equipment GetEquipment(string id)
    {
        Equipment equip = Instance.equipmentCollection.GetItem(id);
        switch (equip.type)
        {
            case "weapon":
                return equip.ToWeapon();
            case "armor":
                return equip.ToArmor();
            case "helmet":
                return equip.ToHelmet();
            case "shoes":
                return equip.ToShoes();
            case "artifact":
                return equip.ToArtifact();
            default:
                throw new NotImplementedException($"Invalid equipment type: {equip.type}");
        }
    }
}
