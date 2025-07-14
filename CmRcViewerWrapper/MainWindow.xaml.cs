using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.Configuration;

namespace CmRcViewerWrapper;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private IConfiguration _configuration;
    private string _execPath = Consts.EXEC_PATH;
    public ObservableCollection<ComboBoxItem> HostItems { get; set; }
    public ComboBoxItem SelectedHost { get; set; }

    public MainWindow()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        _execPath = GetExecPathFromConfiguration();

        InitializeComponent();
        BuildLayoutFromConfiguration();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var selectedHost = HostSelector.Text.Trim();
        var identityAddress = IdentityAddressBox.Text.Trim();

        try
        {
            Utils.RunCommand(selectedHost, identityAddress, _execPath);
        }
        catch
        {
            MessageBox.Show("Incorrect data provided.");
        }
    }

    private void BuildLayoutFromConfiguration()
    {
        DataContext = this;

        var hosts = GetHostsFromConfiguration();

        if (hosts == null)
        {
            MessageBox.Show("Set the hosts to select in the configuration file and reset the application.");
            return;
        }

        var selectedHost = new ComboBoxItem { Content = hosts.First() };
        SelectedHost = selectedHost;
        hosts.RemoveAt(0);

        HostItems = new ObservableCollection<ComboBoxItem>() { selectedHost };

        if (hosts.Count > 0)
        {
            foreach (var host in hosts)
            {
                HostItems.Add(new ComboBoxItem { Content = host });
            }
        }    
    }

    private string GetExecPathFromConfiguration()
        => _configuration.GetValue<string>("CmRcViewerExecPath")
        ?? Consts.EXEC_PATH;

    private List<string>? GetHostsFromConfiguration()
        => _configuration.GetSection("HostList").Get<List<string>>()
        ?? null;
}