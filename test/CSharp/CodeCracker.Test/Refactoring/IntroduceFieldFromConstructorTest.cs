using CodeCracker.CSharp.Refactoring;
using System.Threading.Tasks;
using Xunit;

namespace CodeCracker.Test.CSharp.Refactoring
{
    public class IntroduceFieldFromConstructorTest : CodeFixVerifier<IntroduceFieldFromConstructorAnalyzer, IntroduceFieldFromConstructorCodeFixProvider>
    {
        [Fact]
        public async Task WhenConstructorParameterHasPrivateField()
        {
            const string test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private int mPar;

            public TypeName(int par)
            {
               mPar = par;
            }
        }
    }";
            await VerifyCSharpHasNoDiagnosticsAsync(test);
        }

        [Fact]
        public async Task WhenConstructorParameterHasPrivateReadOnlyField()
        {
            const string test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private readonly int mPar;

            public TypeName(int par)
            {
               mPar = par;
            }
        }
    }";
            await VerifyCSharpHasNoDiagnosticsAsync(test);
        }

        [Fact]
        public async Task WhenConstructorParameterHasAnyFieldAssign()
        {
            const string test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private int myField;

            public TypeName(int par)
            {
               this.myField = par;
            }
        }
    }";
            await VerifyCSharpHasNoDiagnosticsAsync(test);
        }

        [Fact]
        public async Task ConstructorParameterWithPrivateField()
        {
            const string test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            public TypeName(int par)
            {
            }
        }
    }";

            const string expected = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private readonly int mPar;

            public TypeName(int par)
            {
               mPar = par;
            }
        }
    }";
            await VerifyCSharpFixAsync(test, expected);
        }

        [Fact]
        public async Task ConstructorParameterWithPrivateFieldTwoConstructors()
        {
            const string test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            public TypeName()
            {
            }

            public TypeName(int par)
            {
            }
        }
    }";

            const string expected = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private readonly int mPar;

            public TypeName()
            {
            }

            public TypeName(int par)
            {
               mPar = par;
            }
        }
    }";
            await VerifyCSharpFixAsync(test, expected);
        }

        [Fact]
        public async Task FieldAlreadyExistsAndMatchesType()
        {
            const string test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private readonly int mPar;

            public TypeName(int par)
            {
            }
        }
    }";

            const string expected = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private readonly int mPar;

            public TypeName(int par)
            {
               mPar = par;
            }
        }
    }";
            await VerifyCSharpFixAsync(test, expected);
        }

        [Fact]
        public async Task ConstructorParameterWithPrivateFieldWhenFieldParameterNameAlreadyExists()
        {
            const string test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private string mPar;

            public TypeName(int par)
            {
            }
        }
    }";
            const string expected = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private readonly int mPar1;
            private string mPar;

            public TypeName(int par)
            {
               mPar1 = par;
            }
        }
    }";
            await VerifyCSharpFixAsync(test, expected);
        }

        [Fact]
        public async Task ConstructorParameterWithPrivateFieldWhenFieldParameterNameAlreadyExistsOnEvent()
        {
            const string test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private event Action mPar;

            public TypeName(int par)
            {
            }
        }
    }";
            const string expected = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private readonly int mPar1;

            private event Action mPar;

            public TypeName(int par)
            {
               mPar1 = par;
            }
        }
    }";
            await VerifyCSharpFixAsync(test, expected);
        }

        [Fact]
        public async Task ConstructorParameterWithPrivateFieldWhenFieldParameterNameAlreadyExistsOnInterface()
        {
            const string test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            interface mPar { }

            public TypeName(int par)
            {
            }
        }
    }";
            const string expected = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private readonly int mPar1;

            interface mPar { }

            public TypeName(int par)
            {
               mPar1 = par;
            }
        }
    }";
            await VerifyCSharpFixAsync(test, expected);
        }

        [Fact]
        public async Task ConstructorParameterWithPrivateFieldWhenFieldParameterNameAlreadyExistsOnDelegate()
        {
            const string test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            delegate void mPar();

            public TypeName(int par)
            {
            }
        }
    }";
            const string expected = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private readonly int mPar1;

            delegate void mPar();

            public TypeName(int par)
            {
               mPar1 = par;
            }
        }
    }";
            await VerifyCSharpFixAsync(test, expected);
        }

        [Fact]
        public async Task ConstructorParameterWithPrivateFieldWhenFieldParameterNameAlreadyExistsOnInnerEnum()
        {
            const string test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            enum mPar { }

            public TypeName(int par)
            {
            }
        }
    }";
            const string expected = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private readonly int mPar1;

            enum mPar { }

            public TypeName(int par)
            {
               mPar1 = par;
            }
        }
    }";
            await VerifyCSharpFixAsync(test, expected);
        }

        [Fact]
        public async Task ConstructorParameterWithPrivateFieldWhenFieldParameterNameAlreadyExistsOnInnerStruct()
        {
            const string test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            struct mPar { }

            public TypeName(int par)
            {
            }
        }
    }";
            const string expected = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private readonly int mPar1;

            struct mPar { }

            public TypeName(int par)
            {
               mPar1 = par;
            }
        }
    }";
            await VerifyCSharpFixAsync(test, expected);
        }

        [Fact]
        public async Task ConstructorParameterWithPrivateFieldWhenFieldParameterNameAlreadyExistsOnInnerClass()
        {
            const string test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            class mPar { }

            public TypeName(int par)
            {
            }
        }
    }";
            const string expected = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private readonly int mPar1;

            class mPar { }

            public TypeName(int par)
            {
               mPar1 = par;
            }
        }
    }";
            await VerifyCSharpFixAsync(test, expected);
        }

        [Fact]
        public async Task ConstructorParameterWithPrivateFieldWhenFieldParameterNameAlreadyExistsInSecondPosition()
        {
            const string test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private string mBar, mPar;

            public TypeName(int par)
            {
            }
        }
    }";
            const string expected = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private readonly int mPar1;
            private string mBar, mPar;

            public TypeName(int par)
            {
               mPar1 = par;
            }
        }
    }";
            await VerifyCSharpFixAsync(test, expected);
        }

        [Fact]
        public async Task ConstructorParameterWithPrivateFieldWhenFieldParameterNameAlreadyExistsTwice()
        {
            const string test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private string mPar;
            private string mPar1;

            public TypeName(int par)
            {
            }
        }
    }";

            const string expected = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            private readonly int mPar2;
            private string mPar;
            private string mPar1;

            public TypeName(int par)
            {
               mPar2 = par;
            }
        }
    }";
            await VerifyCSharpFixAsync(test, expected);
        }

        [Fact]
        public async Task IntroduceFieldConstructorFixAllInProject()
        {
            const string source1 = @"
                using System;
                class foo1
                {
                    public foo1(int a)
                    {
                    }
                }
                class foo2
                {
                    public foo2(int a, int b)
                    {
                    }
                }
";
            const string source2 = @"
                using system;
                class foo3
                {
                    private string mBar;

                    public foo3(int bar)
                    {
                    }
                }
";
            const string source3 = @"
               using system;
               class foo4
               {
                   public foo4(int a, string b)
                   {
                   }
               }
            ";
            const string fixtest1 = @"
                using System;
                class foo1
                {
                    private readonly int mA;

                    public foo1(int a)
                    {
                        mA = a;
                    }
                }
                class foo2
                {
                    private readonly int mB;
                    private readonly int mA;

                    public foo2(int a, int b)
                    {
                        mA = a;
                        mB = b;
                    }
                }
";
            const string fixtest2 = @"
                using system;
                class foo3
                {
                    private readonly int mBar1;
                    private string mBar;

                    public foo3(int bar)
                    {
                        mBar1 = bar;
                    }
                }
";

            const string fixtest3 = @"
                using system;
                class foo4
                {
                    private readonly string mB;
                    private readonly int mA;

                    public foo4(int a, string b)
                    {
                        mA = a;
                        mB = b;
                    }
                }
    ";

            await VerifyCSharpFixAllAsync(new[] { source1, source2, source3 }, new[] { fixtest1, fixtest2, fixtest3 });
        }
    }
}