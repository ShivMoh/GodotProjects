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

		if (this.chosenAttack.attackTargetMeta.targetableCount == 1 && !this.chosenAttack.attackTargetMeta.areaOfEffect) {
			this.targets.Add(this.chooseRandomCharacter(refinedTargetCandidates));
		} else if (this.chosenAttack.attackTargetMeta.areaOfEffect) {
			// lets deal with this later i suppose
			this.targets.Add(this.chooseRandomCharacter(refinedTargetCandidates));
			
		} else {
			
			Character randomCharacter = this.chooseRandomCharacter(refinedTargetCandidates);

			if (randomCharacter is not null) {
				this.targets.Add(randomCharacter);
			} else {
				// //GD.Print("Null character reference");
			}
			// this for ranged attacks with more than one targets

		}
		MapEntities.attackRange = targetRange;
	}

    // character who priortizes most characters targeted
    // character who priortizes most damage dealth
    // character who priortizes furthest distance to attack from
    // character who priortizes safest attack 
    public void chooseRangedTargets() {

    }

    public void findSafestAttackRoute() {
        
    }


}