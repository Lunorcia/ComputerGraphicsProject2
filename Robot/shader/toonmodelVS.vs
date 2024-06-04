#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;
layout (location = 3) in vec3 aOffset;

out vec3 FragPos;
out vec3 Normal;
out vec2 TexCoords;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

uniform mat4 normalMat;

void main()
{
    mat4 instanceModel = model;
    instanceModel[3] = instanceModel[3] + vec4(aOffset, 0.0);

    FragPos = vec3(instanceModel * vec4(aPos, 1.0));
    Normal = mat3(normalMat) * aNormal;
    TexCoords = aTexCoords;
    
    gl_Position = projection * view * vec4(FragPos, 1.0);
}