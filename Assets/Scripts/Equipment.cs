using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public enum Rank
{
   none = -1, common, uncommon, rare, unique, legendary,
}

[System.Serializable]
public enum Prefix
{
    none = -1 ,broken, weak, normal, strong, amazing,
}
public enum WeaponType
{
    none = -1, sword, blunt, spear, dagger, wand,
}

[System.Serializable]
public class Equipment : JsonItem
{
    public string type;
    public string name;
    public int price;
    public Stat statEffect;
    public Rank rank;
    public Prefix prefix;

    public Weapon ToWeapon(WeaponType weaponType)
        => new Weapon() {id=id,type=type, name=name, price=price, statEffect=statEffect, rank=rank, prefix=prefix,weaponType= weaponType };
    public Armor ToArmor()
        => new Armor() { id = id, type =type, name=name, price=price, statEffect=statEffect, rank=rank, prefix=prefix};
    public Helmet ToHelmet()
        => new Helmet() { id = id, type =type, name=name, price=price, statEffect=statEffect, rank=rank, prefix=prefix};
    public Shoes ToShoes()
        => new Shoes() { id = id, type =type, name=name, price=price, statEffect=statEffect, rank=rank, prefix=prefix};   

    public override string ToString() => $"{prefix.ToString()} {rank.ToString()} {name}";
}

[System.Serializable]
public class Weapon : Equipment
{
    public WeaponType weaponType;

    static Dictionary<WeaponType, Sprite> weaponImgs = new Dictionary<WeaponType, Sprite>()
    {
        { WeaponType.none , Resources.Load("Img/fist", typeof(Sprite)) as Sprite },
        { WeaponType.sword , Resources.Load("Img/sword", typeof(Sprite)) as Sprite },
        { WeaponType.blunt , Resources.Load("Img/blunt", typeof(Sprite)) as Sprite },
        { WeaponType.spear , Resources.Load("Img/spear", typeof(Sprite)) as Sprite },
        { WeaponType.dagger , Resources.Load("Img/dagger", typeof(Sprite)) as Sprite },
        { WeaponType.wand , Resources.Load("Img/wand", typeof(Sprite)) as Sprite }
    };

    public Sprite weaponImg { get => Weapon.weaponImgs[this.weaponType]; }


}
public class Armor : Equipment {}
public class Helmet : Equipment {}
public class Shoes : Equipment {}

[System.Serializable]
public class Artifact : JsonItem
{
    public string name;
    public bool isBossItem;
    public StatBuff statEffect;
}
[System.Serializable]
public class EquipmentSlot
{
    List<Weapon> weapons = new List<Weapon>();
    Armor armor;
    Helmet helmet;
    Shoes shoes;
    List<Artifact> artifacts = new List<Artifact>();
    
    public List<Weapon> GetWeaponsList()
    {
        return weapons;
    }
    public void SetWeaponsList(List<Weapon> weapons)
    {
        this.weapons = weapons;
    }
    public void SetEquipments(Helmet helmet, Armor armor, Shoes shoes)
    {
        this.helmet = helmet;
        this.armor = armor;
        this.shoes = shoes;
    }
    public void ResetWeaponsList()
    {
        weapons.Clear();
    }
    public Armor GetArmorE()
    {
        return armor;
    }
    public Helmet GetHelmetE()
    {
        return helmet;
    }
    public Shoes GetShoesE()
    {
        return shoes;
    }
    public List<Artifact> GetArtifacts()
    {
        return artifacts;
    }
    public int ArtifactCount()
    {
       return artifacts.Count();
    }
    public void ChangeAtArtifact(int index,Artifact artifact)
    {
        if (index < ArtifactCount())
        {
            artifacts.RemoveAt(index);
            artifacts.Add(artifact);
        }
      
    }
    public void RemoveAtArtifact(int index)
    {
        if (index < ArtifactCount()) artifacts.RemoveAt(index);
    }
    public void SetEquipment(Equipment equip)
    {
        switch (equip)
        {
            case Weapon w:
                this.weapons.Add(w);
                break;
            case Armor a:
                this.armor = a;
                break;
            case Helmet h:
                this.helmet = h;
                break;
            case Shoes s:
                this.shoes = s;
                break;
            default:
                throw new NotImplementedException($"Invalid equipment type: {equip.GetType().ToString()}");
        }
    }
    public void SetEquipment(Artifact artifact)
    {
        this.artifacts.Add(artifact);
    }

    public Stat GetTotalStat()
    {
        Stat zeroStat = new Stat();
        Stat totalStat = new Stat();
        totalStat += armor?.statEffect ?? zeroStat;
        totalStat += helmet?.statEffect ?? zeroStat;
        totalStat += shoes?.statEffect ?? zeroStat;
       // totalStat += artifacts?.Aggregate(zeroStat, (stat, equip) => stat + equip.statEffect) ?? zeroStat; //TODO : ���߿� ���õ� ��Ƽ��Ʈ ����(?)�� �־����
        return totalStat;
    }
}
