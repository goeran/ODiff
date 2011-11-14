using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ODiff.Tests.Fakes;

namespace ODiff.Tests.Learning
{
    [TestFixture]
    public class How_to_get_members_from_and_ExpandoObject
    {
        [Test]
        public void Its_a_dictionary_so_a_dictionary_cast_and_lookup_would_do()
        {
            dynamic a = new ExpandoObject();
            a.Name = "Steve";

            var members = (IDictionary<string, object>) a;
            Assert.IsTrue(members.ContainsKey("Name"));
        }
    }

    [TestFixture]
    public class Reflection_and_enums
    {
        [Test]
        public void How_to_figure_out_if_an_value_is_an_enum()
        {
            Object gender = Gender.Femal;

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
