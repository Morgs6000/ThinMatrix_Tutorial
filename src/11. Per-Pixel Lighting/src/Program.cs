using System.Numerics;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using ThinMatrix_Tutorial.src.entities;
using ThinMatrix_Tutorial.src.models;
using ThinMatrix_Tutorial.src.renderEngine;
using ThinMatrix_Tutorial.src.shaders;
using ThinMatrix_Tutorial.src.textures;

namespace ThinMatrix_Tutorial.src;

public class Program
{
    private static Glfw _glfw = Glfw.GetApi();
    private static GL _gl = GL.GetApi(_glfw.GetProcAddress);

    public static GL GL = _gl;

    // configurações
    public const int SCR_WIDTH = 1280;
    public const int SCR_HEIGHT = 720;

    private static bool[] _keys = new bool[1024];
    private static bool[] _keysProcessed = new bool[1024];

    private static unsafe void Main(string[] args)
    {
        // glfw: inicializar e configurar
        // --------------------------------------------------
        _glfw.Init();
        _glfw.WindowHint(WindowHintInt.ContextVersionMajor, 3);
        _glfw.WindowHint(WindowHintInt.ContextVersionMinor, 3);
        _glfw.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);

        if (OperatingSystem.IsMacOS())
        {
            _glfw.WindowHint(WindowHintBool.OpenGLForwardCompat, true);
        }

        // criação da janela glfw
        // --------------------------------------------------
        WindowHandle* window = _glfw.CreateWindow(SCR_WIDTH, SCR_HEIGHT, "ThinMatrix Tutorial", null, null);

        if (window == null)
        {
            Console.WriteLine("Falha ao criar a janela GLFW");
            _glfw.Terminate();
            return;
        }

        if (OperatingSystem.IsWindows())
        {
            // Obtém o tamanho da janela passado para glfwCreateWindow
            _glfw.GetWindowSize(window, out int pWidth, out int pHeight);

            // Obtém a resolução do monitor principal
            VideoMode* videoMode = _glfw.GetVideoMode(_glfw.GetPrimaryMonitor());

            // Centralizar a janela
            _glfw.SetWindowPos(
                window,
                (videoMode->Width - pWidth) / 2,
                (videoMode->Height - pHeight) / 2
            );
        }

        _glfw.MakeContextCurrent(window);
        _glfw.SetFramebufferSizeCallback(window, FramebufferSizeCallback);
        _glfw.SetKeyCallback(window, KeyCallback);

        // Ativar v-sync
        _glfw.SwapInterval(1);

        Loader loader = new Loader();
        StaticShader shader = new StaticShader();
        Renderer renderer = new Renderer(shader);

        RawModel model = OBJLoader.LoadObjModel("dragon", loader);

        ModelTexture texture = new ModelTexture(loader.LoadTexture("white"));
        TexturedModel staticModel = new TexturedModel(model, texture);

        Entitiy entitiy = new Entitiy(staticModel, new Vector3(0.0f, 0.0f, -50.0f), 0.0f, 0.0f, 0.0f, 1.0f);
        Light light = new Light(new Vector3(0.0f, 0.0f, -20.0f), new Vector3(1.0f, 1.0f, 1.0f));

        Camera camera = new Camera();
        
        // loop de renderização
        // --------------------------------------------------
        while (!_glfw.WindowShouldClose(window))
        {
            // input
            // --------------------------------------------------
            ProcessInput(window);

            entitiy.IncreaseRotation(0.0f, 1.0f, 0.0f);

            camera.Move();

            renderer.Prepare();

            shader.Start();  
            shader.LoadLight(light);
            shader.LoadViewMatrix(camera);   

            renderer.Render(entitiy, shader);

            shader.Stop();

            Array.Copy(_keys, _keysProcessed, _keys.Length);

            // glfw: troca os buffers e processa eventos de E/S (teclas pressionadas/liberadas, movimento do mouse, etc.)
            // --------------------------------------------------
            _glfw.SwapBuffers(window);
            _glfw.PollEvents();
        }

        shader.CleanUp();
        loader.CleanUp();

        // glfw: encerra, liberando todos os recursos do GLFW alocados anteriormente.
        // --------------------------------------------------
        _glfw.Terminate();
        return;
    }

    // processar toda a entrada: consultar a GLFW para saber se teclas relevantes foram pressionadas ou liberadas neste quadro e reagir de acordo
    // --------------------------------------------------
    private static unsafe void ProcessInput(WindowHandle* window)
    {
        if (_glfw.GetKey(window, Keys.Escape) == (int)InputAction.Press)
        {
            _glfw.SetWindowShouldClose(window, true);
        }
    }

    // glfw: sempre que o tamanho da janela é alterado (pelo SO ou por redimensionamento do usuário), esta função de callback é executada
    // --------------------------------------------------
    private static unsafe void FramebufferSizeCallback(WindowHandle* window, int width, int height)
    {
        // certifique-se de que a viewport corresponda às novas dimensões da janela; observe que a largura e
        // a altura serão significativamente maiores do que as especificadas em telas Retina.
        _gl.Viewport(0, 0, (uint)width, (uint)height);
    }

    private static unsafe void KeyCallback(WindowHandle* window, Keys key, int scanCode, InputAction action, KeyModifiers mods)
    {
        if (action == InputAction.Press)
        {
            _keys[(int)key] = true;
        }
        else if (action == InputAction.Release)
        {
            _keys[(int)key] = false;
        }
    }

    public static bool IsKeyDown(Keys key)
    {
        return _keys[(int)key];
    }

    public static bool IsKeyPressed(Keys key)
    {
        return _keys[(int)key] && !_keysProcessed[(int)key];
    }

    public static bool IsKeyRelease(Keys key)
    {
        return !_keys[(int)key] && _keysProcessed[(int)key];
    }
}
