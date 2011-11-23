using System;
using NUnit.Framework;
using ODiff.Tests.Fakes;

namespace ODiff.Tests.Learning
{
    public class Learning_about_reflection
    {
        [TestFixture]
        public class Reflection_and_enums
        {
            [Test]
            public void How_to_figure_out_if_an_value_is_an_enum()
            {
                Object gender = Gender.Female;

                Assert.IsTrue(gender.GetType().IsEnum);

            }
        }

        [TestFixture]
        public class Enums_and_boxing
        {
            [Test]
            public void Compare_enums_as_objects()
            {
                Object male = Gender.Male;
                Object anotherMaleGender = Gender.Male;

                Assert.IsFalse(male == anotherMaleGender);
                Assert.IsTrue(male.Equals(anotherMaleGender));
            }
        }
    }
}
