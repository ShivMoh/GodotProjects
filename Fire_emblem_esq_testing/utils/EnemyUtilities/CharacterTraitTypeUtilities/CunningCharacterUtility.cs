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

		int targetRange = 0;

		if (canAttackFromDistance()) {
			attackCandidates = chooseAttacksWithGreatestRange();
				
			List<Character> characterList = targetCandidates.ElementAt(0);

			refinedTargetCandidates.AddRange(characterList);

			targetRange = attackCandidates.First().attackTargetMeta.range;			
		}

		refinedTargetCandidates = behaviourSelector(refinedTargetCandidates, availableAttacks);
		
		this.targets = refinedTargetCandidates;
		targetRange = this.chosenAttack.attackTargetMeta.range;
		MapEntities.attackRange = targetRange;
	}

	protected List<Character> behaviourSelector(List<Character> refinedTargetCandidates, List<AttackMeta> attackCandidates) {
		int threshold = 2;
		List<Character> targets = new List<Character>(refinedTargetCandidates);

				
		AttackMeta attackWithGreatestRange = attackCandidates.MaxBy(attack => attack.attackTargetMeta.range);
		List<AttackMeta> areaOfEffectAttacks  = attackCandidates.Where(attack => attack.attackTargetMeta.areaOfEffect).ToList();
		List<AttackMeta> multiTargetAttacks = attackCandidates.Where(attack => attack.attackTargetMeta.targetableCount > 1).ToList();
		List<AttackMeta> viableAttacks = new List<AttackMeta>();
		
		// if the attack with the greatest range is a area of effect attack or multitarget attack, just go with that
		if (attackWithGreatestRange.attackTargetMeta.areaOfEffect || attackWithGreatestRange.attackTargetMeta.targetableCount > 1) {		
			this.chosenAttack = attackWithGreatestRange;
			Dictionary<Vector2I, List<Character>> pools = this.findPoolsForAttack(this.chosenAttack);

			var maxPool = pools.MaxBy(pool => pool.Value.Count());

			targets.Clear();
			targets.AddRange(maxPool.Value);

			return targets;

		} 
		

		if (this.generateRandomFloat() > 0.5 && (areaOfEffectAttacks.Count() != 0 || multiTargetAttacks.Count() != 0)) {
			
			// get viable attacks
			foreach (AttackMeta attack in areaOfEffectAttacks.Concat(multiTargetAttacks))
			{	
				if (Mathf.Abs(attackWithGreatestRange.attackTargetMeta.range - attack.attackTargetMeta.range) <= threshold) {
					viableAttacks.Add(attack);
				}
			}

			// prioritize range so we pick viable attacks with greatest range
			int maxRange = viableAttacks.Max(attack => attack.attackTargetMeta.range);
			viableAttacks = viableAttacks.Where(attack => attack.attackTargetMeta.range == maxRange).ToList();
			
			if (viableAttacks.Count() > 0) {
				// get the/a area of effect attack with the greatest area of effect
				areaOfEffectAttacks = viableAttacks.Where(attack => attack.attackTargetMeta.areaOfEffect).ToList(); 
				AttackMeta chosenAreaOfEffectAttack = areaOfEffectAttacks.FirstOrDefault();
				AttackMeta chosenMultiTargetAttack = multiTargetAttacks.FirstOrDefault();

				if (areaOfEffectAttacks.Count() != 0) {
					chosenAreaOfEffectAttack = areaOfEffectAttacks.MaxBy(attack => attack.attackTargetMeta.radius);
				} else {
					// by notion of the entry condition, since areaOfEffectAttacks are 0, multiTargetAttacks cannot be 0 so no need for checks
					multiTargetAttacks = viableAttacks.Where(attack => attack.attackTargetMeta.targetableCount > 1).ToList(); 
					this.chosenAttack = multiTargetAttacks.MaxBy(attack => attack.attackTargetMeta.targetableCount);
					AttackMeta attackMeta = enemyCharacter.attacks.Where(attack => attack.name == this.chosenAttack.name).First();
					int indexOfChosenAttack = enemyCharacter.attacks.IndexOf(attackMeta);
					targets = this.targetCandidates.ElementAt(indexOfChosenAttack);
					return targets;
				}

				// get multitarget attack with the greates number of targets
				multiTargetAttacks = viableAttacks.Where(attack => attack.attackTargetMeta.targetableCount > 1).ToList(); 

				if (multiTargetAttacks.Count() != 0) {
					chosenMultiTargetAttack = multiTargetAttacks.MaxBy(attack => attack.attackTargetMeta.targetableCount);

				} else {
					chosenAreaOfEffectAttack = areaOfEffectAttacks.MaxBy(attack => attack.attackTargetMeta.radius);
					this.chosenAttack = chosenAreaOfEffectAttack;
					Dictionary<Vector2I, List<Character>> pools = this.findPoolsForAttack(this.chosenAttack);
					var maxKeyValuePair = pools.MaxBy(pool => pool.Value.Count());
					targets = maxKeyValuePair.Value;
					return targets;
				}


				if (areaOfEffectAttacks.Count() != 0 && multiTargetAttacks.Count() != 0) {
					Dictionary<Vector2I, List<Character>> pools = this.findPoolsForAttack(chosenAreaOfEffectAttack);

					var maxKeyValuePair = pools.MaxBy(pool => pool.Value.Count());

					if(chosenMultiTargetAttack.attackTargetMeta.targetableCount > maxKeyValuePair.Value.Count()) {
						this.chosenAttack = chosenMultiTargetAttack;
						int indexOfChosenAttack = enemyCharacter.attacks.IndexOf(this.chosenAttack);
						targets = targetCandidates.ElementAt(indexOfChosenAttack);

					} else if (chosenMultiTargetAttack.attackTargetMeta.targetableCount > maxKeyValuePair.Value.Count()) {
						this.chosenAttack = chosenAreaOfEffectAttack;
						targets = maxKeyValuePair.Value;
					} else {
						viableAttacks.Clear();
						viableAttacks.Add(chosenAreaOfEffectAttack);
						viableAttacks.Add(chosenMultiTargetAttack);
						this.chosenAttack = this.choseRandomAttack(viableAttacks);
					}
				}
				

			}

			return targets;

		} 
		
		if (true) {
			List<Character> temp = new List<Character>() {
				this.chooseRandomCharacter(targets)
			};						
			this.chosenAttack = attackWithGreatestRange;
			return temp;
		}
	}

	

	// public Dictionary<Vector2I, List<Character>> findPoolsForAttack(AttackMeta attackMeta) {
	// 	Dictionary<Vector2I, List<Character>> spots = new Dictionary<Vector2I, List<Character>>();
		
	// 	int attackIndex = enemyCharacter.attacks.IndexOf(attackMeta);
		
	// 	List<Character> targetCandidatesForAttack = targetCandidates[attackIndex];

	// 	foreach(Character character in targetCandidatesForAttack) {

	// 		Vector2I tileCoordsForCharacter = MapEntities.map.LocalToMap(character.Position);
	// 		List<Character> hitCharacters = new List<Character>();
	// 		int radius = (int) Mathf.Floor( (float)  attackMeta.attackTargetMeta.radius / 2);

	// 		Vector2 pointA = new Vector2(tileCoordsForCharacter.X - radius, tileCoordsForCharacter.Y - radius);
	// 		Vector2 pointB = new Vector2(tileCoordsForCharacter.X + radius, tileCoordsForCharacter.Y + radius);
			
	// 		foreach(Character character2 in targetCandidatesForAttack) {
	// 			Vector2 tileCoordsForCharacter2 = MapEntities.map.LocalToMap(character2.Position);

	// 			if( 	tileCoordsForCharacter2.X > pointA.X && tileCoordsForCharacter2.X < pointB.X &&
	// 					tileCoordsForCharacter2.X < pointB.X && tileCoordsForCharacter2.Y < pointB.Y
	// 			) {
	// 				hitCharacters.Add(character2);
	// 			}
				
	// 		}

	// 		spots.Add(tileCoordsForCharacter, hitCharacters);
	// 	}

	// 	return spots;
	// }

	

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
