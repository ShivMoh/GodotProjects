using Godot;
using System.Text.Json;
using System.Collections.Generic;
using System;

public static partial class CharacterStats {
    
    public static Dictionary<string, CharacterStat> playableCharactersBase = new Dictionary<string, CharacterStat>()
    {   
        {"ellie", new CharacterStat(
            name: "Ellie",
            health: 50,
            strenth: 2,
            magic : 18,
            speed: 20,
            magicalDefence: 20,
            physicalDefence: 20,
            intelligence: 15,
            skill: 10,
            constition: 20,
            trait: Trait.CUNNING
	    )},
        { "teya", new CharacterStat(
            name: "Teya",
            health: 50,
            strenth: 2,
            magic : 18,
            speed: 20,
            magicalDefence: 20,
            physicalDefence: 20,
            intelligence: 15,
            skill: 10,
            constition: 20,
            trait: Trait.CUNNING
	    )},
        { "trent", new CharacterStat(
            name: "trent",
            health: 50,
            strenth: 2,
            magic : 18,
            speed: 20,
            magicalDefence: 20,
            physicalDefence: 20,
            intelligence: 15,
            skill: 10,
            constition: 20,
            trait: Trait.CUNNING
	    )},

    };
}   