using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AttackSelectionUtility {

	protected EnemyCharacter enemyCharacter;
	protected List<List<Character>> targetCandidates;

	protected List<Character> targetableCharactersInCloseRange;

	protected List<AttackMeta> availableAttacks; 

	protected AttackMeta chosenAttack;

	protected List<Character> targets;

	public AttackSelectionUtility(
		EnemyCharacter enemyCharacter, 
		List<List<Character>> targetCandidates,
		List<AttackMeta> availableAttacks,
		List<Character> targetableCharactersInCloseRange
	) {
		this.enemyCharacter = enemyCharacter;
		this.targetCandidates = targetCandidates;
		this.availableAttacks = availableAttacks;
		this.targetableCharactersInCloseRange = targetableCharactersInCloseRange;
		this.targets = new List<Character>();
	}

	public virtual void chooseAttack() {
		return;
	}
	
	// public void chooseCunningCharacterAttack() {
	  
	// 	List<AttackMeta> attackCandidates = new List<AttackMeta>();
	// 	List<Character> refinedTargetCandidates = new List<Character>();        
	// 	int targetRange = 0;

	// 	if (canAttackFromDistance()) {
	// 		attackCandidates = chooseAttacksWithGreatestRange();
			
	// 		for (int i = 0; i < attackCandidates.Count(); i++)
	// 		{
	// 			List<Character> characterList = targetCandidates.ElementAt(i);
				
	// 			foreach (Character character in characterList)
	// 			{
	// 				if (!willAttackingHurtCharacter(character, attackCandidates.ElementAt(i))) {
	// 					refinedTargetCandidates.Add(character);
	// 				}
	// 			}
	// 		}

	// 		targetRange = attackCandidates.First().attackTargetMeta.range;			
	// 	}


	// 	if (refinedTargetCandidates.Count() == 0) {

	// 		List<AttackMeta> closeRangeAttacks = this.getCloseRangeAttacks();
	// 		List<Character> closeRangeTargetable = new List<Character>();

	// 		foreach (Character character in targetableCharactersInCloseRange)
	// 		{
	// 			if (closeRangeAttacks.Count() > 0) {
	// 				if (!willAttackingHurtCharacter(character, closeRangeAttacks.First())) {
	// 					closeRangeTargetable.Add(character);
	// 				}
	// 			}     
	// 		}

	// 		if (closeRangeTargetable.Count() > 0) {
	// 			refinedTargetCandidates = closeRangeTargetable;
	// 			attackCandidates = closeRangeAttacks;
	// 			targetRange = 1;			

	// 		} else {
	// 			refinedTargetCandidates = this.targetCandidates.ElementAt(0);
	// 			targetRange = this.availableAttacks.Max(attack => attack.attackTargetMeta.range);; 
	// 		}
 
	// 	} 
		
	// 	this.chosenAttack = this.choseRandomAttack(attackCandidates);

	// 	if (this.chosenAttack.attackTargetMeta.targetableCount == 1 && !this.chosenAttack.attackTargetMeta.areaOfEffect) {
	// 		this.targets.Add(this.chooseRandomCharacter(refinedTargetCandidates));
	// 	} else if (this.chosenAttack.attackTargetMeta.areaOfEffect) {
	// 		// lets deal with this later i suppose
	// 		this.targets.Add(this.chooseRandomCharacter(refinedTargetCandidates));
			
	// 	} else {
			
	// 		Character randomCharacter = this.chooseRandomCharacter(refinedTargetCandidates);

	// 		if (randomCharacter is not null) {
	// 			this.targets.Add(randomCharacter);
	// 		} else {
	// 			// //GD.Print("Null character reference");
	// 		}
	// 		// this for ranged attacks with more than one targets

	// 		// I'll have to like calculate the optimal GlobalPosition to target the most enemies...
	// 		// so yh lets deal with this later
	// 		// if(this.chosenAttack.attackTargetMeta.targetableCount == refinedTargetCandidates.Count()) {
	// 		// 	this.targets = refinedTargetCandidates;
	// 		// } else if (this.chosenAttack.attackTargetMeta.targetableCount < refinedTargetCandidates.Count()) {
	// 		// 	for(int i = 0; i < refinedTargetCandidates.Count(); i++) {
	// 		// 		Character candidate = this.chooseRandomCharacter(refinedTargetCandidates);
	// 		// 		this.targets.Add(candidate);
	// 		// 		refinedTargetCandidates.Remove(candidate);				
	// 		// 	}
	// 		// } else {
	// 		// 	this.targets.AddRange(refinedTargetCandidates);
	// 		// 	List<Character> targetableCharacters = this.targetCandidates.ElementAt(
	// 		// 		this.enemyCharacter.attacks.IndexOf(this.chosenAttack)
	// 		// 	);

	// 		// 	for(int i = 0; i < (this.chosenAttack.attackTargetMeta.targetableCount - this.targets.Count()); i++) {

	// 		// 		while (true)
	// 		// 		{
	// 		// 			Character candidate = this.chooseRandomCharacter(targetableCharacters);

	// 		// 			if(!this.targets.Contains(candidate)) {
	// 		// 				this.targets.Add(candidate);				
	// 		// 				break;
	// 		// 			} 

	// 		// 			targetableCharacters.Remove(candidate);
						
	// 		// 		}
	// 		// 	}
	// 		// }
	// 	}
	// 	MapEntities.attackRange = targetRange;
	// }

	public List<Character> getTargets() {
		return this.targets;
	}

	public AttackMeta getAttack() {
		return this.chosenAttack;
	}

	public void clearAttacks() {
		this.chosenAttack = null;
	}

	public void clearTargets() {
		this.targets.Clear();
	}


	protected Dictionary<Vector2I, List<Character>> findPoolsForAttack(AttackMeta attackMeta) {
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


				Vector2I tileCoordsForCharacter2 = MapEntities.map.LocalToMap(character2.Position);

				if( 	(tileCoordsForCharacter2.X >= pointA.X && tileCoordsForCharacter2.X <= pointB.X) &&
						(tileCoordsForCharacter2.Y >= pointA.Y && tileCoordsForCharacter2.Y <= pointB.Y)
				) {
					hitCharacters.Add(character2);
				}
				
			}

			spots.Add(tileCoordsForCharacter, hitCharacters);
			// hitCharacters.Clear();
		}

		return spots;
	}

	protected AttackMeta choseRandomAttack(List<AttackMeta> attacks) {
		var rand = new Random();

		int randomIndex = rand.Next(0, attacks.Count());

		return attacks.ElementAt(randomIndex);
	}


	protected Character chooseRandomCharacter(List<Character> characters) {
		var rand = new Random();

		if (characters.Count() == 0) return null;
		if (characters.Count() == 1) return characters.First();

		int randomIndex = rand.Next(0, characters.Count());
		
		if (!characters.ElementAt(randomIndex).IsQueuedForDeletion()) {
			return characters.ElementAt(randomIndex);
		} else {
			return chooseRandomCharacter(characters);
		}
	}

	protected double generateRandomFloat() {
		var rand = new Random();

		double randomFloat = rand.NextDouble();

		return randomFloat;
	}	

	protected bool canAttackFromDistance() {
		return this.availableAttacks.First().attackTargetMeta.range > 1;
	}

	protected List<AttackMeta> getCloseRangeAttacks() {
		return (List<AttackMeta>)   this.availableAttacks
									.Where(attack => attack.attackTargetMeta.closeRange == true)
									.ToList();
	}

	protected List<AttackMeta> chooseAttacksWithGreatestRange() {
		int greatestRange = availableAttacks.Max(attack => attack.attackTargetMeta.range);
		return (List<AttackMeta>)   availableAttacks
									.Where(attack => attack.attackTargetMeta.range == greatestRange)
									.OrderByDescending(attack => attack.attackTargetMeta.range)
									.ToList();
	}


	protected bool willAttackingHurtCharacter(Character target, AttackMeta attackCandidate) {
		return target.equipedAttack.attackTargetMeta.range >= attackCandidate.attackTargetMeta.range;
	}

	protected bool shouldEnemyAttackFromDistance() {

		if (enemyCharacter.characterStat.health > 10) {
			return false;
		}        

		return true;
	}
	public static List<AttackMeta> sortAttacksByRange(List<AttackMeta> attackMetas) {
		return (List<AttackMeta>)   attackMetas
									.OrderByDescending(attack => attack.attackTargetMeta.range)
									.ToList();
	}

	public static List<AttackMeta> determineAvailableAttacks(Character character) {
		if (character.attacks.Count() == 0) return new List<AttackMeta>();
		return character.attacks.FindAll(attack => attack.timesUsableUntilReset > 0);
	}
}
