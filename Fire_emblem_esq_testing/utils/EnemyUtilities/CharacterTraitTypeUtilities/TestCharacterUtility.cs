using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class TestCharacterUtility : AttackSelectionUtility{
   

	public TestCharacterUtility(
		EnemyCharacter enemyCharacter, 
		List<List<Character>> targetCandidates, 
		List<AttackMeta> availableAttacks, 
		List<Character> targetableCharactersInCloseRange) : 
		base(enemyCharacter, targetCandidates, availableAttacks, targetableCharactersInCloseRange)
	{
	}

	public override void chooseAttack() {
	  
		this.chosenAttack = this.enemyCharacter.attacks.Where(attack => attack.attackTargetMeta.areaOfEffect).FirstOrDefault();

		Dictionary<Vector2I, List<Character>> pools = findPoolsForAttack(this.chosenAttack);

		int maxCount = pools.Values.First().Count();
		List<Character> characters = new List<Character>();
		foreach(List<Character> pool in pools.Values) {
			if(pool.Count() > maxCount) {
				maxCount = pool.Count();
				characters.Clear();
				characters.AddRange(pool);
			}
		}

		if(characters.Count() == 0) characters.AddRange(pools.Values.First());
		GD.Print(pools.Values.First().Count());
		GD.Print(maxCount);
		GD.Print("Characters", characters.Count());
		this.targets.AddRange(characters);

		MapEntities.attackRange = this.chosenAttack.attackTargetMeta.range;
		
	}

	

	public Dictionary<Vector2I, List<Character>> findPoolsForAttack(AttackMeta attackMeta) {
		Dictionary<Vector2I, List<Character>> spots = new Dictionary<Vector2I, List<Character>>();
		
		int attackIndex = enemyCharacter.attacks.IndexOf(attackMeta);
		// GD.Print("attack index", attackIndex);
		// GD.Print("targetCandididates count", targetCandidates.Count());
		List<Character> targetCandidatesForAttack = targetCandidates[attackIndex];

		foreach(Character character in targetCandidatesForAttack) {

			Vector2I tileCoordsForCharacter = MapEntities.map.LocalToMap(character.Position);
			List<Character> hitCharacters = new List<Character>();
			int radius = (int) Mathf.Floor( (float)  attackMeta.attackTargetMeta.radius / 2);
			// GD.Print("Radius", radius);

			Vector2 pointA = new Vector2(tileCoordsForCharacter.X - radius, tileCoordsForCharacter.Y - radius);
			Vector2 pointB = new Vector2(tileCoordsForCharacter.X + radius, tileCoordsForCharacter.Y + radius);
			
			foreach(Character character2 in targetCandidatesForAttack) {

				// if (character == character2) continue;

				Vector2I tileCoordsForCharacter2 = MapEntities.map.LocalToMap(character2.Position);

				if( 	(tileCoordsForCharacter2.X >= pointA.X && tileCoordsForCharacter2.X <= pointB.X) &&
						(tileCoordsForCharacter2.Y >= pointA.Y && tileCoordsForCharacter2.Y <= pointB.Y)
				) {
					GD.Print("Hit");
					hitCharacters.Add(character2);
				}
				
			}

			spots.Add(tileCoordsForCharacter, hitCharacters);
			// hitCharacters.Clear();
		}

		foreach(List<Character> characterS in spots.Values) {
			GD.Print("Checking count", characterS.Count());
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
