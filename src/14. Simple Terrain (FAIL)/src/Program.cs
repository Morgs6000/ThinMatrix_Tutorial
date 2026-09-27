using System.Numerics;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using ThinMatrix_Tutorial.src.entities;
using ThinMatrix_Tutorial.src.models;
using ThinMatrix_Tutorial.src.renderEngine;
using ThinMatrix_Tutorial.src.shaders;
using ThinMatrix_Tutorial.src.terrains;
using ThinMatrix_Tutorial.src.textures;

namespace ThinMatrix_Tutorial.src;

public class Program
{
    private static Glfw _glfw = null!;
    private static GL _gl = null!;

    public static GL GL = null!;

    // configurações
    public const int SCR_WIDTH = 1280;
    public const int SCR_HEIGHT = 720;

    private static bool[] _keys = new bool[1024];
    private static bool[] _keysProcessed = new bool[1024];

    private static unsafe void Main(string[] args)
    {
        try
        {
        _glfw = Glfw.GetApi();

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

        _gl = GL.GetApi(_glfw.GetProcAddress);
        GL = _gl;

        _glfw.SetFramebufferSizeCallback(window, FramebufferSizeCallback);
        _glfw.SetKeyCallback(window, KeyCallback);

        // Ativar v-sync
        _glfw.SwapInterval(1);

        Loader loader = new Loader();

        RawModel model = OBJLoader.LoadObjModel("tree", loader);

        TexturedModel staticModel = new TexturedModel(model, new ModelTexture(loader.LoadTexture("tree")));

        Entitiy entitiy = new Entitiy(staticModel, new Vector3(0.0f, 0.0f, -25.0f), 0.0f, 0.0f, 0.0f, 1.0f);
        Light light = new Light(new Vector3(3000.0f, 2000.0f, 2000.0f), new Vector3(1.0f, 1.0f, 1.0f));

        Terrain terrain = new Terrain(0, 0, loader, new ModelTexture(loader.LoadTexture("grass")));
        Terrain terrain2 = new Terrain(1, 0, loader, new ModelTexture(loader.LoadTexture("grass")));

        Camera camera = new Camera();
        MasterRenderer renderer = new MasterRenderer();
        
        // loop de renderização
        // --------------------------------------------------
        while (!_glfw.WindowShouldClose(window))
        {
            // input
            // --------------------------------------------------
            ProcessInput(window);

            camera.Move();

            renderer.ProcessTerrain(terrain);
            renderer.ProcessTerrain(terrain2);

            renderer.ProcessEntitiy(entitiy);

            renderer.Render(light, camera);

            Array.Copy(_keys, _keysProcessed, _keys.Length);

            // glfw: troca os buffers e processa eventos de E/S (teclas pressionadas/liberadas, movimento do mouse, etc.)
            // --------------------------------------------------
            _glfw.SwapBuffers(window);
            _glfw.PollEvents();
        }

        renderer.CleanUp();
        loader.CleanUp();

        // glfw: encerra, liberando todos os recursos do GLFW alocados anteriormente.
        // --------------------------------------------------
        _glfw.Terminate();
        return;
        }
        catch (Exception ex)
        {
            Console.WriteLine("=== EXCEÇÃO ===");
            Console.WriteLine(ex.ToString());
        }

        Console.WriteLine("Pressione ENTER para sair...");
        Console.ReadLine();
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
