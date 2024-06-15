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

	public List<AttackMeta> getAvailableAttacks() {
		return this.availableAttacks;
	}
	public List<List<Character>> findTargetsWithinRange(List<PlayableCharacter> characters) {

		Vector2I enemyTileLocation = tileMap.LocalToMap(enemyCharacter.GlobalPosition);
		this.availableAttacks = AttackSelectionUtility.determineAvailableAttacks(enemyCharacter);
		//.OrderByDescending(attack => attack.attackTargetMeta.range);
		List<List<Character>> targetCandidates = new List<List<Character>>();

		if (availableAttacks.Count() == 0) return targetCandidates; 
		foreach (AttackMeta attack in availableAttacks) {
			List<Character> targetableCandidatesForAttack = new List<Character>();

			foreach (PlayableCharacter playableCharacter in characters) {
				Vector2I playableTileLocation = tileMap.LocalToMap(playableCharacter.GlobalPosition);

				int numberOfStepsTo = Mathf.Abs(playableTileLocation.Y - enemyTileLocation.Y) + 
				Mathf.Abs(playableTileLocation.X - enemyTileLocation.X) - (attack.attackTargetMeta.range + 1);

				if (numberOfStepsTo <= enemyCharacter.moveSteps) {
					targetableCandidatesForAttack.Add(playableCharacter);
				}          

				
			}          
			
			targetCandidates.Add(targetableCandidatesForAttack);

		}

		return targetCandidates;
	}

	public List<Character> findTargetsWithinCloseRange(List<PlayableCharacter> characters) {
		List<Character> targetCandidates = new List<Character>();
		Vector2I enemyTileLocation = tileMap.LocalToMap(enemyCharacter.GlobalPosition);

		foreach (PlayableCharacter playableCharacter in characters) {
			Vector2I playableTileLocation = tileMap.LocalToMap(playableCharacter.GlobalPosition);

			int numberOfStepsTo = Mathf.Abs(playableTileLocation.Y - enemyTileLocation.Y) + 
			Mathf.Abs(playableTileLocation.X - enemyTileLocation.X) - 1;

			if (numberOfStepsTo <= enemyCharacter.moveSteps) {
				targetCandidates.Add(playableCharacter);
			}        
		}  

		return targetCandidates;
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
