extends State

func Enter():
	print("I am ont enter")
	
func Update(_delta: float):
	Transitioned.emit(self, "Run")
