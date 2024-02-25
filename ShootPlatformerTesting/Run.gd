extends PlayerState

func physics_update(delta: float) -> void:
	var direction = Input.get_axis("ui_left", "ui_right")
	if direction:
		player.velocity.x = direction * 200		
	else:
		player.velocity.x = move_toward(player.velocity.x, 0, 200)
		
	player.move_and_slide()
	
