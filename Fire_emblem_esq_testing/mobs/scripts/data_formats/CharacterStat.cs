using System;
using Godot;

public enum Trait
{
    CUNNING,
    // 1. will always attack prefer a distance, unless its a killing blow (and distance doesn't deal the killing blow)
    // 2. targets the weakest or most injured character

    BRAZEN, 
    // 1. will likely fight upfront, if possible
    // 2. targets the strongest character on the board 
    // 3. will likely prefer to use the most damaging attack they possess

    COWARD,
    // 1. will attack from a distance (prefer attacks where they don't get hurt back) or not attack at all unless its a killing blow
    // 2. will run if certain conditions are met such as low health, number of allies < x, etc.

    NONE
    // 1. Basic soldier, will just blindly attack whomever is closest that will deal the most damage
}


public partial class CharacterStat : ICloneable {

    public string name {get; set;} = String.Empty;

    public int health { get; set; } = 0;

    public int strenth {get; set;} = 0;

    public int magic {get; set;} = 0;

    public int speed {get; set; } = 0;

    public int physicalDefence {get; set; } = 0;

    public int magicalDefence {get; set; } = 0;
    
    public int intelligence {get; set;} = 0;

    public int skill {get; set;} = 0;

    public int constition {get; set;} = 0;
    public Trait trait {get; set;} = Trait.NONE;
    public CharacterStat(
        string name,
        int health,
        int strenth,
        int magic,
        int speed,
        int physicalDefence,
        int magicalDefence,
        int intelligence,
        int skill,
        int constition,
        Trait trait
    ) {
        this.name = name;
        this.health = health;
        this.strenth = strenth;
        this.magic = magic;
        this.speed = speed;
        this.physicalDefence = physicalDefence;
        this.magicalDefence = magicalDefence;
        this.intelligence = intelligence;
        this.skill = skill;
        this.constition = constition;
        this.trait = trait;
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}