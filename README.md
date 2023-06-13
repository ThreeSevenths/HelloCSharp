# HelloCSharp

This is a simple windows executable using the Excellent [Vanara.PInvoke](https://github.com/dahall/Vanara) libraries to manually create a window and window message loop. This follows along with [Hello, Assembly! Retrocoding the World's Smallest Windows App in x86 ASM](https://www.youtube.com/watch?v=b0zxIfJJLAY), except obviously not using assembly. It might be possible to code this in IL more succintly, but I am fine with a C# example.

This program creates a custom window class, creates a window, and runs a message pump for said window. The window contains the application title painted in the centre. It generally follows the structure of Dave's assembly program. I am aware of how the Main function essentially recreates the main func at this level 🤷.

MIT licensed, feel free to do with this as you wish, if it's of use to you!