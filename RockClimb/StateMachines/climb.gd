extends State

func Enter(_args = []):
	player.velocity = Vector2.ZERO
	print("I am on climb")
	
func Update(delta: float):
	pass
	
func Physics_Update(delta: float):
	
	if Input.is_action_just_pressed("ui_left"):
		player.velocity.x = -200
		Transitioned.emit(self, "start")
	
	if Input.is_action_just_pressed("ui_right"):
		player.velocity.x = 200
		Transitioned.emit(self, "start")
		
	if Input.is_action_pressed("ui_up"):
		player.velocity.y = -100
	else:
		player.velocity.y = 0
	
		
	player.move_and_slide()
		
		
	

	
