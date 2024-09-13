extends State
class_name JumpState

const jump_force : float = -400
func enter():
	self.state_name = "JumpState"

func physics_update(delta):
	player.velocity.y = jump_force

	emit_signal("state_change", "walk.gd", self, Vector2.ZERO)
