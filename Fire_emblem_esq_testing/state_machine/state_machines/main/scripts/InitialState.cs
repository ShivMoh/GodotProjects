using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class InitialState : State {

	CharacterMeta[] playableCharactersMeta = {
		new CharacterMeta(
			tileCoord: new Vector2I(5, 5),
			characterPath : "res://mobs/scenes/playable_character.tscn",
			attacks : BasicAttackPack.basicMagicAttacksPacks["0001"],
			characterStat : CharacterStats.playableCharactersBase["ellie"].Clone() as CharacterStat
		),
		new CharacterMeta(
			tileCoord: new Vector2I(0, 7),
			characterPath : "res://mobs/scenes/playable_character.tscn",
			attacks : BasicAttackPack.basicMagicAttacksPacks["0002"],
			characterStat : CharacterStats.playableCharactersBase["teya"].Clone() as CharacterStat

		),
		new CharacterMeta(
			tileCoord: new Vector2I(5, 0),
			characterPath : "res://mobs/scenes/playable_character.tscn",
			attacks : BasicAttackPack.basicMagicAttacksPacks["0002"],
			characterStat : CharacterStats.playableCharactersBase["trent"].Clone() as CharacterStat
		),
	};

	CharacterMeta[] enemyCharactersMeta = {
		new CharacterMeta(
			tileCoord: Vector2I.Zero,
			characterPath : "",
			attacks : BasicAttackPack.basicMagicAttacksPacks["0001"],
			characterStat : CharacterStats.playableCharactersBase["trent"].Clone() as CharacterStat

		),
		new CharacterMeta(
			tileCoord: Vector2I.Zero,
			characterPath : "",
			attacks : BasicAttackPack.basicMagicAttacksPacks["0001"],
			characterStat : CharacterStats.playableCharactersBase["trent"].Clone() as CharacterStat

		)
	};

	private SetupUtility setupUtility;

	public override void enter()
	{
		MapEntities.cursorCoords = MapEntities.map.LocalToMap(MapEntities.selectedCharacter.Position);
		
		loadCharacters();

		// MapEntities.characters.AddRange(MapEntities.enemyCharacters);
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

		//GD.Print("Initial Setup", MapEntities.characters.Count(), MapEntities.playableCharacters.Count(), MapEntities.enemyCharacters.Count());

		spawnRelativeToGlobalPosition(currentGlobalPosition);
		
		for (int i = 0; i < playableCharactersMeta.Length; i++) {

			PlayableCharacter character = Character.instantiate(
				MapEntities.map.MapToLocal(playableCharactersMeta[i].tileCoord),
				playableCharactersMeta[i].characterPath
			) as PlayableCharacter;

			character.setAttacks(playableCharactersMeta[i].attacks);
			character.setCharacterStats(playableCharactersMeta[i].characterStat);
		
			character.moveSteps = character.getCharacterStats().speed;
			character.updateText();

			MapEntities.map.GetNode("playableCharacters").AddChild(character);
			MapEntities.playableCharacters.Add(character);
			// MapEntities.characters.Add(character);
		}

		//GD.Print("After spawning", MapEntities.characters.Count(), MapEntities.playableCharacters.Count(), MapEntities.enemyCharacters.Count());

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
