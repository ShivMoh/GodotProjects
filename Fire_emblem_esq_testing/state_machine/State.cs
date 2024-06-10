using Godot;

public partial class State : Node2D {
    
    [Signal]
    public delegate void StateChangeEventHandler(State state, int stateID);

    // [Signal]
    // public delegate void ShareCharacterEventHandler(PlayableCharacter selectedCharacter);
    public virtual void enter() {}

    public virtual void exit() {}

    public virtual void update(double _delta) {}

    public virtual void physicsUpdate(double _delta) {}

}