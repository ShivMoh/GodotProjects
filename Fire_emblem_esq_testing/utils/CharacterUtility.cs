using Godot;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using Vector2 = Godot.Vector2;
public partial class CharacterUtility {

	// private Vector2I lastTile;

	private TileMap tilemap;
	private Character selectedCharacter;

	private int characterMoveIndex;
	List<PlayableCharacter> playableCharacters; 

	public CharacterUtility(TileMap tilemap, Character selectedCharacter, List<PlayableCharacter> playableCharacters) {
		this.tilemap = tilemap;
		this.selectedCharacter = selectedCharacter;
		this.playableCharacters = playableCharacters;
		this.characterMoveIndex = 0;
	}

	public void setSelectedCharacter(Character selectedCharacter) {
		this.selectedCharacter = selectedCharacter;
	}

	public bool moveCharacter( 
								ref List<Vector2I> path, 
								Vector2I current, 
								Vector2I lastTile
							) {

		bool reachDestination = 	Math.Abs(selectedCharacter.GlobalPosition.X - selectedCharacter.targetPosition.X) < 2.0f
									&& Math.Abs(selectedCharacter.GlobalPosition.Y - selectedCharacter.targetPosition.Y) < 2.0f;
	 
		if (reachDestination && path.Count() != 0 && characterMoveIndex < path.Count()) {
			Vector2I next_tile = path.ElementAt(characterMoveIndex); 
			
			selectedCharacter.targetPosition = tilemap.MapToLocal(next_tile);			
			// GD.Print("Updated position", selectedCharacter.targetPosition);
			characterMoveIndex++;
		} 

		Vector2 lastCoordinates = tilemap.MapToLocal(lastTile);

		bool reachLast = 	Math.Abs(selectedCharacter.GlobalPosition.X - lastCoordinates.X) < 2.0f
									&& Math.Abs(selectedCharacter.GlobalPosition.Y - lastCoordinates.Y) < 2.0f;
		if (reachLast) {
			selectedCharacter.move = false;
			characterMoveIndex = 0;
			this.clearPath(tilemap, ref path);
			return true;
		}

		return false;
	}

	public void clearPath(TileMap tilemap, ref List<Vector2I> path) {	
		// tilemap.ClearLayer(1);
		path.Clear();
		// TileUtil.drawCursor(tilemap, current);
	}


	public static bool compareValues(Vector2 one, Vector2 two, float maxAllowedDistance) {
		float distanceX = Math.Abs(one.X - two.X);
		float distanceY = Math.Abs(one.Y - two.Y);

		return distanceX < maxAllowedDistance && distanceY < maxAllowedDistance;
	}


	public PlayableCharacter selectCharacter( Vector2I current, ref List<Vector2I> path) {
		var character = playableCharacters.FirstOrDefault<PlayableCharacter>(character => CharacterUtility.compareValues(character.GlobalPosition, tilemap.MapToLocal(current), 2.0f), null);
		
		if (character is not null) {
			if (selectedCharacter == null) {
				selectedCharacter = character;

				if (selectedCharacter.usedTurn == true) {
					selectedCharacter = null;
				}
			
				return selectedCharacter as PlayableCharacter;
			} else {
				selectedCharacter = null;
				this.clearPath(tilemap, ref path);
				return null;
			}
		} 
		
		return selectedCharacter as PlayableCharacter;
		
	}

	public PlayableCharacter unselectCharacter(ref List<Vector2I> path)	 {
		this.selectedCharacter = null;
		this.clearPath(tilemap, ref path);
		return this.selectedCharacter as PlayableCharacter;
	}



	public Vector2 calculateTileFromMatchingDistance(Character target) {
		int numberOfTiles = MapEntities.attackRange + 1;
		
		Vector2I tilePosition = MapEntities.map.LocalToMap(target.GlobalPosition);

		Vector2I calculatedPosition = tilePosition - new Vector2I(numberOfTiles, 0);
		if (MapEntities.map.GetUsedCells(0).Contains(calculatedPosition)) return MapEntities.map.MapToLocal(calculatedPosition);

		calculatedPosition = tilePosition - new Vector2I(-numberOfTiles, 0);
		if (MapEntities.map.GetUsedCells(0).Contains(calculatedPosition)) return MapEntities.map.MapToLocal(calculatedPosition);

		calculatedPosition = tilePosition - new Vector2I(0, -numberOfTiles);
		if (MapEntities.map.GetUsedCells(0).Contains(calculatedPosition)) return MapEntities.map.MapToLocal(calculatedPosition);

		calculatedPosition = tilePosition - new Vector2I(0, numberOfTiles);
		if (MapEntities.map.GetUsedCells(0).Contains(calculatedPosition)) return MapEntities.map.MapToLocal(calculatedPosition);

		return Vector2.Zero;
	}



	
}
