using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public enum Rank
{
   uncommon,common,rare,unique,legendary,
}

[System.Serializable]
public enum Prefix
{
   broken,weak,normal,strong,amazing,
}
public enum WeaponType
{
   sword,blunt,spear,dagger,wand,
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
}

[System.Serializable]
public class Weapon : Equipment
{
    public WeaponType weaponType;
}
public class Armor : Equipment {}
public class Helmet : Equipment {}
public class Shoes : Equipment {}
[System.Serializable]
public class Artifact : JsonItem
{
    public string name;
    public int price;
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
        while (this.artifacts.Count > 3)
            this.artifacts.RemoveAt(0);
    }

    public Stat GetTotalStat()
    {
        Stat zeroStat = new Stat();
        Stat totalStat = new Stat();
        totalStat += armor?.statEffect ?? zeroStat;
        totalStat += helmet?.statEffect ?? zeroStat;
        totalStat += shoes?.statEffect ?? zeroStat;
       // totalStat += artifacts?.Aggregate(zeroStat, (stat, equip) => stat + equip.statEffect) ?? zeroStat; //TODO : 나중에 관련된 아티펙트 변수(?)를 넣어놓자
        return totalStat;
    }
}
