extends RigidBody2D

class_name RopeEnd


var marker = Vector2.ZERO


func _ready():
	marker = get_node("Marker").position
	
#
#
#func cal_vector_x():
#	return cos(deg_to_rad(angle) ) * lenghtOfRope
#
#func cal_vector_y():
#	return sin(deg_to_rad(angle)) * lenghtOfRope
#
#func cal_force() -> Vector2: 
#	return Vector2(cal_vector_x(), cal_vector_y()) * 500
#
#func _on_timer_timeout():
#	self.apply_force(cal_force(), self.position)
