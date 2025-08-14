using NUnit.Framework;
using PaladinHub.Services.TalentTrees;
using PaladinHub.Data.Entities;

namespace PaladinHub.Tests.ServiceTests;

[TestFixture]
public class TalentTreeServiceTests
{
	[Test]
	public void ClassTreeBuilder_Builds_NonEmpty_Tree()
	{
		IClassTreeBuilder builder = new PaladinClassTreeBuilder();
		var tree = builder.BuildTree(new List<Spell> { new Spell { Name = "Lay on Hands" } });
		Assert.That(tree, Is.Not.Null);
		Assert.That(tree.Nodes.Count, Is.GreaterThan(0));
	}
}
