#version 330 core
in vec4 FragPos;
in vec4 PrevFragPos;

out vec4 FragColor;

void main()
{
    vec2 currNDC = (FragPos.xy / FragPos.w) / 2;
    vec2 prevNDC = (PrevFragPos.xy / PrevFragPos.w) / 2;
    vec2 velocity = currNDC - prevNDC;
    FragColor = vec4(velocity, 0.0, 1.0);
}