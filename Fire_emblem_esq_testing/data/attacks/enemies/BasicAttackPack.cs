using Godot;
using System.Text.Json;
using System.Collections.Generic;
using System.Reflection.Metadata;

public static partial class BasicAttackPack {
    
    public static Dictionary<string, List<AttackMeta>> basicMagicAttacksPacks = new Dictionary<string, List<AttackMeta>>()
    {   
        {"0001", new List<AttackMeta> {
            BasicMagicAttack.basicMagicAttacks["iceball"]
			// BasicMagicAttack.basicMagicAttacks["fireball"]
        }},
	    {"0002", new List<AttackMeta> {
		    BasicMagicAttack.basicMagicAttacks["iceball"],
		    BasicMagicAttack.basicMagicAttacks["fireball"],
            BasicMagicAttack.basicMagicAttacks["windball"]
	    }}
    };
}   
