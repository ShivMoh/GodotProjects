using Godot;

public partial class testState2 : State {

	public override void enter() {
		// GD.Print("I am on test state 2");
	}

	public override void update(double _delta)
	{
		EmitSignal(SignalName.StateChange, this, 1);
	}
}
