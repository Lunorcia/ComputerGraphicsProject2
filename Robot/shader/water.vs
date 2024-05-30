#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;

out vec4 clipSpace;
out vec2 texCoords;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

const float tiling = 1.0;

void main()
{
    clipSpace = projection * view * model * vec4(aPos, 1.0);
    texCoords = vec2(aPos.x / 2.0 + 0.5, aPos.z / 2.0 + 0.5) * tiling;
	gl_Position = clipSpace;
}