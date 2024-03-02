extends RigidBody2D

func _ready():
	self.apply_central_force(Vector2(10000, 0))

func _physics_process(delta):
	pass
	
