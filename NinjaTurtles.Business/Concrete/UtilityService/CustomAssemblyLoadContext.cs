using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace NinjaTurtles.Business.Concrete.UtilityService
{
    public sealed class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        // İstersen ALC izole olsun diye isDefault: false
        public CustomAssemblyLoadContext() : base(isCollectible: false) { }

        // Dışarıdan çağıracağın güvenli API
        public IntPtr LoadUnmanagedLibrary(string baseDir, string dllName = "libwkhtmltox.dll")
        {
            string archFolder = RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.X64 => "win-x64",
                Architecture.X86 => "win-x86",
                _ => throw new PlatformNotSupportedException(
                        $"Only x86/x64 supported. Process={RuntimeInformation.ProcessArchitecture}")
            };

            string dllPath = Path.Combine(
                baseDir ?? AppContext.BaseDirectory,
                "runtimes", archFolder, "native", dllName);

            if (!File.Exists(dllPath))
                throw new FileNotFoundException($"Native DLL not found at '{dllPath}'.");

            try
            {
                // Arama yollarını BaseDirectory ile genişlet
                if (NativeLibrary.TryLoad(
                        dllPath,
                        Assembly.GetExecutingAssembly(),
                        DllImportSearchPath.AssemblyDirectory |
                        DllImportSearchPath.SafeDirectories,
                        out var handle))
                {
                    return handle;
                }

                // Detaylı istisna için:
                return NativeLibrary.Load(dllPath);
            }
            catch (Exception ex)
            {
                throw new BadImageFormatException(
                    $"Failed to load '{dllPath}'. Process={RuntimeInformation.ProcessArchitecture}, " +
                    $"Is64BitProcess={Environment.Is64BitProcess}. " +
                    $"Hint: DLL mimarisi uyumsuz ya da bağımlılık eksik (VC++ redist?).", ex);
            }
        }

        // Bu override'ı isimle çözümlemek için bırakıyoruz; elle path vereceğimiz için genelde boş döner
        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            // İsimle arama yapmayacağız; manuel path ile yükleyeceğiz.
            return IntPtr.Zero;
        }

        protected override Assembly Load(AssemblyName assemblyName) => null!;
    }
}
