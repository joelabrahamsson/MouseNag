using System;
using System.Windows;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace MouseNag.Specs.TimerBasedInputSourceSpecs
{
    [Subject(typeof(TimerBasedInputSource))]
    public class when_keyboard_input_has_occurred_between_two_mouse_moves
    {
        static Mock<ISystemInput> systemInput = new Mock<ISystemInput>();
        static Mock<ITimer> timer = new Mock<ITimer>();
        static int mouseMovedFiredCount;
        static int keybardKeyPressCount;

        Establish context = () =>
            {
                var inputSource = new TimerBasedInputSource(systemInput.Object, timer.Object);
                
                inputSource.MouseMoved += (sender, e) => mouseMovedFiredCount++;
                inputSource.KeyBoardKeyPressed += (sender, e) => keybardKeyPressCount++;

                systemInput.Setup(i => i.GetCursorPosition()).Returns(new Point(0, 0));
                systemInput.Setup(i => i.GetLastInputTime()).Returns(0);

                //No mouse movement, no keyboard action
                timer.Raise(t => t.Tick += null, new EventArgs());
                
                //Move mouse
                systemInput.Setup(i => i.GetLastInputTime()).Returns(1);
                systemInput.Setup(i => i.GetCursorPosition()).Returns(new Point(1, 0));
                timer.Raise(t => t.Tick += null, new EventArgs());

                //Keyboard key down, no mouse movement
                systemInput.Setup(i => i.GetLastInputTime()).Returns(2);
                timer.Raise(t => t.Tick += null, new EventArgs());

                //Setup new mouse movement
                systemInput.Setup(i => i.GetLastInputTime()).Returns(3);
                systemInput.Setup(i => i.GetCursorPosition()).Returns(new Point(1, 1));
            };


        Because of = () => 
            timer.Raise(t => t.Tick += null, new EventArgs());

        It should_fire_mouse_moved_event_twice = () => mouseMovedFiredCount.ShouldEqual(2);

        It should_fire_keyboardKeyPressed_once = () => keybardKeyPressCount.ShouldEqual(1);
    }
}
