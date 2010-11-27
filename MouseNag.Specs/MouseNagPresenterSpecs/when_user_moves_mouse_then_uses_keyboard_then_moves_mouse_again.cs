using System;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace MouseNag.Specs.MouseNagPresenterSpecs
{
    [Subject(typeof(MouseNagPresenter))]
    public class when_user_moves_mouse_then_uses_keyboard_then_moves_mouse_again
    {
        private static Mock<IInputSource> input;
        private static Mock<INag> annoyance;

        Establish context = () =>
        {
            annoyance = new Mock<INag>();
            input = new Mock<IInputSource>();
            new MouseNagPresenter(input.Object, annoyance.Object);
            input.Raise(i => i.MouseMoved += null, new EventArgs());
            input.Raise(i => i.KeyBoardKeyPressed += null, new EventArgs());
        };

        Because of = () =>
            input.Raise(i => i.MouseMoved += null, new EventArgs());

        It should_annoy_the_user_twice = () =>
            annoyance.Verify(a => a.Nag(), Times.Exactly(2));

    }
}
