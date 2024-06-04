#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 3) in vec3 aOffset;

uniform mat4 lightSpaceMatrix;
uniform mat4 model;

void main()
{
    mat4 instanceModel = model;
    instanceModel[3] = instanceModel[3] + vec4(aOffset, 0.0);
    gl_Position = lightSpaceMatrix * instanceModel * vec4(aPos, 1.0);
}