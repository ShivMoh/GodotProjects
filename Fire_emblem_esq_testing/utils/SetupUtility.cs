using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class SetupUtility {

	private TileMap tileMap;

	public SetupUtility(TileMap tileMap) {
		this.tileMap = tileMap;
	}

	public CharacterMeta[] setUpCharactersMetas(string name, CharacterMeta[] characterMetas) {
		var node = this.tileMap.GetNode("enemyCharacters");
	
		/* NOTE: potentially look into weird issue with the following
			giving a null outputing. Chatgpt suggests that it is due to casting
			a type that is not node2d to type node2d
			
			this is confirmed by the statement "node is Node2d" which outputs false
			howevever, as can be verified in the editor, the type is indeed node2d */


		// Node2D node2d = this.tileMap.GetNode("enemyCharacters") as Node2D;

		// //GD.Print(node);
		// //GD.Print(node2d);
		// //GD.Print(node is Node2D);
		
		for(int i = 0; i < node.GetChildren().Count(); i++) {
			if (node.GetChild(i) is Character character) {
				characterMetas[i].tileCoord = tileMap.LocalToMap(character.ToLocal(character.GlobalPosition));
				characterMetas[i].characterPath = character.GetPath();
			}
		}

		return characterMetas;
	}   

	public List<Character> setUpCharacters(CharacterMeta[] characterMetas, string name) {
		var node = tileMap.GetNode(name);
		
		List<Character> characters = new List<Character>();
		
		for (int i = 0; i < characterMetas.Length; i++) {
			
			Character character = node.GetChild(i) as Character;

			character.Position = MapEntities.map.MapToLocal( 
				 MapEntities.map.LocalToMap(character.Position)
			);

			character.setAttacks(characterMetas[i].attacks);

			character.setCharacterStats(characterMetas[i].characterStat);

			character.moveSteps = character.getCharacterStats().speed;

			characters.Add(character);

			MapEntities.enemyCharacters.Add(character as EnemyCharacter);
		}

		return characters;
	}
}
