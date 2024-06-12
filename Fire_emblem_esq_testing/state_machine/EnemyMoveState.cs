using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class EnemyMoveState : State {
	
	private EnemyCharacter currentActingEnemey;
	private PlayableCharacter target;

	private CharacterUtility characterUtility;

	private Vector2I currentTileCoords;


	List<Vector2I> path = new List<Vector2I>();
	public override void enter()
	{
		
		currentActingEnemey = MapEntities.enemyCharacters.MaxBy(enemy => enemy.getCharacterStats().speed);
		this.characterUtility = new CharacterUtility(MapEntities.map, currentActingEnemey, MapEntities.playableCharacters);

		GD.Print("I am on enemy move state");
		findTarget();
		calculatePathTowardsTarget();


	}

	public override void physicsUpdate(double _delta)
	{
		this.characterUtility.moveCharacter(ref this.path, currentTileCoords, this.path.Last());
	}
	
	private void findTarget() {
		// for now we're going to find the closest target
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

		int distanceX = targetTileCoords.X - actingEnemyTileCoords.X; 
		int distanceY = targetTileCoords.Y - actingEnemyTileCoords.Y;

		for (int y = 1; y < distanceY + 1; y++)
		{
			this.path.Add( new Vector2I(actingEnemyTileCoords.X, actingEnemyTileCoords.Y + y));
		}
		for (int x = 1; x < distanceX + 1; x++)
		{
			this.path.Add( new Vector2I(actingEnemyTileCoords.X + x, targetTileCoords.Y));
		}


	}
}
