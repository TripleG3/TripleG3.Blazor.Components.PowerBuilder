using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace TripleG3.Blazor.Components.PowerBuilder.Tests;

[TestClass]
public class PictureButtonTests
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
	public void Renders_Text_When_ShowText_True()
	{
		// Arrange
		var actor = _testContext.RenderComponent<PictureButton>(parameters => parameters
			.Add(p => p.Text, "Hello")
			.Add(p => p.ShowText, true)
			.Add(p => p.ShowImage, false)
		);

		// Act
		var textSpan = actor.Find("span.pb-text");

		// Assert
		Assert.AreEqual("Hello", textSpan.TextContent);
	}

	[TestMethod]
	public void Does_Not_Render_Image_When_ShowImage_False()
	{
		// Arrange
		var actor = _testContext.RenderComponent<PictureButton>(p => p
			.Add(x => x.Image, "icon.png")
			.Add(x => x.ShowImage, false)
		);

		// Act
		var images = actor.FindAll("img");

		// Assert
		Assert.IsEmpty(images);
	}

	[TestMethod]
	public void Renders_Image_When_ShowImage_True()
	{
		// Arrange
		var actor = _testContext.RenderComponent<PictureButton>(p => p
			.Add(x => x.Image, "icon.png")
			.Add(x => x.ShowImage, true)
		);

		// Act
		var img = actor.Find("img.pb-picture");

		// Assert
		Assert.AreEqual("icon.png", img.GetAttribute("src"));
	}

	[TestMethod]
	public void Click_Invokes_OnClick()
	{
		// Arrange
		MouseEventArgs? received = null;
		var actor = _testContext.RenderComponent<PictureButton>(p => p
			.Add(x => x.Text, "Click Me")
			.Add(x => x.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, (MouseEventArgs e) => received = e))
		);

		// Act
		actor.Find("button").Click();

		// Assert
		Assert.IsNotNull(received);
	}

	[TestMethod]
	public void Toggle_Button_Changes_Checked_State_On_Click()
	{
		// Arrange
		bool? lastChecked = null;
		var actor = _testContext.RenderComponent<PictureButton>(p => p
			.Add(x => x.IsToggle, true)
			.Add(x => x.Checked, false)
			.Add(x => x.CheckedChanged, EventCallback.Factory.Create<bool>(this, (bool v) => lastChecked = v))
		);

		// Act
		actor.Find("button").Click();

		// Assert
		Assert.IsTrue(lastChecked);
	}

	[TestMethod]
	public void Disabled_Button_Does_Not_Invoke_OnClick()
	{
		// Arrange
		bool invoked = false;
		var actor = _testContext.RenderComponent<PictureButton>(p => p
			.Add(x => x.Disabled, true)
			.Add(x => x.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, (MouseEventArgs _) => invoked = true))
		);

		// Act
		actor.Find("button").Click();

		// Assert
		Assert.IsFalse(invoked);
	}

	[TestMethod]
	public void CheckedImage_Is_Prioritized_When_Checked()
	{
		// Arrange
		var actor = _testContext.RenderComponent<PictureButton>(p => p
			.Add(x => x.IsToggle, true)
			.Add(x => x.Checked, true)
			.Add(x => x.Image, "base.png")
			.Add(x => x.PressedImage, "pressed.png")
			.Add(x => x.CheckedImage, "checked.png")
		);

		// Act
		var img = actor.Find("img.pb-picture");

		// Assert
		Assert.AreEqual("checked.png", img.GetAttribute("src"));
	}

	[TestMethod]
	public void HoverImage_Shows_On_MouseEnter()
	{
		// Arrange
		var actor = _testContext.RenderComponent<PictureButton>(p => p
			.Add(x => x.Image, "base.png")
			.Add(x => x.HoverImage, "hover.png")
		);

		// Act
		var button = actor.Find("button");
		button.TriggerEvent("onmouseenter", new MouseEventArgs());
		var img = actor.Find("img.pb-picture");

		// Assert
		Assert.AreEqual("hover.png", img.GetAttribute("src"));
	}

	[TestMethod]
	public void PressedImage_Shows_On_PointerDown()
	{
		// Arrange
		var actor = _testContext.RenderComponent<PictureButton>(p => p
			.Add(x => x.Image, "base.png")
			.Add(x => x.PressedImage, "pressed.png")
		);

		// Act
		var button = actor.Find("button");
		button.TriggerEvent("onpointerdown", new PointerEventArgs());
		var img = actor.Find("img.pb-picture");

		// Assert
		Assert.AreEqual("pressed.png", img.GetAttribute("src"));
	}
}
