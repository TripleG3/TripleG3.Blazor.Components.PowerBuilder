using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using static TripleG3.Blazor.Components.PowerBuilder.TabControl;

namespace TripleG3.Blazor.Components.PowerBuilder.Tests;

[TestClass]
public class TabControlTests
{
	private Bunit.TestContext _testContext = null!;

	[TestInitialize]
	public void Setup()
	{
		_testContext = new Bunit.TestContext();
	}

	[TestCleanup]
	public void Cleanup() => _testContext.Dispose();

	[TestMethod]
	public void Renders_Multiple_Tabs()
	{
		// Arrange
		var tabPages = new List<TabPage>
		{
			new() { Text = "Tab 1" },
			new() { Text = "Tab 2" },
			new() { Text = "Tab 3" }
		};

		var actor = _testContext.RenderComponent<TabControl>(p => p
			.Add(x => x.TabPages, tabPages)
		);

		// Act
		var tabButtons = actor.FindAll("button.pb-tab-button");

		// Assert
		Assert.HasCount(3, tabButtons);
		Assert.AreEqual("Tab 1", tabButtons[0].QuerySelector(".pb-tab-text")?.TextContent);
		Assert.AreEqual("Tab 2", tabButtons[1].QuerySelector(".pb-tab-text")?.TextContent);
		Assert.AreEqual("Tab 3", tabButtons[2].QuerySelector(".pb-tab-text")?.TextContent);
	}

	[TestMethod]
	public void First_Tab_Selected_By_Default()
	{
		// Arrange
		var tabPages = new List<TabPage>
		{
			new() { Text = "Tab 1" },
			new() { Text = "Tab 2" }
		};

		var actor = _testContext.RenderComponent<TabControl>(p => p
			.Add(x => x.TabPages, tabPages)
			.Add(x => x.SelectedIndex, 0)
		);

		// Act
		var selectedTab = actor.Find("button.pb-tab-selected");

		// Assert
		Assert.AreEqual("Tab 1", selectedTab.QuerySelector(".pb-tab-text")?.TextContent);
		Assert.AreEqual("true", selectedTab.GetAttribute("aria-selected"));
	}

	[TestMethod]
	public void Click_Changes_Selected_Tab()
	{
		// Arrange
		int? selectedIndex = null;
		var tabPages = new List<TabPage>
		{
			new() { Text = "Tab 1" },
			new() { Text = "Tab 2" }
		};

		var actor = _testContext.RenderComponent<TabControl>(p => p
			.Add(x => x.TabPages, tabPages)
			.Add(x => x.SelectedIndex, 0)
			.Add(x => x.SelectedIndexChanged, EventCallback.Factory.Create<int>(this, (int i) => selectedIndex = i))
		);

		// Act
		var tabButtons = actor.FindAll("button.pb-tab-button");
		tabButtons[1].Click();

		// Assert
		Assert.AreEqual(1, selectedIndex);
	}

	[TestMethod]
	public void Disabled_Tab_Does_Not_Change_Selection()
	{
		// Arrange
		int? selectedIndex = 0;
		var tabPages = new List<TabPage>
		{
			new() { Text = "Tab 1" },
			new() { Text = "Tab 2", Disabled = true }
		};

		var actor = _testContext.RenderComponent<TabControl>(p => p
			.Add(x => x.TabPages, tabPages)
			.Add(x => x.SelectedIndex, 0)
			.Add(x => x.SelectedIndexChanged, EventCallback.Factory.Create<int>(this, (int i) => selectedIndex = i))
		);

		// Act
		var tabButtons = actor.FindAll("button.pb-tab-button");
		tabButtons[1].Click();

		// Assert
		Assert.AreEqual(0, selectedIndex);
		Assert.IsTrue(tabButtons[1].ClassList.Contains("pb-tab-disabled"));
	}

	[TestMethod]
	public void Tab_Displays_Icon_When_PictureName_Set()
	{
		// Arrange
		var tabPages = new List<TabPage>
		{
			new() { Text = "Tab 1", PictureName = "icon.png" }
		};

		var actor = _testContext.RenderComponent<TabControl>(p => p
			.Add(x => x.TabPages, tabPages)
		);

		// Act
		var icon = actor.Find("img.pb-tab-icon");

		// Assert
		Assert.AreEqual("icon.png", icon.GetAttribute("src"));
	}

	[TestMethod]
	public void Tab_Without_Icon_Does_Not_Render_Image()
	{
		// Arrange
		var tabPages = new List<TabPage>
		{
			new() { Text = "Tab 1" }
		};

		var actor = _testContext.RenderComponent<TabControl>(p => p
			.Add(x => x.TabPages, tabPages)
		);

		// Act
		var images = actor.FindAll("img.pb-tab-icon");

		// Assert
		Assert.IsEmpty(images);
	}

	[TestMethod]
	public void Renders_Tab_Content_For_Selected_Tab()
	{
		// Arrange
		var tabPages = new List<TabPage>
		{
			new() { Text = "Tab 1", ChildContent = (builder) =>
			{
				builder.AddContent(0, "Content 1");
			}},
			new() { Text = "Tab 2", ChildContent = (builder) =>
			{
				builder.AddContent(0, "Content 2");
			}}
		};

		var actor = _testContext.RenderComponent<TabControl>(p => p
			.Add(x => x.TabPages, tabPages)
			.Add(x => x.SelectedIndex, 0)
		);

		// Act
		var content = actor.Find(".pb-tab-content");

		// Assert
		Assert.AreEqual("Content 1", content.TextContent.Trim());
	}

	[TestMethod]
	public void Switching_Tab_Changes_Content()
	{
		// Arrange
		var tabPages = new List<TabPage>
		{
			new() { Text = "Tab 1", ChildContent = (builder) =>
			{
				builder.AddContent(0, "Content 1");
			}},
			new() { Text = "Tab 2", ChildContent = (builder) =>
			{
				builder.AddContent(0, "Content 2");
			}}
		};

		var actor = _testContext.RenderComponent<TabControl>(p => p
			.Add(x => x.TabPages, tabPages)
			.Add(x => x.SelectedIndex, 0)
		);

		// Act
		var tabButtons = actor.FindAll("button.pb-tab-button");
		tabButtons[1].Click();
		var content = actor.Find(".pb-tab-content");

		// Assert
		Assert.AreEqual("Content 2", content.TextContent.Trim());
	}

	[TestMethod]
	public void BoldSelectedText_Applies_Bold_Class_To_Selected_Tab()
	{
		// Arrange
		var tabPages = new List<TabPage>
		{
			new() { Text = "Tab 1" },
			new() { Text = "Tab 2" }
		};

		var actor = _testContext.RenderComponent<TabControl>(p => p
			.Add(x => x.TabPages, tabPages)
			.Add(x => x.SelectedIndex, 0)
			.Add(x => x.BoldSelectedText, true)
		);

		// Act
		var selectedTabText = actor.Find("button.pb-tab-selected .pb-tab-text");

		// Assert
		Assert.IsTrue(selectedTabText.ClassList.Contains("pb-bold"));
	}

	[TestMethod]
	public void TabPosition_Top_Applies_Correct_Class()
	{
		// Arrange
		var tabPages = new List<TabPage> { new() { Text = "Tab 1" } };

		var actor = _testContext.RenderComponent<TabControl>(p => p
			.Add(x => x.TabPages, tabPages)
			.Add(x => x.Position, TabPositionValue.Top)
		);

		// Act
		var control = actor.Find(".pb-tab-control");

		// Assert
		Assert.IsTrue(control.ClassList.Contains("pb-tabs-top"));
	}

	[TestMethod]
	public void TabPosition_Bottom_Applies_Correct_Class()
	{
		// Arrange
		var tabPages = new List<TabPage> { new() { Text = "Tab 1" } };

		var actor = _testContext.RenderComponent<TabControl>(p => p
			.Add(x => x.TabPages, tabPages)
			.Add(x => x.Position, TabPositionValue.Bottom)
		);

		// Act
		var control = actor.Find(".pb-tab-control");

		// Assert
		Assert.IsTrue(control.ClassList.Contains("pb-tabs-bottom"));
	}

	[TestMethod]
	public void TabPosition_Left_Applies_Correct_Class()
	{
		// Arrange
		var tabPages = new List<TabPage> { new() { Text = "Tab 1" } };

		var actor = _testContext.RenderComponent<TabControl>(p => p
			.Add(x => x.TabPages, tabPages)
			.Add(x => x.Position, TabPositionValue.Left)
		);

		// Act
		var control = actor.Find(".pb-tab-control");

		// Assert
		Assert.IsTrue(control.ClassList.Contains("pb-tabs-left"));
	}

	[TestMethod]
	public void TabPosition_Right_Applies_Correct_Class()
	{
		// Arrange
		var tabPages = new List<TabPage> { new() { Text = "Tab 1" } };

		var actor = _testContext.RenderComponent<TabControl>(p => p
			.Add(x => x.TabPages, tabPages)
			.Add(x => x.Position, TabPositionValue.Right)
		);

		// Act
		var control = actor.Find(".pb-tab-control");

		// Assert
		Assert.IsTrue(control.ClassList.Contains("pb-tabs-right"));
	}

	[TestMethod]
	public void OnSelectionChanged_Invoked_When_Tab_Clicked()
	{
		// Arrange
		int? changedIndex = null;
		var tabPages = new List<TabPage>
		{
			new() { Text = "Tab 1" },
			new() { Text = "Tab 2" }
		};

		var actor = _testContext.RenderComponent<TabControl>(p => p
			.Add(x => x.TabPages, tabPages)
			.Add(x => x.SelectedIndex, 0)
			.Add(x => x.OnSelectionChanged, EventCallback.Factory.Create<int>(this, (int i) => changedIndex = i))
		);

		// Act
		var tabButtons = actor.FindAll("button.pb-tab-button");
		tabButtons[1].Click();

		// Assert
		Assert.AreEqual(1, changedIndex);
	}

	[TestMethod]
	public void AriaLabel_Used_When_Provided()
	{
		// Arrange
		var tabPages = new List<TabPage>
		{
			new() { Text = "Tab 1", AriaLabel = "First Tab" }
		};

		var actor = _testContext.RenderComponent<TabControl>(p => p
			.Add(x => x.TabPages, tabPages)
		);

		// Act
		var tabButton = actor.Find("button.pb-tab-button");

		// Assert
		Assert.AreEqual("First Tab", tabButton.GetAttribute("aria-label"));
	}

	[TestMethod]
	public void Title_Applied_To_Tab_Button()
	{
		// Arrange
		var tabPages = new List<TabPage>
		{
			new() { Text = "Tab 1", Title = "Tooltip for Tab 1" }
		};

		var actor = _testContext.RenderComponent<TabControl>(p => p
			.Add(x => x.TabPages, tabPages)
		);

		// Act
		var tabButton = actor.Find("button.pb-tab-button");

		// Assert
		Assert.AreEqual("Tooltip for Tab 1", tabButton.GetAttribute("title"));
	}
}
