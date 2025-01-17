using Godot;
using System.Collections.Generic;
using System.Linq;
public partial class OpenWorldInitialState : State {
	static CharacterStat characterStat= new CharacterStat(
		name: "John",
		health: 100,
		strenth: 2,
		magic : 18,
		speed: 20,
		magicalDefence: 10,
		physicalDefence: 10,
		intelligence: 15,
		skill: 10,
		constition: 20,
		trait: Trait.CUNNING
	);

	static CharacterStat characterStat2 = new CharacterStat(
		name: "Broski",
		health: 120,
		strenth: 2,
		magic : 18,
		speed: 20,
		magicalDefence: 10,
		physicalDefence: 10,
		intelligence: 15,
		skill: 10,
		constition: 20,
		trait: Trait.CUNNING
	);

	CharacterMeta playableCharacterMeta = new CharacterMeta(
		tileCoord: new Vector2I(5, 5),
		characterPath : "res://mobs/scenes/playable_character.tscn",
		attacks : BasicAttackPack.basicMagicAttacksPacks["0001"],
		characterStat : characterStat.Clone() as CharacterStat
	);

	CharacterMeta[] enemyCharactersMeta = {
		new CharacterMeta(
			tileCoord: new Vector2I(7, 2),
			characterPath : "res://mobs/scenes/enemy_character.tscn",
			attacks : BasicAttackPack.basicMagicAttacksPacks["0001"],
			characterStat : characterStat.Clone() as CharacterStat
		  ),  
		new CharacterMeta(
			tileCoord: new Vector2I(7, 2),
			characterPath : "res://mobs/scenes/enemy_character.tscn",
			attacks : BasicAttackPack.basicMagicAttacksPacks["0001"],
			characterStat : characterStat2.Clone() as CharacterStat
		),
		new CharacterMeta(
			tileCoord: new Vector2I(7, 2),
			characterPath : "res://mobs/scenes/enemy_character.tscn",
			attacks : BasicAttackPack.basicMagicAttacksPacks["0001"],
			characterStat : characterStat.Clone() as CharacterStat
		),
		new CharacterMeta(
			tileCoord: new Vector2I(7, 2),
			characterPath : "res://mobs/scenes/enemy_character.tscn",
			attacks : BasicAttackPack.basicMagicAttacksPacks["0001"],
			characterStat : characterStat2.Clone() as CharacterStat
		  )
	 };
	// public TileMap tilemap;

	private SetupUtility setupUtility;

	public override void enter()
	{
		// MapEntities.map = tilemap;
		//GD.Print("I am on open world initial state, initial state, initial state");
		MapEntities.selectedCharacter = null;
		MapEntities.cursorCoords = new Vector2I(0, 0);
		
		// //GD.Print(BasicAttackPack.basicMagicAttacksPacks["0001"]);
		this.setupUtility = new SetupUtility(MapEntities.map);

		this.enemyCharactersMeta = this.setupUtility.setUpCharactersMetas("enemyCharacters", enemyCharactersMeta);
		this.setupUtility.setUpCharacters(this.enemyCharactersMeta, "enemyCharacters");

		loadSelectedCharacter();
	
		MapEntities.characters.AddRange(MapEntities.enemyCharacters);
		MapEntities.characters.AddRange(MapEntities.playableCharacters);
		
		//GD.Print(this.Name);

	}

	public override void physicsUpdate(double _delta)
	{
		EmitSignal(SignalName.StateChange, this, typeof(OpenWorldExploreState).ToString());
	}
	
	private void loadSelectedCharacter() {
		PlayableCharacter character = Character.instantiate(
			MapEntities.map.MapToLocal(playableCharacterMeta.tileCoord),
			playableCharacterMeta.characterPath
		) as PlayableCharacter;

		character.setAttacks(playableCharacterMeta.attacks);
		character.setCharacterStats(playableCharacterMeta.characterStat);	
		character.moveSteps = character.getCharacterStats().speed;
		MapEntities.map.GetNode("playableCharacters").AddChild(character);

		MapEntities.playableCharacters.Add(character);
	}

}
