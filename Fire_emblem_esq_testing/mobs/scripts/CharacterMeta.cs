using System.Collections.Generic;
using Godot;

public record CharacterMeta(
    Vector2I tileCoord,
    string characterPath,
    List<AttackMeta> attacks
);