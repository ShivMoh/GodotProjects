using System.Collections.Generic;
using System.Numerics;
using Godot;
using Vector2 = Godot.Vector2;

public partial class CombatUtil {

    public List<Character> detectEnemy(TileMap tilemap, List<Character> enemies, PlayableCharacter selectedCharacter) {

        List<Character> characters = new List<Character>();

        Vector2I currentCoord = tilemap.LocalToMap(selectedCharacter.GlobalPosition);

        foreach (Character character in enemies)
        {
            Vector2I enemyCoord = tilemap.LocalToMap(character.GlobalPosition);
            if (this.checkAdjacent(currentCoord, enemyCoord)) {
                characters.Add(character);
            }

        }

        return characters;
    }   

    private bool checkAdjacent(Vector2I one, Vector2I two) {
        bool checkPositiveY = (one.Y + 1) == two.Y;
        bool checkNegativeY = (one.Y - 1) == two.Y;
        bool checkPositiveX = (one.X + 1) == two.X;
        bool checkNegativeX = (one.X - 1) == two.X;

        if (checkPositiveY || checkNegativeY || checkPositiveX || checkNegativeX) return true;
        return false;
    }

    
}