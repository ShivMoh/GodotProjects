using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

	/* 
		TODO - this function doesn't account for blocked paths. Will need to rewrite it or utlize another function
		for determining paths that incorporate objects blocking path
	*/
	public List<List<Character>> findTargetsWithinRange(List<PlayableCharacter> characters) {

		Vector2I enemyTileLocation = tileMap.LocalToMap(enemyCharacter.Position);
		this.availableAttacks = AttackSelectionUtility.determineAvailableAttacks(enemyCharacter);
		//.OrderByDescending(attack => attack.attackTargetMeta.range);
		List<List<Character>> targetCandidates = new List<List<Character>>();

		if (availableAttacks.Count() == 0) return targetCandidates; 
		foreach (AttackMeta attack in availableAttacks) {
			List<Character> targetableCandidatesForAttack = new List<Character>();

			foreach (PlayableCharacter playableCharacter in characters) {
				Vector2I playableTileLocation = tileMap.LocalToMap(playableCharacter.Position);

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
		Vector2I enemyTileLocation = tileMap.LocalToMap(enemyCharacter.Position);

		foreach (PlayableCharacter playableCharacter in characters) {
			Vector2I playableTileLocation = tileMap.LocalToMap(playableCharacter.Position);

			int numberOfStepsTo = Mathf.Abs(playableTileLocation.Y - enemyTileLocation.Y) + 
			Mathf.Abs(playableTileLocation.X - enemyTileLocation.X) - 1;
			

			if (numberOfStepsTo <= enemyCharacter.moveSteps) {
				targetCandidates.Add(playableCharacter);
			}        
		}  

		return targetCandidates;
	} 

	
	// this is the circle function 
	public Vector2 findAvailableSpotForTarget(Character character, List<EnemyCharacter> entities, int range) {

		Vector2I centerPosition = this.tileMap.LocalToMap(character.Position);
		Vector2I pointPosition = Vector2I.Zero;
		bool occupied = false;
		float increment = 0.0f;
		
		for (int i = 0; i < 12; i++)
		{
			pointPosition = new Vector2I(
											Mathf.RoundToInt(centerPosition.X + range * Mathf.Cos(Mathf.DegToRad(increment))),
											Mathf.RoundToInt(centerPosition.Y + range * Mathf.Sin(Mathf.DegToRad(increment)))
										);
			
			foreach (Character entity in entities)
			{
				// //GD.Print("ENEMY Position", entity.Position);
				Vector2I entityGlobalPosition = this.tileMap.LocalToMap(entity.Position);
	
				if (pointPosition == entityGlobalPosition) {
					// //GD.Print(centerPosition.X + range * Mathf.Cos(Mathf.DegToRad(increment)));
					// //GD.Print(centerPosition.Y + range * Mathf.Sin(Mathf.DegToRad(increment)));
					// //GD.Print(entityGlobalPosition, pointPosition);
					occupied = true;
				} 

			}
			
			increment+=30;

			if (occupied == false) {
				//GD.Print("Does this run");
				return this.tileMap.MapToLocal(pointPosition); 
			} else {
				occupied = false;
			}
		}
		
		return Vector2.Zero;

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
// 	Mathf.Pow(playableCharacter.Position.X - enemyCharacter.Position.X, 2) +                 
// 	Mathf.Pow(playableCharacter.Position.Y - enemyCharacter.Position.Y, 2) 
// );
