using System;
using System.Windows;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace MouseNag.Specs.TimerBasedInputSourceSpecs
{
    [Subject(typeof(TimerBasedInputSource))]
    public class when_timer_ticks_a_second_time_and_mouse_has_been_moved
    {
        static Mock<ISystemInput> systemInput = new Mock<ISystemInput>();
        static Mock<ITimer> timer = new Mock<ITimer>();
        static bool mouseMovedFired;
        static bool keyboardKeyPressedFired;

        Establish context = () =>
            {
                var inputSource = new TimerBasedInputSource(systemInput.Object, timer.Object);
                inputSource.MouseMoved += (sender, e) => mouseMovedFired = true;
                inputSource.KeyBoardKeyPressed += (sender, e) => keyboardKeyPressedFired = true;
                systemInput.Setup(i => i.GetCursorPosition()).Returns(new Point(0, 0));
                timer.Raise(t => t.Tick += null, new EventArgs());
                systemInput.Setup(i => i.GetCursorPosition()).Returns(new Point(1d, 1d));
            };


        Because of = () => 
            timer.Raise(t => t.Tick += null, new EventArgs());

        It should_fire_mouse_moved_event = () => mouseMovedFired.ShouldBeTrue();

        It should_not_fire_keyboard_key_pressed_event = () => keyboardKeyPressedFired.ShouldBeFalse();
    }
}
