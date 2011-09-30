﻿using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace Whut.AttachTo
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidAttachToPkgString)]
    public sealed class AttachToPackage : Package
    {
        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the menu item.
                CommandID attachToIISMenuCommandID = new CommandID(GuidList.guidAttachToCmdSet, (int)PkgCmdIDList.cmdidWhutAttachToIIS);
                MenuCommand attachToIISMenuItem = new MenuCommand(AttachToIISMenuItemCallback, attachToIISMenuCommandID);
                mcs.AddCommand(attachToIISMenuItem);
            }
        }

        private void AttachToIISMenuItemCallback(object sender, EventArgs e)
        {
            DTE dte = (DTE)this.GetService(typeof(DTE));
            foreach (Process process in dte.Debugger.LocalProcesses)
            {
                if (process.Name.EndsWith("w3wp.exe"))
                {
                    process.Attach();
                }
            }
        }

    }
}
