extends State
class_name WindState

func enter():
	print("I am on wind state")
	
func physics_update(delta):
		
	if direction.x == 1:
		print("this is running right?")
		player.velocity.x = -200000 * delta
	if direction.x == -1:
		print("this is running right? 2")
		player.velocity.x = 20000 * delta
	if direction.y == 1:
		print("this is running right? 3")
		player.velocity.y = -20000 * delta
	if direction.y == -1:
		print("this is running right? 4") 
		player.velocity.y = 20000 * delta
	
	player.move_and_slide()

	emit_signal("state_change", "walk.gd", self, Vector2.ZERO)
