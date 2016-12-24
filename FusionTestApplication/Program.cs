using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ManagedFusion;

namespace FusionTestApplication
{
    class Program
    {

        private const string _rndm = "someRandomString";

        static void Main()
        {
            // Working without administrator rights:
            VerifyIfAssemblyIsInstalledExample();
            EnumerateCacheAndReferencesExample();
            // Need administrator rights for the following:
            InstallAssemblyExample();
            UninstallAssemblyExample();
        }

        static void VerifyIfAssemblyIsInstalledExample()
        {
            Console.WriteLine("This example will test whether the specified assemblies are installed in the GAC.");
            InstallerDescription description
              = InstallerDescription.CreateForOpaqueString("My fusion test application", _rndm);
            AssemblyCache cache = new AssemblyCache(description);
            while (true)
            {
                Console.WriteLine("Specifiy an assembly name or \"stop\":");
                string value = Console.ReadLine();
                if (value == "stop")
                    break;
                bool isInstalled = cache.IsInstalled(new AssemblyName(value));
                Console.WriteLine("IsInstalled: " + isInstalled + Environment.NewLine);
            }
            Console.WriteLine("");
        }

        static void EnumerateCacheAndReferencesExample()
        {
            Console.WriteLine("This example will enumerate all assemblies in the GAC, and their references.");
            Console.WriteLine("For displaying purposes it is recommended to make your your console window about 140 characters wide.");
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
            InstallerDescription description
              = InstallerDescription.CreateForOpaqueString("My test application", _rndm);
            AssemblyCache cache = new AssemblyCache(description);
            foreach (AssemblyName c in cache)
            {
                Console.WriteLine(c.FullName);
                foreach (InstallerDescription rfr in cache.GetReferences(c))
                    Console.WriteLine("\t" + rfr);
            }
            Console.WriteLine(Environment.NewLine + "Done!" + Environment.NewLine + "Press enter to continue...");
            Console.ReadLine();
        }

        static void InstallAssemblyExample()
        {
            Console.WriteLine("This example will install an assembly to the GAC.");
            string currentExe = Process.GetCurrentProcess().MainModule.FileName;
            InstallerDescription description
              = InstallerDescription.CreateForFile("My test application", currentExe);
            AssemblyCache cache = new AssemblyCache(description);
            AssemblyName assemblyName = new AssemblyName("ManagedFusion")
            {
                CodeBase = Path.Combine(Path.GetDirectoryName(currentExe), "ManagedFusion.dll")
            };
            Console.WriteLine("  AssemblyName: " + assemblyName);
            Console.WriteLine("  InstallerDescription: " + description);
            cache.InstallAssembly(assemblyName, InstallBehaviour.Default);
            Console.WriteLine("Done!" + Environment.NewLine + "Press enter to continue...");
            Console.ReadLine();
        }

        static void UninstallAssemblyExample()
        {
            Console.WriteLine("This example will uninstall an assembly from the GAC.");
            string currentExe = Process.GetCurrentProcess().MainModule.FileName;
            InstallerDescription description
              = InstallerDescription.CreateForFile("My test application", currentExe);
            AssemblyCache cache = new AssemblyCache(description);
            AssemblyName assemblyName = new AssemblyName("ManagedFusion");
            Console.WriteLine("  AssemblyName: " + assemblyName);
            Console.WriteLine("  InstallerDescription: " + description);
            UninstallDisposition result = cache.UninstallAssembly(assemblyName);
            Console.WriteLine("Done! With result: " + result + Environment.NewLine + "Press enter to continue...");
            Console.ReadLine();
        }

    }
}
