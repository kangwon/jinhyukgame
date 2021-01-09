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

    public List<T> itemList;
    Dictionary<string, T> itemDict = new Dictionary<string, T>();
    public JsonCollection(string resourcePath) 
    {        
        TextAsset jsonFile = Resources.Load<TextAsset>(resourcePath);
        itemList = JsonUtility.FromJson<RawData>(jsonFile.text).items;
        foreach (T item in itemList)
            itemDict.Add(item.id, item);
    }

    public T GetItem(string id) => itemDict[id];
}

class JsonDB
{
    private JsonCollection<Equipment> equipmentCollection;
    private JsonCollection<StatBuff> buffCollection;
    private JsonCollection<Monster> monsterCollection;
    private JsonCollection<Weapon> weaponCollection;
    private static readonly JsonDB instance = new JsonDB();  
    static JsonDB() {}  
    private JsonDB() 
    {        
        equipmentCollection = new JsonCollection<Equipment>("equipment");
        buffCollection = new JsonCollection<StatBuff>("buff");
        monsterCollection = new JsonCollection<Monster>("monster");
        weaponCollection = new JsonCollection<Weapon>("weapon");
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
    public static Weapon GetWeapon(string id)
        => Instance.weaponCollection.GetItem(id);        

    public static StatBuff GetBuff(string id)
        => Instance.buffCollection.GetItem(id);
    public static List<StatBuff> GetBuffs()
        => Instance.buffCollection.itemList.FindAll(b => b.IsBuff());
    public static List<StatBuff> GetDebuffs()
        => Instance.buffCollection.itemList.FindAll(b => b.IsDebuff());
    
    public static Monster GetMonster(string id)
        => Instance.monsterCollection.GetItem(id);
    public static List<Monster> GetWorldMonsters(int worldNumber)
        => Instance.monsterCollection.itemList.FindAll(m => m.worldNumber == worldNumber);
}
