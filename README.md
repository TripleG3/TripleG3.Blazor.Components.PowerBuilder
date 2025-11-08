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
