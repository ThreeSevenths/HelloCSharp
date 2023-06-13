using Vanara.PInvoke;
using static Vanara.PInvoke.Kernel32;
using static Vanara.PInvoke.User32;
using static Vanara.PInvoke.Gdi32;
using System.Runtime.InteropServices;
using Vanara;

namespace HelloCSharp
{
    internal class Program
    {
        static int Main(string[] args)
        {
            using var hInstance = GetModuleHandle(null);
            var lpCmdLine = GetCommandLine();
            GetStartupInfo(out var sui);
            ShowWindowCommand nCmdShow;
            if ((sui.dwFlags & STARTF.STARTF_USESHOWWINDOW) == STARTF.STARTF_USESHOWWINDOW)
            {
                nCmdShow = sui.ShowWindowCommand;
            }
            else
            {
                nCmdShow = ShowWindowCommand.SW_SHOWDEFAULT;
            }
            return WinMain(hInstance, null, lpCmdLine, nCmdShow);
        }

        const string WindowClassName = "CSharpWindowClass";
        const string AppName = "Hello CSharp";

        static int WinMain(SafeHINSTANCE hInstance, SafeHINSTANCE? hPrevInst, string lpCmdLine, ShowWindowCommand nCmdShow)
        {
            WNDCLASSEX wc = new();
            MSG msg;

            wc.cbSize = (uint)Marshal.SizeOf<WNDCLASSEX>();
            wc.style = WindowClassStyles.CS_HREDRAW | WindowClassStyles.CS_VREDRAW;
            wc.lpfnWndProc = WndProc;
            wc.cbClsExtra = 0;
            wc.cbWndExtra = 0;
            wc.hInstance = hInstance;
            wc.hbrBackground = SystemColorIndex.COLOR_WINDOW;
            wc.lpszMenuName = null;
            wc.lpszClassName = WindowClassName;

            var hIcon = LoadIcon(HINSTANCE.NULL, IDI_APPLICATION);

            if (hIcon.IsInvalid)
            {
                var error = GetLastError();
                return -1;
            }

            wc.hIcon = hIcon;
            wc.hIconSm = hIcon;

            var hCursor = LoadCursor(HINSTANCE.NULL, IDC_ARROW);

            if (hCursor.IsInvalid)
            {
                var error = GetLastError();
                return -1;
            }

            wc.hCursor = hCursor;

            if (RegisterClassEx(wc) == 0)
            {
                var error = GetLastError();
                return -1;
            }

            using var hWnd = CreateWindowEx(default, WindowClassName, AppName, WindowStyles.WS_OVERLAPPEDWINDOW | WindowStyles.WS_VISIBLE, CW_USEDEFAULT, CW_USEDEFAULT, 640, 480, HWND.NULL, HMENU.NULL, hInstance, nint.Zero);            

            if (hWnd.IsInvalid)
            {
                var error = GetLastError();
                return -1;
            }

            UpdateWindow(hWnd);

            int bRet;

            while((bRet = GetMessage(out msg, hWnd, 0, 0)) != 0)
            {
                if (bRet == -1)
                {
                    break;
                }
                var message = (WindowMessage)msg.message;
                TranslateMessage(msg);
                DispatchMessage(msg);
            }

            return msg.wParam.ToInt32();
        }

        static IntPtr WndProc(HWND hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            var wmmsg = (WindowMessage)msg;

            switch (wmmsg)
            {
                case WindowMessage.WM_DESTROY:
                    PostQuitMessage(0);
                    break;
                case WindowMessage.WM_PAINT:
                    Paint(hWnd);
                    break;
                default:
                    return DefWindowProc(hWnd, msg, wParam, lParam);
            }

            return 0;
        }

        static void Paint(HWND hWnd)
        {
            var hDC = BeginPaint(hWnd, out var ps);

            SetBkMode(hDC, BackgroundMode.TRANSPARENT);

            GetClientRect(hWnd, out var rect);

            DrawText(hDC, AppName, -1, rect, DrawTextFlags.DT_SINGLELINE | DrawTextFlags.DT_CENTER | DrawTextFlags.DT_VCENTER);

            EndPaint(hWnd, ps);
        }

    }
}