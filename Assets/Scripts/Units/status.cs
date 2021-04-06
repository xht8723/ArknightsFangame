using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//enum for which direction a character is facing. take more dmg from attacks coming from back.
public enum facing
{
    left,
    right,
    up,
    down
}

[System.Serializable]
public class status
{
    public bool isMagic;
    public facing facing;
    public int hp;
    public int maxHp;
    public int def;
    public int mr;
    public int atk;
    public List<Effect> effects;//for stroring buff/debuff.

    public status(status target)
    {
        this.isMagic = target.isMagic;
        this.hp = target.hp;
        this.maxHp = target.maxHp;
        this.def = target.def;
        this.mr = target.mr;
        this.atk = target.atk;
        this.effects = target.effects;
        this.facing = target.facing;
    }

    public status(bool isMagic, int hp, int def, int mr, int atk)
    {
        this.isMagic = isMagic;
        this.hp = hp;
        this.maxHp = hp;
        this.def = def;
        this.mr = mr;
        this.atk = atk;
        this.effects = new List<Effect>(); 
    }

    //check if the target character
    public bool isBack(GameObject other)
    {
        facing facingToward = other.GetComponent<Unit>().Status.facing;
        if (facing.Equals(facingToward))
        {
            return true;
        }
        return false;
    }
}

[System.Serializable]
//buff/debuff types
public enum EffectType
{
    stun,
    fragile,
    atkboost,
    defboost,
    mrboost
}

[System.Serializable]
//class for buff/debuff
public abstract class Effect
{
    public abstract EffectType type{get;}
    public abstract int period { get; set; }
    public abstract void applyEffect(Unit unit);
    public abstract void removeEffect(Unit unit);

}