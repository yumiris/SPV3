# POST PROCESSING SHADER DESCRIPTION (SPV3.2)
--------------------------------------------
## Ambient Occlusion
**Technique used - MXAO 2.0 (Marty McFly's Ambient Obscurance)**
It is a shading/rendering technique used to calculate how each point in the scene is exposed to ambient light. Depending on the geometry the algorithm is used to apply dynamic shadows on occluded surfaces.

## Depth of Field
**Technique used - ADOF (Advanced Depth of Field)**
Effect in which objects within some range of distances in a scene appear in focus, and objects nearer or farther than this range appear out of focus or blurred out. Just like any camera or your eye, ADOF enables autofocusing.

## Dynamic Flares
**Technique used - Dynamic Pseudo Lens Flares**
This effect shows lensflares automatically based bright spots in the scene.

## Lensdirt
**Technique used - Bloom & Lensdirt**
Effect which shows subtle dirt / scratch effects on the screen.

## Eye Adaptation
**Technique used - Adaptive Contrast Curve**
This effect automatically adjust the scene exposure based on ambient light. Unlike commonly used implementations of this effect, the adaptation is instant.

## Antialiasing
**Technique used - Subpixel Morphological Antialiasing**
It's a technique used to smooth otherwise jagged lines or textures by blending the color of an edge with the color of pixels around it. The result should be a more pleasing and realistic appearance, depending on the intensity of the effect.

##Debanding
**Technique used - Debanding**
Effect which eliminated banding artifacts (sort of band like anomalies that appear in otherwise smooth gradients of colors, due to small color space)