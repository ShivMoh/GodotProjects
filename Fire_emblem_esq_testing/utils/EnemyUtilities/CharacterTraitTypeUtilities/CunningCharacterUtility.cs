using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class CunningCharacterUtility : AttackSelectionUtility{
   

    public CunningCharacterUtility(
        EnemyCharacter enemyCharacter, 
        List<List<Character>> targetCandidates, 
        List<AttackMeta> availableAttacks, 
        List<Character> targetableCharactersInCloseRange) : 
        base(enemyCharacter, targetCandidates, availableAttacks, targetableCharactersInCloseRange)
    {
    }

    public override void chooseAttack() {
	  
		List<AttackMeta> attackCandidates = new List<AttackMeta>();
		List<Character> refinedTargetCandidates = new List<Character>();        
		int threshold = 10;

		int targetRange = 0;

		if (canAttackFromDistance()) {
			attackCandidates = chooseAttacksWithGreatestRange();
				
			List<Character> characterList = targetCandidates.ElementAt(0);

            refinedTargetCandidates.AddRange(characterList);

			targetRange = attackCandidates.First().attackTargetMeta.range;			
		}

		this.chosenAttack = this.choseRandomAttack(attackCandidates);

		if (this.chosenAttack.attackTargetMeta.targetableCount == 1 && !this.chosenAttack.attackTargetMeta.areaOfEffect) {
			this.targets.Add(this.chooseRandomCharacter(refinedTargetCandidates));
		} else if (this.chosenAttack.attackTargetMeta.areaOfEffect) {
			this.targets.Add(this.chooseRandomCharacter(refinedTargetCandidates));
		} else {
			
			
			
			// Character randomCharacter = this.chooseRandomCharacter(refinedTargetCandidates);

			// if (randomCharacter is not null) {
			// 	this.targets.Add(randomCharacter);
			// } else {
			// 	// //GD.Print("Null character reference");
			// }
		}
		
		MapEntities.attackRange = targetRange;
	}

	

	public Dictionary<Vector2I, List<Character>> findPoolsForAttack(AttackMeta attackMeta) {
		Dictionary<Vector2I, List<Character>> spots = new Dictionary<Vector2I, List<Character>>();
		
		int attackIndex = enemyCharacter.attacks.IndexOf(attackMeta);
		
		List<Character> targetCandidatesForAttack = targetCandidates[attackIndex];

		foreach(Character character in targetCandidatesForAttack) {

			Vector2I tileCoordsForCharacter = MapEntities.map.LocalToMap(character.Position);
			List<Character> hitCharacters = new List<Character>();
			int radius = (int) Mathf.Floor( (float)  attackMeta.attackTargetMeta.radius / 2);

			Vector2 pointA = new Vector2(tileCoordsForCharacter.X - radius, tileCoordsForCharacter.Y - radius);
			Vector2 pointB = new Vector2(tileCoordsForCharacter.X + radius, tileCoordsForCharacter.Y + radius);
			
			foreach(Character character2 in targetCandidatesForAttack) {
				Vector2 tileCoordsForCharacter2 = MapEntities.map.LocalToMap(character2.Position);

				if( 	tileCoordsForCharacter2.X > pointA.X && tileCoordsForCharacter2.X < pointB.X &&
						tileCoordsForCharacter2.X < pointB.X && tileCoordsForCharacter2.Y < pointB.Y
				) {
					hitCharacters.Add(character2);
				}
				
			}

			spots.Add(tileCoordsForCharacter, hitCharacters);
		}

		return spots;
	}

	

    // character who priortizes most characters targeted
    // character who priortizes most damage dealth
    // character who priortizes furthest distance to attack from
    // character who priortizes safest attack 
    public void chooseRangedTargets() {

    }

    public void findSafestAttackRoute() {

    }


	private bool isEnemyInRange(Vector2I currentCoord, Vector2I enemyCoord, int attackRange) {
		float distanceTo  = Mathf.Sqrt(Mathf.Pow(currentCoord.X - enemyCoord.X, 2) + Mathf.Pow(currentCoord.Y - enemyCoord.Y, 2));
		if (Mathf.Floor(distanceTo) <= attackRange ) return true;
		return false;
	}



}