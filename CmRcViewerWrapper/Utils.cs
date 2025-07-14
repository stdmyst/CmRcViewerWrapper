using System.Diagnostics;

namespace CmRcViewerWrapper;

public static class Utils
{
    public static void RunCommand(string selectedHost, string identityAddress, string executionPath)
    {
        Process p = new();
        p.StartInfo.FileName = executionPath;
        p.StartInfo.Arguments = $"{identityAddress} [{selectedHost}]";

        p.Start();
        p.WaitForExit();
    }
}
