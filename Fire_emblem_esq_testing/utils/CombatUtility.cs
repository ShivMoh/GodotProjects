using System.Collections.Generic;
using System.Numerics;
using Godot;
using Vector2 = Godot.Vector2;

public partial class CombatUtility {
	
	private TileMap tilemap;
	private List<EnemyCharacter> enemies;
	private PlayableCharacter selectedCharacter;

	public CombatUtility(TileMap tilemap, List<EnemyCharacter> enemies, Character selectedCharacter) {
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

	public List<EnemyCharacter> detectEnemy(TileUtility tileUtility) {

		List<EnemyCharacter> characters = new List<EnemyCharacter>();

		Vector2I currentCoord = tilemap.LocalToMap(selectedCharacter.GlobalPosition);

		foreach (EnemyCharacter character in enemies)
		{
			Vector2I enemyCoord = tilemap.LocalToMap(character.GlobalPosition);
			if (this.checkAdjacent(currentCoord, enemyCoord)) {
				characters.Add(character);
			}

		}

		return characters;
	}   

	public void attackCharacter(Character selectedCharacter, Character target, AttackMeta chosenAttack) {
		int damage = selectedCharacter.getCharacterStats().strenth + chosenAttack.power;

		target.getCharacterStats().health -= damage;
		target.healthBar.takeDamage(damage);

		if (target.getCharacterStats().health <= 0) {
			MapEntities.enemyCharacters.Remove(target as EnemyCharacter);
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
