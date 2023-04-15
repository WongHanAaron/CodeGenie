using CodeGenie.Core.Services.Generators.ComponentGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Core.Tests.Services.ComponentGenerators
{
    [TestFixture]
    public class CodeGenieComponentGenerator_Tests : Generator_TestBase<CodeGenieComponentGenerator>
    {
        [SetUp]
        public void SetUp()
        {
            SetUpGeneratorAndParser();
        }


    }
}
