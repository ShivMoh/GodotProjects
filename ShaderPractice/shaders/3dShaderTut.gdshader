shader_type spatial;

uniform sampler2D noise;
uniform float height_scale;

void vertex() {
	// Called for every vertex the material is visible on.

	
	// VERTEX.y += cos(VERTEX.x) * sin(VERTEX.z);

	// VERTEX.y += cos(VERTEX.x * height_scale) * sin(VERTEX.z * height_scale);
	float height = texture(noise, VERTEX.xz / 2.0 + 0.5).x;
	VERTEX.y += height * height_scale;
}

void fragment() {
	// Called for every pixel the material is visible on.
	COLOR = vec4(1.0, 0.0, 0.0, 1.0);
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
