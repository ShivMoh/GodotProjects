using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class InitialState : State {

	static List<AttackMeta> attacksMeta = new List<AttackMeta>() {
		new AttackMeta(
			name: "Fire ball",
			power: 5,
			timesUsableUntilReset: 20,
			attackTargetMeta: new AttackTargetMeta(20, 1, radius: 5, areaOfEffect: true),
			attackAttribute: AttackAttribute.FIRE,
			effect: AttackEffect.BURN,
			attackType: AttackType.MAGICAL
		),
		new AttackMeta(
			name: "Ice ball",
			power: 3,
			timesUsableUntilReset: 20,
			attackTargetMeta: new AttackTargetMeta(2, 1, radius: 1),
			attackAttribute: AttackAttribute.WATER,
			effect: AttackEffect.FREEZE,
			attackType: AttackType.MAGICAL
		)
	};
	
	static List<AttackMeta> enemyAttacks = new List<AttackMeta>() {
		new AttackMeta(
			name: "Ice ball",
			power: 3,
			timesUsableUntilReset: 20,
			attackTargetMeta: new AttackTargetMeta(2, 1, radius: 1),
			attackAttribute: AttackAttribute.WATER,
			effect: AttackEffect.FREEZE
		)
	};

	static CharacterStat characterStat= new CharacterStat(
		name: "John",
		health: 50,
		strenth: 2,
		magic : 18,
		speed: 20,
		magicalDefence: 20,
		physicalDefence: 20,
		intelligence: 15,
		skill: 10,
		constition: 20,
		trait: Trait.CUNNING
	);

	static CharacterStat characterStat2 = new CharacterStat(
		name: "John",
		health: 100,
		strenth: 2,
		magic : 18,
		speed: 20,
		magicalDefence: 20,
		physicalDefence: 20,
		intelligence: 15,
		skill: 10,
		constition: 20,
		trait: Trait.CUNNING
	);

	CharacterMeta[] playableCharactersMeta = {
		new CharacterMeta(
			tileCoord: new Vector2I(5, 5),
			characterPath : "res://mobs/scenes/playable_character.tscn",
			attacks : BasicAttackPack.basicMagicAttacksPacks["0001"],
			characterStat : characterStat.Clone() as CharacterStat
		),
		new CharacterMeta(
			tileCoord: new Vector2I(0, 7),
			characterPath : "res://mobs/scenes/playable_character.tscn",
			attacks : attacksMeta,
			characterStat : characterStat.Clone() as CharacterStat

		),
		new CharacterMeta(
			tileCoord: new Vector2I(5, 0),
			characterPath : "res://mobs/scenes/playable_character.tscn",
			attacks : attacksMeta,
			characterStat : characterStat.Clone() as CharacterStat			
		),
	};

	CharacterMeta[] enemyCharactersMeta = {
		new CharacterMeta(
			tileCoord: Vector2I.Zero,
			characterPath : "",
			attacks : enemyAttacks,
			characterStat : characterStat.Clone() as CharacterStat
		),
		new CharacterMeta(
			tileCoord: Vector2I.Zero,
			characterPath : "",
			attacks : enemyAttacks,
			characterStat : characterStat.Clone() as CharacterStat
		)
	};

	// public TileMap tilemap;

	private SetupUtility setupUtility;

	public override void enter()
	{
		MapEntities.cursorCoords = new Vector2I(0, 0);
		
		loadCharacters();

		MapEntities.characters.AddRange(MapEntities.enemyCharacters);
		MapEntities.characters.AddRange(MapEntities.playableCharacters);
	}

	public override void physicsUpdate(double _delta)
	{
		EmitSignal(SignalName.StateChange, this, nameof(ExploreState));
	}
	private void loadCharacters() {

		Vector2 currentGlobalPosition = MapEntities.selectedCharacter.Position;
		MapEntities.selectedCharacter.QueueFree();
		MapEntities.selectedCharacter = null;

		spawnRelativeToGlobalPosition(currentGlobalPosition);
		
		for (int i = 0; i < playableCharactersMeta.Length; i++) {

			PlayableCharacter character = Character.instantiate(
				MapEntities.map.MapToLocal(playableCharactersMeta[i].tileCoord),
				playableCharactersMeta[i].characterPath
			) as PlayableCharacter;

			character.setAttacks(playableCharactersMeta[i].attacks);
			character.setCharacterStats(playableCharactersMeta[i].characterStat);
		
			character.moveSteps = character.getCharacterStats().speed;

			MapEntities.map.GetNode("playableCharacters").AddChild(character);
			MapEntities.playableCharacters.Add(character);
		}
	}

	private void spawnRelativeToGlobalPosition(Vector2 GlobalPosition) {
		foreach (CharacterMeta characterMeta in playableCharactersMeta)
		{
			characterMeta.tileCoord += MapEntities.map.LocalToMap(GlobalPosition);
		}
		
	}

	// private void loadEnemies() {
	// 	for (int i = 0; i < enemyCharactersMeta.Length; i++) {
	// 		EnemyCharacter character = Character.instantiate(
	// 			MapEntities.map.MapToLocal(enemyCharactersMeta[i].tileCoord),
	// 			enemyCharactersMeta[i].characterPath
	// 		) as EnemyCharacter;


	// 		character.setAttacks(enemyCharactersMeta[i].attacks);
	// 		// character.setCharacterStats(characterStat);

	// 		character.setCharacterStats(enemyCharactersMeta[i].characterStat);

	// 		character.moveSteps = character.getCharacterStats().speed;
		
	// 		MapEntities.map.GetNode("enemyCharacters").AddChild(character);
	// 		MapEntities.enemyCharacters.Add(character);
	// 	}
	// }
}
