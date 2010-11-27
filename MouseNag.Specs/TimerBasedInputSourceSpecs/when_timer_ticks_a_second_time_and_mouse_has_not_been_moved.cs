using System;
using System.Windows;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace MouseNag.Specs.TimerBasedInputSourceSpecs
{
    [Subject(typeof(TimerBasedInputSource))]
    public class when_timer_ticks_a_second_time_and_mouse_has_not_been_moved
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
            };


        Because of = () => 
            timer.Raise(t => t.Tick += null, new EventArgs());

        It should_not_fire_mouse_moved_event = () => mouseMovedFired.ShouldBeFalse();

        It should_not_fire_keyboard_key_pressed_event = () => keyboardKeyPressedFired.ShouldBeFalse();
    }
}
