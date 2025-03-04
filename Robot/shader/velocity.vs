#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;
layout (location = 3) in vec3 aOffset;

out vec4 FragPos;
out vec4 PrevFragPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 prevModel;
uniform mat4 prevView;
uniform mat4 prevProjection;

uniform bool isInstance;

void main()
{
    mat4 modelMatrix = model;
    mat4 prevModelMatrix = prevModel;
    if(isInstance)
    {
        modelMatrix[3] = modelMatrix[3] + vec4(aOffset, 0.0);
        prevModelMatrix[3] = prevModelMatrix[3] + vec4(aOffset, 0.0);
    }

    FragPos = modelMatrix * vec4(aPos, 1.0);
    PrevFragPos = prevModelMatrix * vec4(aPos, 1.0);

    gl_Position = projection * view * FragPos;
}