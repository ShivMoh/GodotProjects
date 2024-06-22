using System.Collections.Generic;
using System.Numerics;
using Godot;
using Vector2 = Godot.Vector2;

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

		Vector2I currentCoord = tilemap.LocalToMap(selectedCharacter.GlobalPosition);

		foreach (Character character in enemies)
		{
			Vector2I enemyCoord = tilemap.LocalToMap(character.GlobalPosition);

			if (attack.attackTargetMeta.closeRange) {
				if (this.checkAdjacent(currentCoord, enemyCoord)) {
					characters.Add(character);
				}
			}

			if (this.isEnemyInRange(currentCoord, tilemap.LocalToMap(character.GlobalPosition), attack.attackTargetMeta.range)) {
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

	public void attackCharacter(Character selectedCharacter, Character target, AttackMeta chosenAttack) {
		int damage = selectedCharacter.getCharacterStats().strenth + chosenAttack.power;

		target.getCharacterStats().health -= damage;
		target.healthBar.takeDamage(damage);

		if (target.getCharacterStats().health <= 0) {

			if (target.GetType().Name is nameof(EnemyCharacter)) {
				MapEntities.enemyCharacters.Remove(target as EnemyCharacter);
			}

			if (target.GetType().Name is nameof(PlayableCharacter)) {
				MapEntities.playableCharacters.Remove(target as PlayableCharacter);
			}

			MapEntities.characters.Remove(target);
			target.QueueFree();
		}
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
