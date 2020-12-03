
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace resource.package
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(CONSTANT.NAME, CONSTANT.DESCRIPTION, CONSTANT.VERSION)]
    [Guid(CONSTANT.GUID)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.ShellInitialized_string, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class PreviewCURPackage : AsyncPackage
    {
        internal static class CONSTANT
        {
            public const string COPYRIGHT = "Copyright (c) 2020 by Viacheslav Lozinskyi. All rights reserved.";
            public const string DESCRIPTION = "Quick preview of CUR files";
            public const string EXTENSION = ".CUR";
            public const string GUID = "7657CB66-2C1D-49A4-A34B-A4E259B5FD7E";
            public const string NAME = "Preview-CUR";
            public const string VERSION = "1.0.3";
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            {
                cartridge.AnyPreview.Connect();
                cartridge.AnyPreview.Register(cartridge.AnyPreview.MODE.PREVIEW, CONSTANT.EXTENSION, new resource.preview.Native());
            }
            {
                await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            }
        }

        protected override int QueryClose(out bool canClose)
        {
            {
                cartridge.AnyPreview.Disconnect();
                canClose = true;
            }
            return VSConstants.S_OK;
        }
    }
}
