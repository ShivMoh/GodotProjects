using System.Collections.Generic;
using System.Numerics;
using Godot;
using Vector2 = Godot.Vector2;
using System.Linq;
using System;

public partial class CombatUtility {
	
	private TileMap tilemap;
	private List<Character> enemies;
	private PlayableCharacter selectedCharacter;

	public CombatUtility(TileMap tilemap, List<Character> enemies, Character selectedCharacter) {
		this.tilemap = tilemap;
		this.enemies = enemies;

		if (selectedCharacter is PlayableCharacter) {
			this.selectedCharacter = selectedCharacter as PlayableCharacter;
		}
	}

	public void setSelectedCharacter(Character selectedCharacter) {
		if (selectedCharacter is PlayableCharacter) {
			this.selectedCharacter = selectedCharacter as PlayableCharacter;
		} else {
			this.selectedCharacter = null;
		}
	}

	public void highLightEnemy(TileUtility tileUtility, Vector2I coords) {
		tileUtility.highLight(coords, new Vector2I(0, 0));
	}

	public List<Character> detectEnemiesForAttack(TileUtility tileUtility, AttackMeta attack) {

		List<Character> characters = new List<Character>();

		Vector2I currentCoord = tilemap.LocalToMap(selectedCharacter.Position);

		foreach (Character character in enemies)
		{
			Vector2I enemyCoord = tilemap.LocalToMap(character.Position);
			
			// perhaps instead of this, I'll not make it impossible for like a bow to shoot a close range
			// but rather incur a penality on its attack
			if (attack.attackTargetMeta.closeRange) {
				if (this.checkAdjacent(currentCoord, enemyCoord)) {
					characters.Add(character);
				}
			}

			if (this.isEnemyInRange(currentCoord, tilemap.LocalToMap(character.Position), attack.attackTargetMeta.range)) {
				characters.Add(character);
			}
		}


		return characters;
	}   

	private bool isEnemyInRange(Vector2I currentCoord, Vector2I enemyCoord, int attackRange) {
		
		int absX = Mathf.Abs(enemyCoord.X - currentCoord.X);
		int absY = Mathf.Abs(enemyCoord.Y - currentCoord.Y);
		// check if in x
		if(absX <= attackRange && enemyCoord.Y == currentCoord.Y) return true;
	   
		// check if in y
		if (absY <= attackRange && enemyCoord.X == currentCoord.X) return true;

		// check if in diagonal
		if (absX <= attackRange && absX == absY) return true;

	//	float distanceTo  = Mathf.Sqrt(Mathf.Pow(currentCoord.X - enemyCoord.X, 2) + Mathf.Pow(currentCoord.Y - enemyCoord.Y, 2));
	//		if (Mathf.Floor(distanceTo) < attackRange ) return true;
		return false;
	}

	public  bool attackCharacter(Character selectedCharacter, Character target, AttackMeta chosenAttack) {

		target.GetNode<Label>("Hit").Text = "Missed!";
		if (!doesAttackHit(selectedCharacter, target, chosenAttack)) return false;
		target.GetNode<Label>("Hit").Text = "Hit!";

		int damage = this.attackSequence(selectedCharacter, target, chosenAttack);
		target.characterStat.health -= damage;

		if (target is EnemyCharacter) GD.Print("Health after attack", target.characterStat.health, damage);
		selectedCharacter.equipedAttack = chosenAttack;
		target.updateText();

		if (target.characterStat.health <= 0) {

			if (target.GetType().Name is nameof(EnemyCharacter)) {
				MapEntities.enemyCharacters.Remove(target as EnemyCharacter);
			}

			if (target.GetType().Name is nameof(PlayableCharacter)) {
				MapEntities.playableCharacters.Remove(target as PlayableCharacter);
			}

			MapEntities.characters.Remove(target);

			return true;

		} else {
			target.healthBar.takeDamage(damage);
		}
		return false;
	 }

	private bool doesAttackHit(Character attacker, Character target, AttackMeta chosenAttack) {

		RandomNumberGenerator random = new RandomNumberGenerator();
		int randomNumber = random.RandiRange(0, 100);
		int chanceToHit = (attacker.characterStat.skill + chosenAttack.accuracy) - (target.characterStat.speed); 

		if (chanceToHit >= randomNumber) return true;
		
		return false;
	}
	
	private int attackSequence(Character attacker, Character target, AttackMeta chosenAttack) {
		int damage = 0;
	
		if (chosenAttack.attackType == AttackType.MAGICAL) {
			damage = (attacker.characterStat.magic + chosenAttack.power);
		}

		if (chosenAttack.attackType == AttackType.PHYSICAL) {
			damage = (attacker.characterStat.strenth + chosenAttack.power); 

		}
		
		int currentAttackindexPlus = 
			AttackMeta.attributeMap.IndexOf(chosenAttack.attackAttribute) + 1 == AttackMeta.attributeMap.Count() 
			? 0 
			: AttackMeta.attributeMap.IndexOf(chosenAttack.attackAttribute) + 1;	

		int currentAttackindexLess = 
			AttackMeta.attributeMap.IndexOf(chosenAttack.attackAttribute) - 1 == -1 
			?  AttackMeta.attributeMap.Count() - 1 
			: AttackMeta.attributeMap.IndexOf(chosenAttack.attackAttribute) - 1;	

		if ( 		currentAttackindexPlus == 
				AttackMeta.attributeMap.IndexOf(target.equipedAttack.attackAttribute)) {
					GD.Print("Double Damage");
					damage = damage * 2;
			}

		if ( 		currentAttackindexLess == 
				AttackMeta.attributeMap.IndexOf(target.equipedAttack.attackAttribute)) {
					GD.Print("Half Damage");
					damage = damage/2;
			}
		if (chosenAttack.attackType == AttackType.MAGICAL) damage -= (target.equipedAttack.defence + target.characterStat.magicalDefence);
		if (chosenAttack.attackType == AttackType.PHYSICAL) damage -= (target.equipedAttack.defence + target.characterStat.physicalDefence);

		GD.Print(damage);
		if (damage < 0 ) damage = 0;


		 return damage; 
	}

	private bool checkAdjacent(Vector2I one, Vector2I two) {
		bool checkPositiveY = (one.Y + 1) == two.Y && one.X == two.X;
		bool checkNegativeY = (one.Y - 1) == two.Y && one.X == two.X;
		bool checkPositiveX = (one.X + 1) == two.X && one.Y == two.Y;
		bool checkNegativeX = (one.X - 1) == two.X && one.Y == two.Y;

		if (checkPositiveY || checkNegativeY || checkPositiveX || checkNegativeX) return true;
		return false;
	}


}
