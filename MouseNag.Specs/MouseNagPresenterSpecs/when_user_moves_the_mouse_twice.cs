using System;
using Machine.Specifications;
using Moq;
using MouseNag.InputMonitoring;
using MouseNag.Presentation;
using It = Machine.Specifications.It;

namespace MouseNag.Specs.MouseNagPresenterSpecs
{
    [Subject(typeof(MouseNagPresenter))]
    public class when_user_moves_the_mouse_twice
    {
        private static Mock<IInputSource> input;
        private static Mock<INag> annoyance;

        Establish context = () =>
        {
            annoyance = new Mock<INag>();
            input = new Mock<IInputSource>();
            new MouseNagPresenter(input.Object, annoyance.Object);
            input.Raise(i => i.MouseMoved += null, new EventArgs());
        };

        Because of = () =>
            input.Raise(i => i.MouseMoved += null, new EventArgs());

        It should_not_nag = () =>
            annoyance.Verify(a => a.Nag(), Times.Never());

    }
}
