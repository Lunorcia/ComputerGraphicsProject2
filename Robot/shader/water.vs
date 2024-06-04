#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;

out vec4 clipSpace;
out vec2 texCoords;
out vec3 FragPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

const float tiling = 3.0;

void main()
{
    FragPos = vec3(model * vec4(aPos, 1.0));
    clipSpace = projection * view * model * vec4(aPos, 1.0);
    texCoords = aTexCoords * tiling;
    //texCoords = vec2(aPos.x / 2 + 5, aPos.z / 2 + 5) * tiling;
	gl_Position = clipSpace;
}