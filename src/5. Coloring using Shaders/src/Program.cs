using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using ThinMatrix_Tutorial.src.renderEngine;
using ThinMatrix_Tutorial.src.shaders;

namespace ThinMatrix_Tutorial.src;

public class Program
{
    private static Glfw _glfw = Glfw.GetApi();
    private static GL _gl = GL.GetApi(_glfw.GetProcAddress);

    public static GL GL = _gl;

    // configurações
    private const int SCR_WIDTH = 1280;
    private const int SCR_HEIGHT = 720;

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

        // Ativar v-sync
        _glfw.SwapInterval(1);

        Loader loader = new Loader();
        Renderer renderer = new Renderer();
        StaticShader shader = new StaticShader();

        float[] vertices =
        {
            -0.5f, -0.5f,  0.0f,
             0.5f, -0.5f,  0.0f,
             0.5f,  0.5f,  0.0f,
            -0.5f,  0.5f,  0.0f
        };

        uint[] indices =
        {
            0, 1, 2,
            0, 2, 3
        };

        RawModel model = loader.LoadToVao(vertices, indices);
        
        // loop de renderização
        // --------------------------------------------------
        while (!_glfw.WindowShouldClose(window))
        {
            // input
            // --------------------------------------------------
            ProcessInput(window);

            renderer.Prepare();

            shader.Start();            
            renderer.Render(model);
            shader.Stop();

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
}
