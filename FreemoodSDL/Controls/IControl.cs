using System;

using FreeMoO.Service;
using FreeMoO.Collections;

using SdlDotNet.Input;

namespace FreeMoO.Controls
{
    public interface IControl
    {
        void Update(FreemooTimer pTimer);
        void Draw(FreemooTimer pTimer, FreeMoO.Service.GuiService pGuiService);
        void Release();
        void keyPressed(KeyboardEventArgs pKea);

        void keyReleased(KeyboardEventArgs pKea);

        void mousePressed(MouseButtonEventArgs pMbea);

        void mouseReleased(MouseButtonEventArgs pMbea);

        void mouseMoved(MouseMotionEventArgs pMbea);

        string Id { get; }

        ControlCollection Controls { get; }

        IControl ParentControl { get; set; }

        bool SafeRemove { get; set; }

        int X { get; set; }
        int Y { get; set; }
        int Width { get; set; }
        int Height { get; set; }

        bool Enabled { get; set; }
        bool Visible { get; set; }

        event EventHandler<MouseButtonEventArgs> mouseReleasedEvent;

        event EventHandler<MouseButtonEventArgs> mousePressedEvent;

        event EventHandler<MouseMotionEventArgs> mouseMovedEvent;

        event EventHandler<KeyboardEventArgs> keyPressedEvent;

        event EventHandler<KeyboardEventArgs> keyReleasedEvent;
    }
}
