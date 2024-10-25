extends Node


func _physics_process(delta):
	randomize()
	var random_number : int = randi_range(0, 10)
	
	if random_number > 5:
		print("A wind is being generated")
		var generatedWind : WindSomething = WindSomething.new()	
		generatedWind.global_position = self.global_position
		generatedWind.freeze = true
		add_child(generatedWind)
