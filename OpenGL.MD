# Manuel OpenGL

## Fonctionnement OpenGL

Le tampons de rendu est composé de 3 sous-tampons :

- ColorBuffer
  - Contient chaque pixels
- DephtBuffer
  - Décrit la profondeur de chaque pixel entre 0 et 1 (1 étant le plus éloigné)
- Stencil
  - Effets

Il existe deux tampons principaux, le front buffer et le back buffer.
A chaque actualisation de rendu, OpenGL dessine sur le Back Buffer en affichant le front buffer.
Les deux buffers sont alternés à chaque nouveau rendu.

# rendering pipeline
- Vertex shader : points
- Fragment shader : couleur