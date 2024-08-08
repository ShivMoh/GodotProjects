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
		
		var maxPool = pools.MaxBy(pool => pool.Value.Count());
		// int maxCount = pools.Values.First().Count();
		// List<Character> characters = new List<Character>();
		// foreach(List<Character> pool in pools.Values) {
		// 	if(pool.Count() > maxCount) {
		// 		maxCount = pool.Count();
		// 		characters.Clear();
		// 		characters.AddRange(pool);
		// 	}
		// }

		// if(characters.Count() == 0) characters.AddRange(pools.Values.First());
	
		this.targets.AddRange(maxPool.Value);

		MapEntities.attackRange = this.chosenAttack.attackTargetMeta.range;
		
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
