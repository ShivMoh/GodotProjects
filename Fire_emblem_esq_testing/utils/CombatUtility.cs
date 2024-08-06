using System.Collections.Generic;
using System.Numerics;
using Godot;
using Vector2 = Godot.Vector2;
using System.Linq;

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
		float distanceTo  = Mathf.Sqrt(Mathf.Pow(currentCoord.X - enemyCoord.X, 2) + Mathf.Pow(currentCoord.Y - enemyCoord.Y, 2));
		if (Mathf.Floor(distanceTo) <= attackRange ) return true;
		return false;
	}

	public  bool attackCharacter(Character selectedCharacter, Character target, AttackMeta chosenAttack) {

		int damage = this.attackSequence(selectedCharacter, target, chosenAttack);
		GD.Print("Base attack", (selectedCharacter.characterStat.magic + chosenAttack.power) - target.equipedAttack.defence);
		GD.Print(damage);
		
		target.characterStat.health -= damage;

		GD.Print("Character", target.characterStat.name, target.characterStat.health);

		selectedCharacter.equipedAttack = chosenAttack;

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
			GD.Print(target.healthBar.Value);
		}
		return false;
	 }
	
	private int attackSequence(Character attacker, Character target, AttackMeta chosenAttack) {
		int damage = 0;
	
		if (chosenAttack.attackType == AttackType.MAGICAL) {
			 damage = (attacker.characterStat.magic + chosenAttack.power) - target.equipedAttack.defence;
		  }

		if (chosenAttack.attackType == AttackType.PHYSICAL) {
			 damage = (attacker.characterStat.strenth + chosenAttack.power) - target.equipedAttack.defence;
		  }
		
		int currentAttackindexPlus = AttackMeta.attributeMap.IndexOf(chosenAttack.attackAttribute) + 1 == AttackMeta.attributeMap.Count() ? 0 : AttackMeta.attributeMap.IndexOf(chosenAttack.attackAttribute) + 1;	

			int currentAttackindexLess = AttackMeta.attributeMap.IndexOf(chosenAttack.attackAttribute) - 1 == -1 ?  AttackMeta.attributeMap.Count() - 1 : AttackMeta.attributeMap.IndexOf(chosenAttack.attackAttribute) - 1;	
		  if ( 		currentAttackindexPlus == 
					AttackMeta.attributeMap.IndexOf(target.equipedAttack.attackAttribute)) {
						 damage = damage * 2;
			   }

		  if ( 		currentAttackindexLess == 
					AttackMeta.attributeMap.IndexOf(target.equipedAttack.attackAttribute)) {
						 damage = damage/2;
			   }
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
