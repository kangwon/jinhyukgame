using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[System.Serializable]
public class Equipment
{
    public string type;
    public string name;

    public Stat effect;
    public int level;
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
    List<Artifact> artifacts;

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
        totalStat += weapon?.effect ?? zeroStat;
        totalStat += armor?.effect ?? zeroStat;
        totalStat += helmet?.effect ?? zeroStat;
        totalStat += shoes?.effect ?? zeroStat;
        totalStat += artifacts?.Aggregate(zeroStat, (stat, equip) => stat + equip.effect) ?? zeroStat;
        return totalStat;
    }
}
