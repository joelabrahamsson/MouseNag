using System;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace MouseNag.Specs.MouseNagPresenterSpecs
{
    [Subject(typeof(MouseNagPresenter))]
    public class when_user_has_been_using_the_keyboard_and_moves_the_mouse
    {
        private static Mock<IInputSource> input;
        private static Mock<INag> annoyance;

        Establish context = () =>
        {
            annoyance = new Mock<INag>();
            input = new Mock<IInputSource>();
            new MouseNagPresenter(input.Object, annoyance.Object);
            input.Raise(i => i.KeyBoardKeyPressed += null, new EventArgs());
        };

        Because of = () =>
            input.Raise(i => i.MouseMoved += null, new EventArgs());

        It should_nag = () =>
            annoyance.Verify(a => a.Nag());

    }
}
