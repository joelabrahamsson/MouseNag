using System;
using Machine.Specifications;
using Moq;
using MouseNag.InputMonitoring;
using MouseNag.Presentation;
using It = Machine.Specifications.It;

namespace MouseNag.Specs.MouseNagPresenterSpecs
{
    [Subject(typeof(MouseNagPresenter))]
    public class user_starts_program_and_moves_the_mouse
    {
        private static Mock<IInputSource> input;
        private static Mock<INag> annoyance;

        Establish context = () =>
        {
            annoyance = new Mock<INag>();
            input = new Mock<IInputSource>();
            new MouseNagPresenter(input.Object, annoyance.Object);
        };

        Because of = () =>
            input.Raise(i => i.MouseMoved += null, new EventArgs());

        It should_not_nag = () =>
            annoyance.Verify(a => a.Nag(), Times.Never());

    }
}
