using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class CunningCharacterUtility {
    private EnemyCharacter enemyCharacter;
    private List<List<Character>> targetCandidates;

    private List<Character> targetableCharactersInCloseRange;

    private List<AttackMeta> availableAttacks; 

    private AttackMeta chosenAttack;

    private Character target;

    public CunningCharacterUtility(
        EnemyCharacter enemyCharacter, 
        List<List<Character>> targetCandidates,
        List<AttackMeta> availableAttacks
    ) {
        this.enemyCharacter = enemyCharacter;
        this.targetCandidates = targetCandidates;
        this.availableAttacks = availableAttacks;
    }

    public void chooseCunningCharacterAttack() {
      
        List<AttackMeta> attackCandidates = new List<AttackMeta>();
        List<Character> refinedTargetCandidates = new List<Character>();        

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
                } else {
                    refinedTargetCandidates = this.targetCandidates.ElementAt(attackCandidates.IndexOf(chosenAttack));
                }
            } else {
                // do nothing ig
            }

        }
        
        this.chosenAttack = this.choseRandomAttack(attackCandidates);
        this.target = this.chooseRandomCharacter(refinedTargetCandidates);

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
        return (List<AttackMeta>) this.availableAttacks.Where(attack => attack.attackTargetMeta.closeRange == true);
    }

    private List<AttackMeta> chooseAttacksWithGreatestRange() {
        int greatestRange = availableAttacks.Max(attack => attack.attackTargetMeta.range);
        return (List<AttackMeta>)   availableAttacks
                                    .Where(attack => attack.attackTargetMeta.range == greatestRange)
                                    .OrderByDescending(attack => attack.attackTargetMeta.range);
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
}