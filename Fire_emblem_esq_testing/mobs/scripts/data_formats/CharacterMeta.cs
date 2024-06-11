using System.Collections.Generic;
using Godot;

public partial class CharacterMeta : Node {
    public Vector2I tileCoord;
    public string characterPath;
    public List<AttackMeta> attacks;

    public CharacterMeta(Vector2I tileCoord, string characterPath, List<AttackMeta> attacks) {
        this.tileCoord = tileCoord;
        this.characterPath = characterPath;
        this.attacks = attacks;
    }
};