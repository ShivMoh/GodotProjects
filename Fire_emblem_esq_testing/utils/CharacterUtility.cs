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
		// bool reachDestination = 	Math.Abs(selectedCharacter.Position.X - selectedCharacter.targetGlobalPosition.X) < 5.0f
		// 							&& Math.Abs(selectedCharacter.Position.Y - selectedCharacter.targetGlobalPosition.Y) < 5.0f;

		bool reachDestination = tilemap.LocalToMap(selectedCharacter.Position) == tilemap.LocalToMap(selectedCharacter.targetGlobalPosition);

		if (reachDestination && path.Count() != 0 && characterMoveIndex < path.Count()) {
			GD.Print("Is this running");
			Vector2I next_tile = path.ElementAt(characterMoveIndex); 

			selectedCharacter.targetGlobalPosition = tilemap.MapToLocal(next_tile);			
			characterMoveIndex++;

			// GD.Print("Updated GlobalPosition", selectedCharacter.targetGlobalPosition);
		} 

		Vector2 lastCoordinates = tilemap.MapToLocal(lastTile);

		bool reachLast = 	Math.Abs(selectedCharacter.Position.X - lastCoordinates.X) < 2.0f
									&& Math.Abs(selectedCharacter.Position.Y - lastCoordinates.Y) < 2.0f;
		if (reachLast) {
			selectedCharacter.move = false;
			characterMoveIndex = 0;
			this.clearPath(tilemap, ref path);
			GD.Print("Path is being cleared");
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

		var character = playableCharacters.FirstOrDefault<PlayableCharacter>(character => CharacterUtility.compareValues(character.Position, tilemap.MapToLocal(current), 2.0f), null);

		foreach (Character cha in playableCharacters)
		{
			GD.Print(cha.Position, tilemap.MapToLocal(current));
		}		
		if (character is not null) {
			if (selectedCharacter == null) {
				selectedCharacter = character;

				if (selectedCharacter.usedTurn == true) {
					selectedCharacter = null;
				}
			
			} else {
				selectedCharacter = null;
				this.clearPath(tilemap, ref path);
			}
		}

		if (selectedCharacter is not null) {
			selectedCharacter.targetGlobalPosition = selectedCharacter.Position;
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
		
		Vector2I tileGlobalPosition = MapEntities.map.LocalToMap(target.Position);

		Vector2I calculatedGlobalPosition = tileGlobalPosition - new Vector2I(numberOfTiles, 0);

		if (MapEntities.map.GetUsedCells(0).Contains(calculatedGlobalPosition)) return MapEntities.map.MapToLocal(calculatedGlobalPosition);

		calculatedGlobalPosition = tileGlobalPosition - new Vector2I(-numberOfTiles, 0);
		if (MapEntities.map.GetUsedCells(0).Contains(calculatedGlobalPosition)) return MapEntities.map.MapToLocal(calculatedGlobalPosition);

		calculatedGlobalPosition = tileGlobalPosition - new Vector2I(0, -numberOfTiles);
		if (MapEntities.map.GetUsedCells(0).Contains(calculatedGlobalPosition)) return MapEntities.map.MapToLocal(calculatedGlobalPosition);

		calculatedGlobalPosition = tileGlobalPosition - new Vector2I(0, numberOfTiles);
		if (MapEntities.map.GetUsedCells(0).Contains(calculatedGlobalPosition)) return MapEntities.map.MapToLocal(calculatedGlobalPosition);

		return Vector2.Zero;
	}



	
}
