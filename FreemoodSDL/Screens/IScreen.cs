using System;

using FreemooSDL.Service;

using SdlDotNet.Input;

namespace FreemooSDL.Screens
{
    public interface IScreen
    {
        FreemooGame Game { get;}

        void start();

        void stop();

        void pause();

        void resume();

        /*void update(FreemooTimer pTimer);

        void draw(FreemooTimer pTimer);

        void keyPressed(KeyboardEventArgs pKea);

        void keyReleased(KeyboardEventArgs pKea);

        void mousePressed(MouseButtonEventArgs pMbea);

        void mouseReleased(MouseButtonEventArgs pMbea);

        void mouseMoved(MouseMotionEventArgs pMbea);

        event EventHandler<MouseButtonEventArgs> mouseReleasedEvent;

        event EventHandler<MouseButtonEventArgs> mousePressedEvent;

        event EventHandler<MouseMotionEventArgs> mouseMovedEvent;

        event EventHandler<KeyboardEventArgs> keyPressedEvent;

        event EventHandler<KeyboardEventArgs> keyReleasedEvent;*/
    }
}
