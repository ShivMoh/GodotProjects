using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class EnemyTargetSelectionState : State {

	private Trait trait;

	private EnemySelectionUtility enemySelectionUtility;

	private TileUtility tileUtility;
	public override void enter()
	{
		GD.Print("I am on enemy target selection state");
		MapEntities.selectedCharacter = MapEntities.enemyCharacters.MaxBy(character => character.characterStat.speed);
		this.tileUtility = new TileUtility(MapEntities.map);

		this.tileUtility.highLight(MapEntities.map.LocalToMap(MapEntities.selectedCharacter.GlobalPosition), new Vector2I(0, 1));
		this.trait = MapEntities.selectedCharacter.characterStat.trait;
		this.enemySelectionUtility = new EnemySelectionUtility(MapEntities.selectedCharacter as EnemyCharacter, MapEntities.map);
	}

	public override void physicsUpdate(double _delta)
	{
		 switch (trait)
		{
			case Trait.NONE:
				GD.Print("NONE");
				break;
			case Trait.BRAZEN:
				GD.Print("BRAZEN");
				break;    
			case Trait.COWARD:
				GD.Print("COWARD");
				break;    
			case Trait.CUNNING:
				MapEntities.targetCandidates = enemySelectionUtility.findTargetsWithinRange(MapEntities.playableCharacters);
				MapEntities.closeRangeTargets = enemySelectionUtility.findTargetsWithinCloseRange(MapEntities.playableCharacters);
				MapEntities.attackMetas = enemySelectionUtility.getAvailableAttacks();
				EmitSignal(SignalName.StateChange, this, "EnemyAttackSelectionState");
				
				break;    
			default:
				break;
		}
	}


}
