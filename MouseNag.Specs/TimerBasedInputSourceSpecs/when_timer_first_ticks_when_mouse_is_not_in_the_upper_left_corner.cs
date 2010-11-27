using System;
using System.Windows;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace MouseNag.Specs.TimerBasedInputSourceSpecs
{
    [Subject(typeof(TimerBasedInputSource))]
    public class when_timer_first_ticks_when_mouse_is_not_in_the_upper_left_corner
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
                systemInput.Setup(i => i.GetCursorPosition()).Returns(new Point(1, 1));
            };

        Because of = () => 
            timer.Raise(t => t.Tick += null, new EventArgs());

        It should_not_fire_mouse_moved_event = () => mouseMovedFired.ShouldBeFalse();
    }
}
