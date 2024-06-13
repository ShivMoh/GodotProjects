using Godot;
using System;
using System.Collections.Generic;

public partial class MapEntities : Node
{

	public static TileMap map = null;
	public static Character selectedCharacter = null;

	public static List<PlayableCharacter> playableCharacters = new List<PlayableCharacter>();
	
	public static List<EnemyCharacter> enemyCharacters = new List<EnemyCharacter>();

	public static List<EnemyCharacter> detectedEnemies = new List<EnemyCharacter>();

	public static List<Character> targetedCharacters = new List<Character>();

	public static List<AttackMeta> attackMetas = new List<AttackMeta>();

	public static List<string> entities = new List<string>();

	public static AttackMeta chosenAttack = null;

	public static Vector2I cursorCoords;

	public static int playableCharacterCount = 0;

	public static int enemyCharacterCount = 0;

	public static int count;

}
