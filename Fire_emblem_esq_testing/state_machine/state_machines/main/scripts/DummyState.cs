using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Godot;

public partial class DummyState : State {

	public override void enter()
	{
        GD.Print("I am on dummy state");
	}

}
