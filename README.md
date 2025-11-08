# TripleG3.Blazor.Components.PowerBuilder
PowerBuilder Components for Blazor

## `PictureButton`
A lightweight button component that can render an image and/or text, with optional toggle behavior and visual states for hover, pressed, disabled, and checked.

- Image states: `Image`, `HoverImage`, `PressedImage`, `DisabledImage`, `CheckedImage`
- Text and accessibility: `Text`, `Title`, `AriaLabel`
- Toggle mode: `IsToggle`, `Checked`, `CheckedChanged`
- Layout and visibility: `Placement` (`Left`, `Right`, `Top`, `Bottom`), `ShowText`, `ShowImage`
- Sizing and styling: `Width`, `Height`, `Class`, `AdditionalAttributes`
- Events: `OnClick` (`MouseEventArgs`)

Add the namespace (if not already available):

```razor
@using TripleG3.Blazor.Components.PowerBuilder
```

Basic usage

```razor
<PictureButton
    Image="images/save.svg"
    Text="Save"
    Title="Save"
    AriaLabel="Save"
    OnClick="HandleSave" />

@code {
    void HandleSave(MouseEventArgs _)
    {
        // Save logic
    }
}
```

Toggle button

```razor
<PictureButton
    IsToggle="true"
    @bind-Checked="_isBold"
    Text="Bold"
    Image="images/bold-off.svg"
    CheckedImage="images/bold-on.svg"
    OnClick="_ => StateHasChanged()" />

@code {
    private bool _isBold;
}
```

Hover/pressed/disabled visuals

```razor
<PictureButton
    Image="images/play.svg"
    HoverImage="images/play-hover.svg"
    PressedImage="images/play-pressed.svg"
    DisabledImage="images/play-disabled.svg"
    Disabled="false" />
```

Layout and sizing

```razor
<PictureButton
    Placement="PicturePlacement.Top"
    Width="120px"
    Height="80px"
    ShowText="true"
    ShowImage="true"
    Text="Upload"
    Image="images/upload.svg" />
```

Notes
- When `IsToggle` is `true`, clicking flips `Checked` and invokes `CheckedChanged`.
- `AriaLabel` falls back to `Text` if provided; otherwise defaults to "button".
- Keyboard: space/enter update the pressed state and click behavior.

## `TabControl`
A tab control component that organizes content into multiple pages with clickable tabs, modeled after PowerBuilder's TabControl.

- Tab configuration: `TabPages` (list of `TabPage` objects)
- Selection state: `SelectedIndex`, `SelectedIndexChanged` (two-way binding)
- Tab positioning: `Position` (`Top`, `Bottom`, `Left`, `Right`)
- Visual options: `BoldSelectedText` (defaults to `true`)
- Sizing and styling: `Width`, `Height`, `Class`, `AdditionalAttributes`
- Events: `OnSelectionChanged` (callback when tab changes)

Each `TabPage` supports:
- `Text` - tab label
- `PictureName` - optional icon image path
- `Title` - tooltip text
- `AriaLabel` - accessibility label (falls back to `Text`)
- `Disabled` - prevents tab selection
- `ChildContent` - the content to display when tab is selected

Add the namespace (if not already available):

```razor
@using TripleG3.Blazor.Components.PowerBuilder
@using static TripleG3.Blazor.Components.PowerBuilder.TabControl
```

Basic usage with top tabs

```razor
<TabControl TabPages="@tabPages" @bind-SelectedIndex="@selectedIndex" />

@code {
    private int selectedIndex = 0;
    
    private readonly List<TabPage> tabPages = new()
    {
        new TabPage 
        { 
            Text = "Home", 
            Title = "Home Page",
            ChildContent = @<div>
                <h3>Welcome to Home</h3>
                <p>This is the home tab content.</p>
            </div>
        },
        new TabPage 
        { 
            Text = "Profile", 
            Title = "User Profile",
            ChildContent = @<div>
                <h3>User Profile</h3>
                <p>View and edit your profile.</p>
            </div>
        },
        new TabPage 
        { 
            Text = "Settings", 
            Title = "Application Settings",
            ChildContent = @<div>
                <h3>Settings</h3>
                <p>Configure your preferences.</p>
            </div>
        }
    };
}
```

Tabs with icons

```razor
<TabControl TabPages="@iconTabPages" />

@code {
    private readonly List<TabPage> iconTabPages = new()
    {
        new TabPage 
        { 
            Text = "Dashboard", 
            PictureName = "images/dashboard.svg",
            Title = "View Dashboard",
            ChildContent = @<div>
                <h3>Dashboard</h3>
                <p>Analytics and metrics.</p>
            </div>
        },
        new TabPage 
        { 
            Text = "Reports", 
            PictureName = "images/reports.svg",
            ChildContent = @<div>
                <h3>Reports</h3>
                <p>View your reports.</p>
            </div>
        }
    };
}
```

Different tab positions

```razor
<!-- Bottom tabs -->
<TabControl TabPages="@tabPages" Position="TabPositionValue.Bottom" />

<!-- Left tabs -->
<TabControl TabPages="@tabPages" Position="TabPositionValue.Left" Height="300px" />

<!-- Right tabs -->
<TabControl TabPages="@tabPages" Position="TabPositionValue.Right" Height="300px" />
```

Tabs with disabled state

```razor
<TabControl TabPages="@disabledTabPages" />

@code {
    private readonly List<TabPage> disabledTabPages = new()
    {
        new TabPage 
        { 
            Text = "Available", 
            ChildContent = @<div><p>This tab is available.</p></div>
        },
        new TabPage 
        { 
            Text = "Disabled", 
            Disabled = true,
            ChildContent = @<div><p>Cannot access this.</p></div>
        }
    };
}
```

Handle selection changes

```razor
<TabControl 
    TabPages="@tabPages" 
    @bind-SelectedIndex="@selectedIndex"
    OnSelectionChanged="@HandleTabChange" />

<p>Selected Index: @selectedIndex</p>

@code {
    private int selectedIndex = 0;
    
    private async Task HandleTabChange(int newIndex)
    {
        // Handle tab change
        Console.WriteLine($"Tab changed to: {newIndex}");
    }
}
```

Notes
- Use `@bind-SelectedIndex` for two-way binding of the selected tab.
- `SelectedIndexChanged` and `OnSelectionChanged` are both invoked when a tab is selected.
- Clicking a disabled tab has no effect.
- `AriaLabel` for each tab falls back to `Text` if not provided.
