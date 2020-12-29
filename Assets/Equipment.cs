using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[System.Serializable]
public class Equipment
{
    string type;
    string name;

    Stat effect;
    int level;
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
}
