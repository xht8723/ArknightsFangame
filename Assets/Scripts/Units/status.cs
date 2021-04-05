using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum for which direction a character is facing. take more dmg from attacks coming from back.
public enum facing
{
    left,
    right,
    up,
    down
}

public class status
{
    public bool isMagic;
    public facing facing;
    public int hp;
    public int def;
    public int mr;
    public int atk;
    public List<Effect> effects;//for stroring buff/debuff.

    public status(bool isMagic, int hp, int def, int mr, int atk)
    {
        this.isMagic = isMagic;
        this.hp = hp;
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

//buff/debuff types
public enum EffectType
{
    stun,
    fragile,
    atkboost,
    defboost,
    mrboost
}

//class for buff/debuff
public abstract class Effect
{
    public abstract EffectType type{get;}
    public abstract int period { get; set; }
    public abstract void applyEffect(Unit unit);
    public abstract void removeEffect(Unit unit);

}