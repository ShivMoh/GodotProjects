extends RigidBody2D

const ropePiece = preload("res://ropePiece.tscn")

var origin: Vector2 = Vector2.ZERO

func _ready():
	origin = self.position
	self.apply_force(Vector2(30000, -30000), self.position)
	

func _physics_process(delta):
	# self.add_constant_force(Vector2(10, 0), self.position)
#	self.add_constant_force(Vector2(10, 0), self.position)
	pass



func addImpulse():
	var car2 = get_parent().get_child(1)
	car2.apply_impulse(Vector2(10, 0), self.position)
	

