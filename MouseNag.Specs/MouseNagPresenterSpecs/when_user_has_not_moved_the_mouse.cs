using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace MouseNag.Specs.MouseNagPresenterSpecs
{
    [Subject(typeof(MouseNagPresenter))]
    public class when_user_has_not_moved_the_mouse
    {
        private static Mock<IInputSource> input;
        private static Mock<INag> annoyance;

        Establish context = () =>
        {
            annoyance = new Mock<INag>();
            input = new Mock<IInputSource>();
            new MouseNagPresenter(input.Object, annoyance.Object);
        };

        It should_not_nag = () =>
            annoyance.Verify(a => a.Nag(), Times.Never());

    }
}
