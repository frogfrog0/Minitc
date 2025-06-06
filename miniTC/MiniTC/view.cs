namespace MiniTC.Views;

// Interface for panel view
public interface IPanelView
{
    // Events that presenter will handle
    event EventHandler<string>? DriveSelected;
    event EventHandler<string>? ItemClicked;
    event EventHandler? DriveDropDownOpened;

    // Properties for presenter to control
    string CurrentPath { get; set; }
    List<string> AvailableDrives { get; set; }
    string? SelectedDrive { get; set; }

    // Methods for presenter to call
    void UpdateContents(List<string> items);
    void ClearSelection();
    void ShowError(string message);
    void SetPath(string path);
}

// Interface for main view
public interface IMainView
{
    // Events that presenter will handle
    event EventHandler? CopyRequested;

    // Properties for presenter to access
    IPanelView LeftPanel { get; }
    IPanelView RightPanel { get; }

    // Methods for presenter to call
    void ShowMessage(string message, string title);
    void ShowError(string message, string title);
}