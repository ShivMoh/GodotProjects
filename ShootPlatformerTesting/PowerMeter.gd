extends TextureProgressBar
class_name  PowerMeter

	
func set_power_value(value: int):
	# note, could be wrong about this
	# but does not work with non integer values
	# perhaps you should double check this
	self.value += value
	
func get_power_value():
	return self.value;
	

