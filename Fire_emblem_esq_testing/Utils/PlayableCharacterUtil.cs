using Godot;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using Vector2 = Godot.Vector2;
public partial class PlayableCharacterUtil {

    // private Vector2I lastTile;

    public bool moveCharacter(
                                TileMap tilemap, 
                                PlayableCharacter selectedCharacter, 
                                ref List<Vector2I> path, 
                                Vector2I current, 
                                ref int characterMoveIndex,
                                Vector2I lastTile
                            ) {

		bool reachDestination = 	Math.Abs(selectedCharacter.GlobalPosition.X - selectedCharacter.targetPosition.X) < 2.0f
									&& Math.Abs(selectedCharacter.GlobalPosition.Y - selectedCharacter.targetPosition.Y) < 2.0f;
	 
		if (reachDestination && path.Count() != 0 && characterMoveIndex < path.Count()) {
			Vector2I next_tile = path.ElementAt(characterMoveIndex); 
			
			selectedCharacter.targetPosition = tilemap.MapToLocal(next_tile);			
			characterMoveIndex++;
		} 

		Vector2 lastCoordinates = tilemap.MapToLocal(lastTile);

		bool reachLast = 	Math.Abs(selectedCharacter.GlobalPosition.X - lastCoordinates.X) < 2.0f
									&& Math.Abs(selectedCharacter.GlobalPosition.Y - lastCoordinates.Y) < 2.0f;
		if (reachLast) {
			selectedCharacter.move = false;
			characterMoveIndex = 0;
			this.clearPath(tilemap, ref path, current);
            return true;
		}

        return false;
	}

    public void clearPath(TileMap tilemap, ref List<Vector2I> path, Vector2I current) {	
		tilemap.ClearLayer(3);
		path.Clear();
		TileUtil.drawCursor(tilemap, current);
	}


	public bool compareValues(Vector2 one, Vector2 two, float maxAllowedDistance) {
		float distanceX = Math.Abs(one.X - two.X);
		float distanceY = Math.Abs(one.Y - two.Y);
		return distanceX < maxAllowedDistance && distanceY < maxAllowedDistance;
	}


    public PlayableCharacter selectCharacter(   TileMap tilemap, 
                                    List<PlayableCharacter> playableCharacters, 
                                    PlayableCharacter selectedCharacter, 
                                    Vector2I current,
                                    ref List<Vector2I> path
                                ) {
		var character = playableCharacters.FirstOrDefault<PlayableCharacter>(character => this.compareValues(character.GlobalPosition, tilemap.MapToLocal(current), 2.0f), null);
		if (character is not null) {
			if (selectedCharacter == null) {
				selectedCharacter = character;
                return character;
			} else {
				selectedCharacter = null;
				this.clearPath(tilemap, ref path, current);
                return null;
			}
		} 

        return selectedCharacter;
        
	}



	
}