using MiniTC.Models;
using MiniTC.Views;

namespace MiniTC.Presenters;

// Presenter for individual panels
public class PanelPresenter
{
    private readonly IPanelView view;
    private readonly FileSystemModel model;
    private string selectedFilePath = "";

    public PanelPresenter(IPanelView view, FileSystemModel model)
    {
        this.view = view ?? throw new ArgumentNullException(nameof(view));
        this.model = model ?? throw new ArgumentNullException(nameof(model));

        // Subscribe to view events
        this.view.DriveSelected += OnDriveSelected;
        this.view.ItemClicked += OnItemClicked;
        this.view.DriveDropDownOpened += OnDriveDropDownOpened;

        // Initialize
        LoadAvailableDrives();
        InitializeToDefaultDrive();
    }

    public string CurrentPath => view.CurrentPath;
    public string SelectedFilePath => selectedFilePath;
    public bool HasFileSelected => !string.IsNullOrEmpty(selectedFilePath) && model.FileExists(selectedFilePath);

    private void LoadAvailableDrives()
    {
        try
        {
            var drives = model.GetAvailableDrives();
            view.AvailableDrives = drives;
        }
        catch (Exception ex)
        {
            view.ShowError($"Error loading drives: {ex.Message}");
        }
    }

    private void InitializeToDefaultDrive()
    {
        if (view.AvailableDrives.Count > 0)
        {
            string initialDrive = view.AvailableDrives.FirstOrDefault(d => d.StartsWith("C:")) ?? view.AvailableDrives[0];
            NavigateToPath(initialDrive);
        }
    }

    private void OnDriveDropDownOpened(object? sender, EventArgs e)
    {
        LoadAvailableDrives();
    }

    private void OnDriveSelected(object? sender, string selectedDrive)
    {
        if (!string.IsNullOrEmpty(selectedDrive))
        {
            NavigateToPath(selectedDrive);
        }
    }

    private void OnItemClicked(object? sender, string selectedItem)
    {
        if (string.IsNullOrEmpty(selectedItem)) return;

        try
        {
            if (selectedItem == "..")
            {
                NavigateToParent();
            }
            else if (selectedItem.StartsWith("<D> "))
            {
                string directoryName = selectedItem.Substring(4); // Remove "<D> " prefix
                string newPath = model.CombinePath(CurrentPath, directoryName);
                NavigateToPath(newPath);
            }
            else
            {
                // File selected
                selectedFilePath = model.CombinePath(CurrentPath, selectedItem);
            }
        }
        catch (Exception ex)
        {
            view.ShowError($"Navigation error: {ex.Message}");
        }
    }

    private void NavigateToParent()
    {
        try
        {
            string? parentPath = model.GetParentDirectory(CurrentPath);
            if (!string.IsNullOrEmpty(parentPath))
            {
                NavigateToPath(parentPath);
            }
        }
        catch (Exception ex)
        {
            view.ShowError($"Error navigating to parent: {ex.Message}");
        }
    }

    public void NavigateToPath(string path)
    {
        try
        {
            if (!model.DirectoryExists(path))
            {
                view.ShowError($"Path does not exist: {path}");
                return;
            }

            // Update path
            view.SetPath(path);
            
            // Update drive selection
            string? pathRoot = Path.GetPathRoot(path);
            if (view.SelectedDrive != pathRoot)
            {
                view.SelectedDrive = pathRoot;
            }

            // Refresh contents
            RefreshContents();
            
            // Clear file selection
            selectedFilePath = "";
            view.ClearSelection();
        }
        catch (Exception ex)
        {
            view.ShowError($"Error navigating to path: {ex.Message}");
        }
    }

    public void RefreshContents()
    {
        try
        {
            var content = model.GetDirectoryContent(CurrentPath);
            var displayItems = new List<string>();

            // Add ".." if not at root
            if (!content.IsRoot)
            {
                displayItems.Add("..");
            }

            // Add directories with <D> prefix
            foreach (string directory in content.Directories)
            {
                displayItems.Add("<D> " + directory);
            }

            // Add files without prefix
            displayItems.AddRange(content.Files);

            view.UpdateContents(displayItems);
        }
        catch (UnauthorizedAccessException)
        {
            view.ShowError("Access denied to this directory.");
        }
        catch (Exception ex)
        {
            view.ShowError($"Error reading directory contents: {ex.Message}");
        }
    }
}

// Main presenter coordinating the application
public class MainPresenter
{
    private readonly IMainView view;
    private readonly FileSystemModel model;
    private readonly PanelPresenter leftPanelPresenter;
    private readonly PanelPresenter rightPanelPresenter;

    public MainPresenter(IMainView view, FileSystemModel model)
    {
        this.view = view ?? throw new ArgumentNullException(nameof(view));
        this.model = model ?? throw new ArgumentNullException(nameof(model));

        // Create panel presenters
        leftPanelPresenter = new PanelPresenter(view.LeftPanel, model);
        rightPanelPresenter = new PanelPresenter(view.RightPanel, model);

        // Subscribe to view events
        this.view.CopyRequested += OnCopyRequested;
    }

    private void OnCopyRequested(object? sender, EventArgs e)
    {
        try
        {
            // Determine source and destination panels
            PanelPresenter? sourcePresenter = null;
            PanelPresenter? destinationPresenter = null;

            if (leftPanelPresenter.HasFileSelected)
            {
                sourcePresenter = leftPanelPresenter;
                destinationPresenter = rightPanelPresenter;
            }
            else if (rightPanelPresenter.HasFileSelected)
            {
                sourcePresenter = rightPanelPresenter;
                destinationPresenter = leftPanelPresenter;
            }
            else
            {
                view.ShowMessage("Please select a file to copy.", "No File Selected");
                return;
            }

            // Perform copy operation
            string sourceFilePath = sourcePresenter.SelectedFilePath;
            string fileName = model.GetFileName(sourceFilePath);
            string destinationFilePath = model.CombinePath(destinationPresenter.CurrentPath, fileName);

            model.CopyFile(sourceFilePath, destinationFilePath);

            // Refresh destination panel
            destinationPresenter.RefreshContents();

            view.ShowMessage($"File '{fileName}' copied successfully.", "Copy Complete");
        }
        catch (Exception ex)
        {
            view.ShowError($"Copy operation failed: {ex.Message}", "Copy Error");
        }
    }
}