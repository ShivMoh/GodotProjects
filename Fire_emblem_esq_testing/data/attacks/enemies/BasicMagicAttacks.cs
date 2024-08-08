using Godot;
using System.Text.Json;
using System.Collections.Generic;  

public static partial class BasicMagicAttack {
    
    public static Dictionary<string, AttackMeta> basicMagicAttacks = new Dictionary<string, AttackMeta>()
    {   
        {"fireball", new AttackMeta(
			name: "Fire ball",
			power: 5,
			defence: 2,
			timesUsableUntilReset: 20,
			attackTargetMeta: new AttackTargetMeta(5, 1, radius: 5, areaOfEffect: true),
			attackAttribute: AttackAttribute.FIRE,
			effect: AttackEffect.BURN,  
			attackType: AttackType.MAGICAL
		)},
        { "iceball", new AttackMeta(
			name: "Ice ball",
			power: 3,
			defence: 4,
			timesUsableUntilReset: 20,
			attackTargetMeta: new AttackTargetMeta(7, 1, radius: 1),
			attackAttribute: AttackAttribute.WATER,
			effect: AttackEffect.FREEZE,
			attackType: AttackType.MAGICAL
		)},
		{ "windball", new AttackMeta(
			name: "Wind ball",
			power: 3,
			defence: 4,
			timesUsableUntilReset: 20,
			attackTargetMeta: new AttackTargetMeta(2, 2, radius: 1),
			attackAttribute: AttackAttribute.WIND,
			effect: AttackEffect.NONE,
			attackType: AttackType.MAGICAL
		)}
    };
}   
