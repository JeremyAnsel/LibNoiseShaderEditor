using Microsoft.Win32;
using System;
using System.Text;
using System.Windows;

namespace LibNoiseShaderEditor.Helpers
{
    public static class FileDialogHelpers
    {
        public static string GetOpenFileName(Window owner, string extension, string directory = null)
        {
            if (extension is null)
            {
                throw new ArgumentNullException(nameof(extension));
            }

            var dialog = new OpenFileDialog
            {
                DefaultExt = extension,
                Filter = $"{extension.ToUpperInvariant()} files |*.{extension.ToLowerInvariant()}|All files|*.*",
                CheckFileExists = true,
                InitialDirectory = directory
            };

            if (dialog.ShowDialog(owner) != true)
            {
                return null;
            }

            return dialog.FileName;
        }

        public static string GetOpenFileName(Window owner, string[] extensions, string directory = null)
        {
            if (extensions is null || extensions.Length == 0)
            {
                throw new ArgumentNullException(nameof(extensions));
            }

            var dialog = new OpenFileDialog
            {
                DefaultExt = extensions[0],
                CheckFileExists = true,
                InitialDirectory = directory
            };

            var sb = new StringBuilder();

            foreach (string extension in extensions)
            {
                sb.Append($"{extension.ToUpperInvariant()} files |*.{extension.ToLowerInvariant()}|");
            }

            sb.Append("All files|*.*");
            dialog.Filter = sb.ToString();

            if (dialog.ShowDialog(owner) != true)
            {
                return null;
            }

            return dialog.FileName;
        }

        public static string GetSaveFileName(Window owner, string extension, string directory = null)
        {
            if (extension is null)
            {
                throw new ArgumentNullException(nameof(extension));
            }

            var dialog = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = extension,
                Filter = $"{extension.ToUpperInvariant()} files |*.{extension.ToLowerInvariant()}|All files|*.*",
                InitialDirectory = directory
            };

            if (dialog.ShowDialog(owner) != true)
            {
                return null;
            }

            return dialog.FileName;
        }

        public static string GetSaveFileName(Window owner, string[] extensions, string directory = null)
        {
            if (extensions is null || extensions.Length == 0)
            {
                throw new ArgumentNullException(nameof(extensions));
            }

            var dialog = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = extensions[0],
                InitialDirectory = directory
            };

            var sb = new StringBuilder();

            foreach (string extension in extensions)
            {
                sb.Append($"{extension.ToUpperInvariant()} files |*.{extension.ToLowerInvariant()}|");
            }

            sb.Append("All files|*.*");
            dialog.Filter = sb.ToString();

            if (dialog.ShowDialog(owner) != true)
            {
                return null;
            }

            return dialog.FileName;
        }
    }
}
