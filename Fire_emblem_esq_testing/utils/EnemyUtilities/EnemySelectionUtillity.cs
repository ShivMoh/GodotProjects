using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class EnemySelectionUtility {
    private List<PlayableCharacter> playableCharacters;

    private EnemyCharacter enemyCharacter;

    private TileMap tileMap;

    private List<AttackMeta> availableAttacks;

    public EnemySelectionUtility(
        EnemyCharacter enemyCharacter,
        TileMap tileMap
    ) {
        this.enemyCharacter = enemyCharacter;
        this.tileMap = tileMap;
    }

    /*
        ["Fire ball", "Ice ball"]
        [
            ["this guy", "that guy"],
            ["this guy", "that guy"]
        ]

        // get all targetable for fireball

        fireballindex = indexof(fireball)
        list[fireballindex]
            */
	public List<List<Character>> findTargetsWithinRange(List<PlayableCharacter> characters) {

        Vector2I enemyTileLocation = tileMap.LocalToMap(enemyCharacter.GlobalPosition);
        List<AttackMeta> availableAttacks = this.getAvailableAttacks(enemyCharacter);
        List<List<Character>> targetCandidates = new List<List<Character>>();

        if (availableAttacks.Count() == 0) return targetCandidates; 

		foreach (PlayableCharacter playableCharacter in characters)
		{
            Vector2I playableTileLocation = tileMap.LocalToMap(playableCharacter.GlobalPosition);

            List<Character> targetableCandidatesForAttack = new List<Character>();

            foreach (AttackMeta attack in availableAttacks)
            {

                int numberOfStepsTo = Mathf.Abs(playableTileLocation.Y - enemyTileLocation.Y) + 
                Mathf.Abs(playableTileLocation.X - enemyTileLocation.X) - (attack.attackTargetMeta.range + 1);

                if (numberOfStepsTo <= enemyCharacter.moveSteps) {
                    targetableCandidatesForAttack.Add(playableCharacter);
                }           
            }

            targetCandidates.Add(targetableCandidatesForAttack);
            targetableCandidatesForAttack.Clear();
		}

        return targetCandidates;
	}

    private List<AttackMeta> getAvailableAttacks(Character character) {
        if (character.attacks.Count() == 0) return new List<AttackMeta>();
        return character.attacks.FindAll(attack => attack.timesUsableUntilReset > 0);
    }

    private int getGreatestRange(PlayableCharacter playableCharacter) {
        int greatestRange = 0;
        foreach (AttackMeta attack in playableCharacter.attacks)
        {
            if (attack.attackTargetMeta.range > greatestRange && !attack.attackTargetMeta.areaOfEffect) {
                greatestRange = attack.attackTargetMeta.range;
            }
        }
        return greatestRange;
    }
    public Character findWeakestDefenceTarget(List<Character> characters) {
        if (enemyCharacter.characterStat.strenth > enemyCharacter.characterStat.magic) {
            return characters.MinBy(character => character.characterStat.physicalDefence);
        } else {
            return characters.MinBy(character => character.characterStat.magicalDefence);
        }
    }

    public void findStrongestTarget() {}
    public void findLowestHealthTarget() {}

    // the area where a given multi attack will have the most effect
    public void findLargestCharacterPool() {}
    
    // like a target u can attack from a distance
    // or without getting hurt back
    public void findSafestToAttackTarget() {}
}



// float distanceBetween = Mathf.Sqrt(
// 	Mathf.Pow(playableCharacter.GlobalPosition.X - enemyCharacter.GlobalPosition.X, 2) +                 
// 	Mathf.Pow(playableCharacter.GlobalPosition.Y - enemyCharacter.GlobalPosition.Y, 2) 
// );