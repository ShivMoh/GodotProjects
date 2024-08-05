using System;
using Godot;
using System.Collections.Generic;

public enum AttackAttribute
{
    FIRE,
    WATER,
    WIND,
    LIGHTNING,
    EARTH,
    LIGHT, 

    DARK,
}

public enum AttackType
{
    MAGICAL,

    PHYSICAL
}

// for the elements, rip off naruto
// for light and dark, dark > elements, light > dark (like really effective more than usual)

public enum AttackEffect
{
    ELECTROCUTED, 
    // electrocutes a target
    // slows movement and randomly will paralyze
    POISON,
    // poisons a target
    // takes fixed damange per turn
    // chance that target may recover, if not will continue until hp is 0 

    FREEZE,
    // target becomes frozen and unable to move for a fixed number of turns

    SLEEP,
    // puts a target to sleep
    
    BURN,
    // burns a foe, 
    // receives variable damage per turn
    // if not treated, chance that target will become engulfed in flames

    BLIND,
    // blind a foe

    DARKEN,
    // corrupts the soul to eventually make a target go mad/berserk
    // fiends are unaffected
    // dark users are unaffected

    ENLIGHTEN,
    // can force those whose minds are clouded by darkness to change sides
    // fiends will simply die if enlightend
    // enlighted/light users are unaffected

    NONE,
    // no effect

}


public partial class AttackMeta  {

    public string name;
    public int power;
	public int defence;

    public int timesUsableUntilReset = 0;

    public AttackTargetMeta attackTargetMeta;

    public AttackAttribute attackAttribute;

    public AttackEffect effect;

    public AttackType attackType;
    
	public static List<AttackAttribute> attributeMap = new List<AttackAttribute> {
		  AttackAttribute.FIRE, // stronger than -->
		  AttackAttribute.EARTH, // stronger than -->
		  AttackAttribute.WIND, // stronger than -->
		  AttackAttribute.WATER // strong than ^
	};
    // perhaps effect in future
    // Effect effect
    public AttackMeta(
        string name, 
        int power,
		int defence,
        int timesUsableUntilReset,
        AttackTargetMeta attackTargetMeta,
        AttackAttribute attackAttribute,
        AttackEffect effect = AttackEffect.NONE,
        AttackType attackType = AttackType.PHYSICAL
    ) {
        this.name = name;
        this.power = power;
		this.defence = defence;
        this.timesUsableUntilReset = timesUsableUntilReset;
        this.attackTargetMeta = attackTargetMeta;
        this.attackAttribute = attackAttribute;
        this.effect = effect;
        this.attackType = attackType;
    }

}
