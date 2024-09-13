extends State
class_name WalkState

var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")
var speed : float = 10000.0

func enter():
	self.state_name = "WalkState"
	print("I am on Walk State")
	
func physics_update(delta):
	
	if self.player.is_on_floor() and Input.is_action_just_pressed("up"):
		emit_signal("state_change", "jump.gd", self, Vector2.ZERO)
	
	if not self.player.is_on_floor():
		self.player.velocity.y += gravity * delta
	
		
	var direction  = Input.get_axis("left", "right")
	
	if direction:
		self.player.velocity.x = speed * direction * delta
	else:
		self.player.velocity.x = move_toward(self.player.velocity.x, 0, speed)
		
	self.player.move_and_slide()

func random_wind():
	randomize()
	var random_wind = randi_range(0, 100)
	
	if random_wind == 1:
		print("I am randomizing something")
		emit_signal("state_change", "wind.gd", null)

	
