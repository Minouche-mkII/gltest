#version 330 core

// For each Vertex

layout (location = 0) in vec3 aPosition; // recover Vertex coordinates from first slot of VBO in aPosition

void main() {
    gl_Position = vec4(aPosition, 1.0); //
}