using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class EnemyTargetSelectionState : State {

	private Trait trait;

	private EnemySelectionUtility enemySelectionUtility;

	private TileUtility tileUtility;

	private bool turnOver = false;
	public override void enter()
	{
		//GD.Print("I am on enemy target selection state");

		int maxSpeed = MapEntities.enemyCharacters.Max(character => character.characterStat.speed);

		MapEntities.selectedCharacter = MapEntities.enemyCharacters.FirstOrDefault(character => { return character.characterStat.speed == maxSpeed && character.usedTurn  == false; }, null);

		this.tileUtility = new TileUtility(MapEntities.map);
		this.tileUtility.highLight(MapEntities.map.LocalToMap(MapEntities.selectedCharacter.GlobalPosition), new Vector2I(0, 0));
		this.trait = MapEntities.selectedCharacter.characterStat.trait;
		this.enemySelectionUtility = new EnemySelectionUtility(MapEntities.selectedCharacter as EnemyCharacter, MapEntities.map);
	}

	public override void physicsUpdate(double _delta)
	{
		if (this.turnOver) {
			EmitSignal(SignalName.StateChange, this, typeof(FinalState).ToString());
		} else {
			switch (trait)
			{
				case Trait.NONE:
					//GD.Print("NONE");
					break;
				case Trait.BRAZEN:
					//GD.Print("BRAZEN");
					break;    
				case Trait.COWARD:
					//GD.Print("COWARD");
					break;    
				case Trait.CUNNING:
					MapEntities.targetCandidates = enemySelectionUtility.findTargetsWithinRange(MapEntities.playableCharacters);
					MapEntities.targetSpotCandidates = enemySelectionUtility.getSpots();
					
					MapEntities.closeRangeTargets = enemySelectionUtility.findTargetsWithinCloseRange(MapEntities.playableCharacters);
					MapEntities.attackMetas = enemySelectionUtility.getAvailableAttacks();
					
					//GD.Print("Is this running");
					EmitSignal(SignalName.StateChange, this, nameof(EnemyAttackSelectionState));
					
					break;    
				default:
					break;
			}
		}
		
	}


}
