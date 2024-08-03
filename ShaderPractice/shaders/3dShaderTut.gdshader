shader_type spatial;
render_mode specular_toon;

uniform sampler2D noise;
uniform float height_scale = 0.5;
uniform sampler2D normal_map;

varying vec2 tex_position;

// not advanced
float height_old(vec2 position, float time) {
	vec2 offset = 0.01 * cos(position + time);
	return texture(noise, (position / 2.0) - offset).x;
}


float wave(vec2 position) {
	 position += texture(noise, position / 2.0).x * 2.0 - 1.0;
	 vec2 wv = 1.0 - abs(sin(position));
	 return pow(1.0 - pow(wv.x * wv.y, 0.65), 2.0);
}

// without layers
float height_without_layers(vec2 position, float time) {
	float h = wave(position * 0.4);
	return h;
}

// with layers
float height(vec2 position, float time) {
	 time = time * 0.3;
	 float d = wave((position + time) * 0.4) * 0.3;

	 d += wave((position - time) * 0.3) * 0.3;
	 d += wave((position + time) * 0.5) * 0.2;
	 d += wave((position - time) * 0.6) * 0.2;
	 

	 return d;
}


vec2 tex_pos(vec2 position, float time, float scaling_factor) {
	 return position / 2.0 + 0.5 * TIME * scaling_factor;
}

vec3 calculate_normal(float k, float time, vec2 pos) {
	 return normalize(vec3(k - height(pos + vec2(0.1, 0.0), time), 0.1, k - height(pos + vec2(0.0, 0.1), time)));
}




void vertex() {
	 vec2 pos = VERTEX.xz;
	 tex_position = pos / 2.0 + 0.5;
	 float k = height(pos, TIME);
	 NORMAL = calculate_normal(k, TIME, pos);
	 VERTEX.y = k * 0.3;

}

void fragment() {
	// Called for every pixel the material is visible on.
	 float fresnel = sqrt(1.0 - dot(NORMAL, VIEW));
	 
	 RIM = 0.2;
	 METALLIC = 0.0;
	 ROUGHNESS = 0.01 * (1.0 - fresnel);
	 ALBEDO = vec3(0.01, 0.03, 0.05) + (0.1 * fresnel);
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
