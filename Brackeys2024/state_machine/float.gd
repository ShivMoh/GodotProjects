extends State
class_name FloatState

func enter():
	state_name = "FloatState"
	print("I am on this state")
	
func physics_update(delta):
	self.state_change.emit("WalkState", self)
