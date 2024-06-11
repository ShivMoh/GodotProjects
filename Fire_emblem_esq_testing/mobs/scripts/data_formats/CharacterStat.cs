using System;
using Godot;

public partial class CharacterStat {

    public string name {get; set;} = String.Empty;

    public int health { get; set; } = 0;

    public int strenth {get; set;} = 0;

    public int intelligence {get; set;} = 0;

    public int skill {get; set;} = 0;

    public int constition {get; set;} = 0;

    public CharacterStat(
        string name,
        int health,
        int strenth,
        int intelligence,
        int skill,
        int constition
    ) {
        this.name = name;
        this.health = health;
        this.strenth = strenth;
        this.intelligence = intelligence;
        this.skill = skill;
        this.constition = constition;
    }   

}