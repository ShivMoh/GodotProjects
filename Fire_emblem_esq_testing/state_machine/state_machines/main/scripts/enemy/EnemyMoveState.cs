using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class EnemyMoveState : State {
	
	private EnemyCharacter currentActingEnemey;
	private Character target;

	private Vector2 targetGlobalPosition;

	private CharacterUtility characterUtility;

	private EnemySelectionUtility enemySelectionUtility;

	private Vector2I currentTileCoords;

	private TileUtility tileUtility;

	List<Vector2I> path = new List<Vector2I>();
	public override void enter()
	{
		//GD.Print(this.Name);

		(MapEntities.selectedCharacter.GetNode("CollisionShape2D") as CollisionShape2D).Disabled = true;
		
		currentActingEnemey = MapEntities.selectedCharacter as EnemyCharacter;

		this.characterUtility = new CharacterUtility(MapEntities.map, currentActingEnemey, MapEntities.playableCharacters);
		this.tileUtility = new TileUtility(MapEntities.map);
		this.enemySelectionUtility = new EnemySelectionUtility(MapEntities.selectedCharacter as EnemyCharacter, MapEntities.map);

		this.tileUtility.highLight(MapEntities.map.LocalToMap(currentActingEnemey.Position), new Vector2I(0, 0));
		// we need the distance from that target (number of tiles away)
		
		this.target = MapEntities.targetedCharacters.First();

		this.targetGlobalPosition = this.enemySelectionUtility.findAvailableSpotForTarget(
														this.target, 
														MapEntities.enemyCharacters, 
														MapEntities.chosenAttack.attackTargetMeta.range
													);
		//GD.Print("TARGET LOCATION", this.targetGlobalPosition);
		calculatePathTowardsTarget();
	
		this.currentActingEnemey.move = true;

	}


	public override void physicsUpdate(double _delta)
	{
		
		bool result = this.characterUtility.moveCharacter(ref this.path, currentTileCoords, this.path.Last());

		if (result == true) {
			EmitSignal(SignalName.StateChange, this, typeof(EnemyAttackState).ToString());
			MapEntities.targetedCharacters.Add(target);
		}
	}
	
	private void findTarget() {
		// for now we're going to find the closest target
		// in the future should incoorporate something perhaps based on character's intelligence
		// if a character is really smart, he might aim for weaker targets, if he's really strong,
		// he'll attack anyone regardless...hmmm, maybe i should have personality trait for this...
		float closestDistance = 10000;
		PlayableCharacter targetCandidate = null;
		foreach (PlayableCharacter playableCharacter in MapEntities.playableCharacters)
		{
			float distanceBetween = Mathf.Sqrt(
				Mathf.Pow(playableCharacter.Position.X - currentActingEnemey.Position.X, 2) +                 
				Mathf.Pow(playableCharacter.Position.Y - currentActingEnemey.Position.Y, 2) 
			);

			if (distanceBetween < closestDistance) {
				closestDistance = distanceBetween;
				targetCandidate = playableCharacter;
			}
		}

		this.target = targetCandidate;

	}

	private void calculatePathTowardsTarget() {
		Vector2I targetTileCoords = MapEntities.map.LocalToMap(this.targetGlobalPosition);
		Vector2I actingEnemyTileCoords = MapEntities.map.LocalToMap(currentActingEnemey.Position);
		this.currentTileCoords = actingEnemyTileCoords;

		path.Add(this.currentTileCoords);

		int distanceX = targetTileCoords.X - actingEnemyTileCoords.X; 
		int distanceY = targetTileCoords.Y - actingEnemyTileCoords.Y;

		int directionX = distanceX > 0 ? 1 : -1;	
		int directionY = distanceY > 0 ? 1 : -1;

		distanceX = Mathf.Abs(distanceX);
		distanceY = Mathf.Abs(distanceY);

		for (int y = 1; y < distanceY + 1; y++)
		{
			this.path.Add( new Vector2I(actingEnemyTileCoords.X, actingEnemyTileCoords.Y + (y * directionY)));
		}
		for (int x = 1; x < distanceX + 1; x++)
		{
			this.path.Add( new Vector2I(actingEnemyTileCoords.X + (x * directionX), targetTileCoords.Y));
		}
	}
}
