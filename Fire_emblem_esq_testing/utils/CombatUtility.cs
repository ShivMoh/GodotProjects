using System.Collections.Generic;
using System.Numerics;
using Godot;
using Vector2 = Godot.Vector2;

public partial class CombatUtility {
	
	private TileMap tilemap;
	private List<EnemyCharacter> enemies;
	private PlayableCharacter selectedCharacter;

	public CombatUtility(TileMap tilemap, List<EnemyCharacter> enemies, PlayableCharacter selectedCharacter) {
		this.tilemap = tilemap;
		this.enemies = enemies;
		this.selectedCharacter = selectedCharacter;
	}

	public void setSelectedCharacter(PlayableCharacter selectedCharacter) {
		this.selectedCharacter = selectedCharacter;
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
				this.highLightEnemy(tileUtility, enemyCoord);
			}

		}

		return characters;
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