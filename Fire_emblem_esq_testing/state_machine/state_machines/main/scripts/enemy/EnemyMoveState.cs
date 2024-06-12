using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class EnemyMoveState : State {
	
	private EnemyCharacter currentActingEnemey;
	private PlayableCharacter target;

	private CharacterUtility characterUtility;

	private Vector2I currentTileCoords;

	private TileUtility tileUtility;

	List<Vector2I> path = new List<Vector2I>();
	public override void enter()
	{
		
		currentActingEnemey = MapEntities.enemyCharacters.MaxBy(enemy => enemy.getCharacterStats().speed);

		this.characterUtility = new CharacterUtility(MapEntities.map, currentActingEnemey, MapEntities.playableCharacters);
		this.tileUtility = new TileUtility(MapEntities.map);

		this.tileUtility.highLight(MapEntities.map.LocalToMap(currentActingEnemey.GlobalPosition), new Vector2I(0, 1));
		GD.Print("I am on enemy move state", MapEntities.map.LocalToMap(currentActingEnemey.GlobalPosition));
		findTarget();
		calculatePathTowardsTarget();
		foreach (Vector2I pat in path)
		{
			GD.Print("PATH", pat);
		}
		this.currentActingEnemey.move = true;


	}

	public override void physicsUpdate(double _delta)
	{
		GD.Print(this.currentActingEnemey.GlobalPosition == this.target.GlobalPosition);
		bool result = this.characterUtility.moveCharacter(ref this.path, currentTileCoords, this.path.Last());

		if (result == true) {
			EmitSignal(SignalName.StateChange, this, "EnemyAttackState");
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
				Mathf.Pow(playableCharacter.GlobalPosition.X - currentActingEnemey.GlobalPosition.X, 2) +                 
				Mathf.Pow(playableCharacter.GlobalPosition.Y - currentActingEnemey.GlobalPosition.Y, 2) 
			);

			if (distanceBetween < closestDistance) {
				closestDistance = distanceBetween;
				targetCandidate = playableCharacter;
			}
		}

		this.target = targetCandidate;

	}

	private void calculatePathTowardsTarget() {
		Vector2I targetTileCoords = MapEntities.map.LocalToMap(target.GlobalPosition);
		Vector2I actingEnemyTileCoords = MapEntities.map.LocalToMap(currentActingEnemey.GlobalPosition);
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
		for (int x = 1; x < distanceX; x++)
		{
			this.path.Add( new Vector2I(actingEnemyTileCoords.X + (x * directionX), targetTileCoords.Y));
		}
	}
}
