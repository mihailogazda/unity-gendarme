//
// Unit tests for OperatorEqualsShouldBeOverloadedRule
//
// Authors:
//	Andreas Noever <andreas.noever@gmail.com>
//
//  (C) 2008 Andreas Noever
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using Gendarme.Rules.Design;

using NUnit.Framework;
using Test.Rules.Definitions;
using Test.Rules.Fixtures;

namespace Test.Rules.Design {

	[TestFixture]
	public class OperatorEqualsShouldBeOverloadedTest : TypeRuleTestFixture<OperatorEqualsShouldBeOverloadedRule> {

		[Test]
		public void DoesNotApply ()
		{
			AssertRuleDoesNotApply (SimpleTypes.Delegate);
			AssertRuleDoesNotApply (SimpleTypes.Enum);
			AssertRuleDoesNotApply (SimpleTypes.Interface);
		}

		class NoOperator {
			public void DoStuff () { }
		}

		[Test]
		public void TestNoOperator ()
		{
			AssertRuleSuccess<NoOperator> ();
		}

		class OnlyOneOperator {
			public static OnlyOneOperator operator + (OnlyOneOperator a, OnlyOneOperator b)
			{
				return new OnlyOneOperator ();
			}
		}

		[Test]
		public void TestOnlyOneOperator ()
		{
			AssertRuleSuccess<OnlyOneOperator> ();
		}

		class EverythingOK {
			public static EverythingOK operator + (EverythingOK a, EverythingOK b)
			{
				return new EverythingOK ();
			}

			public static EverythingOK operator - (EverythingOK a, EverythingOK b)
			{
				return new EverythingOK ();
			}

			public static bool operator == (EverythingOK a, EverythingOK b)
			{
				return true;
			}

			public static bool operator != (EverythingOK a, EverythingOK b)
			{
				return true;
			}
		}

		[Test]
		public void TestEverythingOK ()
		{
			AssertRuleSuccess<EverythingOK> ();
		}

		class NoEquals {
			public static NoEquals operator + (NoEquals a, NoEquals b)
			{
				return null;
			}

			public static NoEquals operator - (NoEquals a, NoEquals b)
			{
				return null;
			}
		}

		[Test]
		public void TestNoEquals ()
		{
			AssertRuleFailure<NoEquals> (1);
		}

		struct StructNoOperator {
			public void DoWork () { }
		}

		[Test]
		public void TestStructNoOperator ()
		{
			AssertRuleSuccess<StructNoOperator> ();
		}

		struct StructNoEquality {
			public override bool Equals (object obj)
			{
				return base.Equals (obj);
			}
		}

		[Test]
		public void TestStructNoEquality ()
		{
			AssertRuleFailure<StructNoEquality> (1);
		}
	}
}
