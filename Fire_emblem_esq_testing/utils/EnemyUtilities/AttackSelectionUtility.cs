using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AttackSelectionUtility {

	private EnemyCharacter enemyCharacter;
	private List<List<Character>> targetCandidates;

	private List<Character> targetableCharactersInCloseRange;

	private List<AttackMeta> availableAttacks; 

	private AttackMeta chosenAttack;

	private List<Character> targets;

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
	}
	
	public void chooseCunningCharacterAttack() {
	  
		List<AttackMeta> attackCandidates = new List<AttackMeta>();
		List<Character> refinedTargetCandidates = new List<Character>();        
		int targetRange = 0;

		if (canAttackFromDistance()) {
			attackCandidates = chooseAttacksWithGreatestRange();
			
			for (int i = 0; i < attackCandidates.Count(); i++)
			{
				List<Character> characterList = targetCandidates.ElementAt(i);
				
				foreach (Character character in characterList)
				{
					if (!willAttackingHurtCharacter(character, attackCandidates.ElementAt(i))) {
						refinedTargetCandidates.Add(character);
					}
				}
			}

			targetRange = attackCandidates.First().attackTargetMeta.range;			
		}


		if (refinedTargetCandidates.Count() == 0) {

			List<AttackMeta> closeRangeAttacks = this.getCloseRangeAttacks();
			List<Character> closeRangeTargetable = new List<Character>();

			foreach (Character character in targetableCharactersInCloseRange)
			{
				if (closeRangeAttacks.Count() > 0) {
					if (!willAttackingHurtCharacter(character, closeRangeAttacks.First())) {
						closeRangeTargetable.Add(character);
					}
				}     
			}

			if (closeRangeTargetable.Count() > 0) {
				refinedTargetCandidates = closeRangeTargetable;
				attackCandidates = closeRangeAttacks;
				targetRange = 1;			

			} else {
				refinedTargetCandidates = this.targetCandidates.ElementAt(0);
				targetRange = this.availableAttacks.Max(attack => attack.attackTargetMeta.range);; 
			}
 
		} 
		
		this.chosenAttack = this.choseRandomAttack(attackCandidates);

		if (this.chosenAttack.attackTargetMeta.targetableCount == 1) {
			this.targets.Add(this.chooseRandomCharacter(refinedTargetCandidates));
		} else if (this.chosenAttack.attackTargetMeta.areaOfEffect) {
			// lets deal with this later i suppose
			this.targets.Add(this.chooseRandomCharacter(refinedTargetCandidates));
		} else {
			this.targets.Add(this.chooseRandomCharacter(refinedTargetCandidates));

			// I'll have to like calculate the optimal position to target the most enemies...
			// so yh lets deal with this later
			// if(this.chosenAttack.attackTargetMeta.targetableCount == refinedTargetCandidates.Count()) {
			// 	this.targets = refinedTargetCandidates;
			// } else if (this.chosenAttack.attackTargetMeta.targetableCount < refinedTargetCandidates.Count()) {
			// 	for(int i = 0; i < refinedTargetCandidates.Count(); i++) {
			// 		Character candidate = this.chooseRandomCharacter(refinedTargetCandidates);
			// 		this.targets.Add(candidate);
			// 		refinedTargetCandidates.Remove(candidate);				
			// 	}
			// } else {
			// 	this.targets.AddRange(refinedTargetCandidates);
			// 	List<Character> targetableCharacters = this.targetCandidates.ElementAt(
			// 		this.enemyCharacter.attacks.IndexOf(this.chosenAttack)
			// 	);

			// 	for(int i = 0; i < (this.chosenAttack.attackTargetMeta.targetableCount - this.targets.Count()); i++) {

			// 		while (true)
			// 		{
			// 			Character candidate = this.chooseRandomCharacter(targetableCharacters);

			// 			if(!this.targets.Contains(candidate)) {
			// 				this.targets.Add(candidate);				
			// 				break;
			// 			} 

			// 			targetableCharacters.Remove(candidate);
						
			// 		}
			// 	}
			// }
		}
		MapEntities.attackRange = targetRange;
	}

	public List<Character> getTargets() {
		return this.targets;
	}

	public AttackMeta getAttack() {
		return this.chosenAttack;
	}


	private AttackMeta choseRandomAttack(List<AttackMeta> attacks) {
		var rand = new Random();

		int randomIndex = rand.Next(0, attacks.Count());

		return attacks.ElementAt(randomIndex);
	}


	private Character chooseRandomCharacter(List<Character> characters) {
		var rand = new Random();

		int randomIndex = rand.Next(0, characters.Count());

		return characters.ElementAt(randomIndex);
	}

	private bool canAttackFromDistance() {
		return this.availableAttacks.First().attackTargetMeta.range > 1;
	}

	private List<AttackMeta> getCloseRangeAttacks() {
		return (List<AttackMeta>)   this.availableAttacks
									.Where(attack => attack.attackTargetMeta.closeRange == true)
									.ToList();
	}

	private List<AttackMeta> chooseAttacksWithGreatestRange() {
		int greatestRange = availableAttacks.Max(attack => attack.attackTargetMeta.range);
		return (List<AttackMeta>)   availableAttacks
									.Where(attack => attack.attackTargetMeta.range == greatestRange)
									.OrderByDescending(attack => attack.attackTargetMeta.range)
									.ToList();
	}


	private bool willAttackingHurtCharacter(Character target, AttackMeta attackCandidate) {
		return target.equipedAttack.attackTargetMeta.range >= attackCandidate.attackTargetMeta.range;
	}

	private bool shouldEnemyAttackFromDistance() {

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
