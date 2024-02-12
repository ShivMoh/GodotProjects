using Godot;
using System;

public partial class state : Node
{
    [Signal]
    public delegate void TransitionedEventHandler(state state, string newStateName);
    public virtual void Enter() {}
    public virtual void Exit() {}
    public virtual void Update(double _delta) {}
    public virtual void PhysicsUpdate(double _delta) {}
}