using Godot;
using System;
using System.Collections.Generic;

public partial class MapEntities : Node
{

	public static TileMap map = null;
	public static PlayableCharacter selectedCharacter = null;

	public static List<PlayableCharacter> playableCharacters = new List<PlayableCharacter>();
	
	public static List<EnemyCharacter> enemyCharacters = new List<EnemyCharacter>();

	public static List<EnemyCharacter> detectedEnemies = new List<EnemyCharacter>();

	public static List<EnemyCharacter> targetedCharacters = new List<EnemyCharacter>();

	public static List<AttackMeta> attackMetas = new List<AttackMeta>();

	public static AttackMeta chosenAttack = null;

	public static Vector2I cursorCoords;
}
