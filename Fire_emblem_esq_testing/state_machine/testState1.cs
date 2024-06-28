using Godot;

public partial class testState1 : State {

	public override void enter() {
		// GD.Print("I am on test state 1");
	}

	public override void update(double _delta)
	{
		EmitSignal(SignalName.StateChange, this, 1);
	}
}
