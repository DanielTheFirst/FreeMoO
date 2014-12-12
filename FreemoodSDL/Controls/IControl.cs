using System;

using FreemooSDL.Service;
using FreemooSDL.Collections;

using SdlDotNet.Input;

namespace FreemooSDL.Controls
{
    public interface IControl
    {
        void update(FreemooTimer pTimer);
        void draw(FreemooTimer pTimer, FreemooSDL.Service.GuiService pGuiService);
        void keyPressed(KeyboardEventArgs pKea);

        void keyReleased(KeyboardEventArgs pKea);

        void mousePressed(MouseButtonEventArgs pMbea);

        void mouseReleased(MouseButtonEventArgs pMbea);

        void mouseMoved(MouseMotionEventArgs pMbea);

        string Id { get; }

        ControlCollection Controls { get; }

        event EventHandler<MouseButtonEventArgs> mouseReleasedEvent;

        event EventHandler<MouseButtonEventArgs> mousePressedEvent;

        event EventHandler<MouseMotionEventArgs> mouseMovedEvent;

        event EventHandler<KeyboardEventArgs> keyPressedEvent;

        event EventHandler<KeyboardEventArgs> keyReleasedEvent;
    }
}
