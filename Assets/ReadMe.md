**Devil-Themed Sword VFX Showcase**



**Unity Version**



\* Unity 6000.3.10f1

\* Universal Render Pipeline (URP)



**Project Overview**



This project showcases a devil-themed weapon inspired by premium weapon inspection effects commonly found in modern action and shooter games.



The visual direction focuses on molten demonic energy, combining animated lava, emissive glow, smoke, and dark aura effects to create a powerful and corrupted legendary weapon presentation.



\---



**Effects Breakdown**



**Custom Weapon Shader**



A custom Shader Graph was created to give the weapon a molten, demonic appearance.



Features:



\* Adjustable emissive glow intensity

\* Customizable emissive color

\* Support for custom UV-scrolling textures

\* Animated Voronoi noise used to simulate flowing lava beneath the weapon surface

\* Material parameters exposed for easy customization and reuse



The shader was designed to provide visual motion even when the weapon is idle.



\---



**Smoke / Energy Effect (VFX Graph)**



A VFX Graph effect surrounds the weapon with flowing energy.



Features:



\* Customizable color controls

\* Can be configured as smoke, energy, or fire

\* Fully loopable effect

\* Modular setup for future variations



\---



**Dark Aura Effect (Particle System)**



A Particle System with trail rendering was used to create dark energy streams flowing around the weapon.



Features:



\* Continuous looping effect

\* Trail-based motion

\* Lightweight particle setup

\* Adjustable colors and emission settings



\---



**Weapon Inspection System**



A custom C# weapon inspection script was implemented to improve presentation quality.



Features:



\* Mouse and touch drag rotation

\* Automatic idle rotation

\* Smooth rotational interpolation

\* Automatic visual-center pivot calculation using mesh bounds

\* Manual pivot offset support for fine tuning



This allows the weapon to rotate around its visual center instead of the imported pivot point.



\---



**Tools Used**



\* Unity URP

\* Shader Graph

\* VFX Graph

\* Particle System

\* C#



\---



**Performance Considerations**



The effects were designed to remain lightweight and suitable for real-time rendering.



Optimization measures:



\* Loopable modular effects

\* Lightweight particle counts

\* Reusable shader properties

\* Minimal CPU-side update logic

\* Configurable VFX parameters to reduce material duplication



**Test Scene Statistics**



Captured during runtime showcase:



\* \~199 FPS

\* Main Thread: \~5.0 ms

\* Render Thread: \~3.0 ms

\* 83 Batches

\* 35 SetPass Calls

\* \~30k Triangles



These values indicate that the scene remains performant while displaying all active visual effects.



\---



**Assets Provided**



\* Devil-themed sword model

\* Texture assets supplied with the assignment



\---



Shruti Kumari

Technical Artist



