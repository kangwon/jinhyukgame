using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class EquipmentRank : JsonItem
{
    
}

[System.Serializable]
public class EquipmentPrefix : JsonItem
{
    
}

[System.Serializable]
public class Equipment : JsonItem
{
    public string type;
    public string name;
    public int price;

    public Stat statEffect;
    public EquipmentRank rank;
    public EquipmentPrefix prefix;

    public Weapon ToWeapon()
        => new Weapon() {type=type, name=name, price=price, statEffect=statEffect, rank=rank, prefix=prefix};
    public Armor ToArmor()
        => new Armor() {type=type, name=name, price=price, statEffect=statEffect, rank=rank, prefix=prefix};
    public Helmet ToHelmet()
        => new Helmet() {type=type, name=name, price=price, statEffect=statEffect, rank=rank, prefix=prefix};
    public Shoes ToShoes()
        => new Shoes() {type=type, name=name, price=price, statEffect=statEffect, rank=rank, prefix=prefix};
    public Artifact ToArtifact()
        => new Artifact() {type=type, name=name, price=price, statEffect=statEffect, rank=rank, prefix=prefix};
    
}

public class Weapon : Equipment {}
public class Armor : Equipment {}
public class Helmet : Equipment {}
public class Shoes : Equipment {}
public class Artifact : Equipment {}

[System.Serializable]
public class EquipmentSlot
{
    Weapon weapon;
    Armor armor;
    Helmet helmet;
    Shoes shoes;
    List<Artifact> artifacts = new List<Artifact>();

    public void SetEquipment(Equipment equip)
    {
        switch (equip)
        {
            case Weapon w:
                this.weapon = w;
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
            case Artifact a:
                this.artifacts.Add(a);
                while (this.artifacts.Count > 3)
                    this.artifacts.RemoveAt(0);
                break;
            default:
                throw new NotImplementedException($"Invalid equipment type: {equip.GetType().ToString()}");
        }
    }

    public Stat GetTotalStat()
    {
        Stat zeroStat = new Stat();
        Stat totalStat = new Stat();
        totalStat += weapon?.statEffect ?? zeroStat;
        totalStat += armor?.statEffect ?? zeroStat;
        totalStat += helmet?.statEffect ?? zeroStat;
        totalStat += shoes?.statEffect ?? zeroStat;
        totalStat += artifacts?.Aggregate(zeroStat, (stat, equip) => stat + equip.statEffect) ?? zeroStat;
        return totalStat;
    }
}
