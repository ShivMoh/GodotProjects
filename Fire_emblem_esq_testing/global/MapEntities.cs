using Godot;
using System;
using System.Collections.Generic;

public partial class MapEntities : Node
{

	public static TileMap map = null;
	public static Character selectedCharacter = null;

	public static Camera2D mapCamera = null;
	public static List<PlayableCharacter> playableCharacters = new List<PlayableCharacter>();
	
	public static List<EnemyCharacter> enemyCharacters = new List<EnemyCharacter>();

	public static List<Character> detectedEnemies = new List<Character>();

	public static List<Character> characters = new List<Character>();

	public static List<Character> targetedCharacters = new List<Character>();

	public static List<List<Character>> targetCandidates = new List<List<Character>>();

	public static Dictionary<string, List<Vector2I>> targetSpotCandidates = new Dictionary<string, List<Vector2I>>(); 
	public static List<Character> closeRangeTargets = new List<Character>();
	public static List<AttackMeta> attackMetas = new List<AttackMeta>();

	public static List<string> entities = new List<string>();

	public static AttackMeta chosenAttack = null;

	public static int attackRange = 0;
	public static Vector2I cursorCoords;
	public static int count;

}
