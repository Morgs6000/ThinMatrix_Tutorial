#version 400 core

in vec3 position;

out vec3 colour;

void main()
{
    gl_Position = vec4(position, 1.0f);

    colour = vec3(position.x + 0.5f, 0.0f, position.y + 0.5f);
}
