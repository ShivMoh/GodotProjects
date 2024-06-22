using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class InitialState : State {

	static List<AttackMeta> attacksMeta = new List<AttackMeta>() {
		new AttackMeta(
			name: "Fire ball",
			power: 5,
			physical : false,
			timesUsableUntilReset: 20,
			attackTargetMeta: new AttackTargetMeta(20, 2, radius: 5),
			attackAttribute: AttackAttribute.FIRE,
			effect: AttackEffect.BURN
		),
		new AttackMeta(
			name: "Ice ball",
			power: 3,
			physical: false,
			timesUsableUntilReset: 20,
			attackTargetMeta: new AttackTargetMeta(2, 2, radius: 1),
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
			attacks : attacksMeta
		),
		new CharacterMeta(
			tileCoord: new Vector2I(0, 7),
			characterPath : "res://mobs/scenes/playable_character.tscn",
			attacks : attacksMeta
		),
		new CharacterMeta(
			tileCoord: new Vector2I(5, 0),
			characterPath : "res://mobs/scenes/playable_character.tscn",
			attacks : attacksMeta			
		),
	};

	CharacterMeta[] enemyCharactersMeta = {
		new CharacterMeta(
			tileCoord: new Vector2I(7, 2),
			characterPath : "res://mobs/scenes/enemy_character.tscn",
			attacks : attacksMeta
		),
		new CharacterMeta(
			tileCoord: new Vector2I(6, 1),
			characterPath : "res://mobs/scenes/enemy_character.tscn",
			attacks : attacksMeta
		),
		new CharacterMeta(
			tileCoord: new Vector2I(6, 3),
			characterPath : "res://mobs/scenes/enemy_character.tscn",
			attacks : attacksMeta
		),
		new CharacterMeta(
			tileCoord: new Vector2I(3, 7),
			characterPath : "res://mobs/scenes/enemy_character.tscn",
			attacks : attacksMeta
		),
		new CharacterMeta(
			tileCoord: new Vector2I(3, 9),
			characterPath : "res://mobs/scenes/enemy_character.tscn",
			attacks : attacksMeta
		),
	};


	[Export]
	TileMap tilemap;

	public override void enter()
	{
		MapEntities.map = tilemap;
		MapEntities.selectedCharacter = null;
		MapEntities.cursorCoords = new Vector2I(0, 0);
		
		loadCharacters();
		loadEnemies();

		MapEntities.playableCharacterCount = MapEntities.playableCharacters.Count();
		MapEntities.enemyCharacterCount = MapEntities.enemyCharacters.Count();

		MapEntities.characters.AddRange(MapEntities.enemyCharacters);
		MapEntities.characters.AddRange(MapEntities.playableCharacters);
	}

	public override void physicsUpdate(double _delta)
	{
		EmitSignal(SignalName.StateChange, this, "ExploreState");
	}

	private void loadCharacters() {
		for (int i = 0; i < playableCharactersMeta.Length; i++) {
			PlayableCharacter character = Character.instantiate(
				MapEntities.map.MapToLocal(playableCharactersMeta[i].tileCoord),
				playableCharactersMeta[i].characterPath
			) as PlayableCharacter;

			character.setAttacks(playableCharactersMeta[i].attacks);
			character.setCharacterStats(characterStat);

		
			character.moveSteps = character.getCharacterStats().speed;
			MapEntities.map.GetNode("playableCharacters").AddChild(character);
			MapEntities.playableCharacters.Add(character);
		}
	}

	private void loadEnemies() {
		for (int i = 0; i < enemyCharactersMeta.Length; i++) {
			EnemyCharacter character = Character.instantiate(
				MapEntities.map.MapToLocal(enemyCharactersMeta[i].tileCoord),
				enemyCharactersMeta[i].characterPath
			) as EnemyCharacter;

			character.setAttacks(enemyCharactersMeta[i].attacks);
			// character.setCharacterStats(characterStat);

			if (i > 1) {
				character.setCharacterStats(characterStat2);
				GD.Print("greater than 1");
			} else {
				character.setCharacterStats(characterStat);
			}

			character.moveSteps = character.getCharacterStats().speed;
		
			MapEntities.map.GetNode("enemyCharacters").AddChild(character);
			MapEntities.enemyCharacters.Add(character);
		}
	}
}
