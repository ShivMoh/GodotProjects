extends State

func Enter():
	print("I am on shoot state")

func Physics_Update(delta: float):
	if Input.is_action_just_released("shoot"):
		Transitioned.emit(self, "start")

		
