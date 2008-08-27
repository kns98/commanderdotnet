﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ShellDll;

namespace Commander
{
    public class ShellContextMenu : NativeWindow
    {
        private IContextMenu iContextMenu, newContextMenu;
        private IContextMenu2 iContextMenu2, newContextMenu2;
        private IContextMenu3 iContextMenu3, newContextMenu3;
        private IntPtr newSubmenuPtr;

        public ShellContextMenu()
        {
            this.CreateHandle(new CreateParams());
        }

        /// <summary>
        /// This method receives WindowMessages. It will make the "Open With" and "Send To" work 
        /// by calling HandleMenuMsg and HandleMenuMsg2. It will also call the OnContextMenuMouseHover 
        /// method of Browser when hovering over a ContextMenu item.
        /// </summary>
        /// <param name="m">the Message of the Browser's WndProc</param>
        /// <returns>true if the message has been handled, false otherwise</returns>
        protected override void WndProc(ref Message m)
        {
            if (iContextMenu2 != null &&
                (m.Msg == (int)ShellAPI.WM.INITMENUPOPUP ||
                 m.Msg == (int)ShellAPI.WM.MEASUREITEM ||
                 m.Msg == (int)ShellAPI.WM.DRAWITEM))
            {
                if (iContextMenu2.HandleMenuMsg(
                    (uint)m.Msg, m.WParam, m.LParam) == ShellAPI.S_OK)
                    return;
            }

            if (newContextMenu2 != null &&
                ((m.Msg == (int)ShellAPI.WM.INITMENUPOPUP && m.WParam == newSubmenuPtr) ||
                 m.Msg == (int)ShellAPI.WM.MEASUREITEM ||
                 m.Msg == (int)ShellAPI.WM.DRAWITEM))
            {
                if (newContextMenu2.HandleMenuMsg(
                    (uint)m.Msg, m.WParam, m.LParam) == ShellAPI.S_OK)
                    return;
            }

            if (iContextMenu3 != null &&
                m.Msg == (int)ShellAPI.WM.MENUCHAR)
            {
                if (iContextMenu3.HandleMenuMsg2(
                    (uint)m.Msg, m.WParam, m.LParam, IntPtr.Zero) == ShellAPI.S_OK)
                    return;
            }

            if (newContextMenu3 != null &&
                m.Msg == (int)ShellAPI.WM.MENUCHAR)
            {
                if (newContextMenu3.HandleMenuMsg2(
                    (uint)m.Msg, m.WParam, m.LParam, IntPtr.Zero) == ShellAPI.S_OK)
                    return;
            }

            base.WndProc(ref m);
        }

        private static IntPtr[] GetPIDLs(params FileSystemInfo[] list)
        {
            List<IntPtr> pidls = new List<IntPtr>(list.Length);

            foreach (FileSystemInfo item in list)
            {
                pidls.Add(ShellFolder.GetPathPIDL(item.FullName));
            }

            return pidls.ToArray();
        }

        private static IntPtr[] GetPIDLs(params string[] pathList)
        {
            List<IntPtr> pidls = new List<IntPtr>(pathList.Length);

            foreach (string path in pathList)
            {
                pidls.Add(ShellFolder.GetPathPIDL(path));
            }

            return pidls.ToArray();
        }

        public void Show(Point location, params string[] pathList)
        {
            IntPtr[] pidls = GetPIDLs(pathList);
            string parentDirectory = Path.GetDirectoryName(pathList[0]);
            if (parentDirectory == null)
            {
                parentDirectory = SpecialFolderPath.MyComputer;
            }
            IShellFolder parentShellFolder = ShellFolder.GetShellFolder(parentDirectory);

            IntPtr contextMenu = IntPtr.Zero;
            IntPtr iContextMenuPtr = IntPtr.Zero;
            IntPtr iContextMenuPtr2 = IntPtr.Zero;
            IntPtr iContextMenuPtr3 = IntPtr.Zero;

            // Show / Invoke
            try
            {
                if (ContextMenuHelper.GetIContextMenu(parentShellFolder, pidls, out iContextMenuPtr, out iContextMenu))
                {
                    contextMenu = ShellAPI.CreatePopupMenu();
                    iContextMenu.QueryContextMenu(
                        contextMenu,
                        0,
                        ShellAPI.CMD_FIRST,
                        ShellAPI.CMD_LAST,
                        ShellAPI.CMF.EXPLORE |
                        ShellAPI.CMF.CANRENAME |
                        ((Control.ModifierKeys & Keys.Shift) != 0 ? ShellAPI.CMF.EXTENDEDVERBS : 0));

                    Marshal.QueryInterface(iContextMenuPtr, ref ShellAPI.IID_IContextMenu2, out iContextMenuPtr2);
                    Marshal.QueryInterface(iContextMenuPtr, ref ShellAPI.IID_IContextMenu3, out iContextMenuPtr3);

                    try
                    {
                        iContextMenu2 = (IContextMenu2)Marshal.GetTypedObjectForIUnknown(iContextMenuPtr2, typeof(IContextMenu2));

                        iContextMenu3 = (IContextMenu3)Marshal.GetTypedObjectForIUnknown(iContextMenuPtr3, typeof(IContextMenu3));
                    }
                    catch (Exception) { }

                    uint selected = ShellAPI.TrackPopupMenuEx(
                                        contextMenu,
                                        ShellAPI.TPM.RETURNCMD,
                                        location.X,
                                        location.Y,
                                        this.Handle,
                                        IntPtr.Zero);

                   
                    if (selected >= ShellAPI.CMD_FIRST)
                    {
                        string command = ContextMenuHelper.GetCommandString(iContextMenu, selected - ShellAPI.CMD_FIRST, true);

                        if (command == "Explore")
                        {
                            /*if (!br.FolderView.SelectedNode.IsExpanded)
                                br.FolderView.SelectedNode.Expand();

                            br.FolderView.SelectedNode = br.FolderView.SelectedNode.Nodes[hitTest.Item.Text];*/
                        }
                        else if (command == "rename")
                        {
                            /*hitTest.Item.BeginEdit();*/
                        }
                        else
                        {
                            ContextMenuHelper.InvokeCommand(
                                iContextMenu,
                                selected - ShellAPI.CMD_FIRST,
                                parentDirectory,
                                location);
                        }
                    }
                }
            }
            catch (Exception e) 
            {
#if DEBUG
                MessageBox.Show("", e.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
            }
            finally
            {
                if (iContextMenu != null)
                {
                    Marshal.ReleaseComObject(iContextMenu);
                    iContextMenu = null;
                }

                if (iContextMenu2 != null)
                {
                    Marshal.ReleaseComObject(iContextMenu2);
                    iContextMenu2 = null;
                }

                if (iContextMenu3 != null)
                {
                    Marshal.ReleaseComObject(iContextMenu3);
                    iContextMenu3 = null;
                }

                if (contextMenu != null)
                    ShellAPI.DestroyMenu(contextMenu);

                if (iContextMenuPtr != IntPtr.Zero)
                    Marshal.Release(iContextMenuPtr);

                if (iContextMenuPtr2 != IntPtr.Zero)
                    Marshal.Release(iContextMenuPtr2);

                if (iContextMenuPtr3 != IntPtr.Zero)
                    Marshal.Release(iContextMenuPtr3);
            }
        }

        public static DirectoryInfo GetParentDirectory(FileSystemInfo item)
        {
            if (item is FileInfo)
            {
                FileInfo file = (FileInfo)item;
                return file.Directory;
            }
            else
            {
                DirectoryInfo directory = (DirectoryInfo)item;
                return directory.Parent;
            }
        }

        public static string GetParentDirectoryPath(FileSystemInfo item)
        {
            DirectoryInfo parentDirectory = GetParentDirectory(item);
            if (parentDirectory == null)
            {
                return SpecialFolderPath.MyComputer;
            }
            return parentDirectory.FullName;
        }

        public void DeleteCommand(params FileSystemInfo[] items)
        {
            IntPtr[] pidls = GetPIDLs(items);
            if (pidls.Length > 0)
            {
                string parentDirectory = GetParentDirectoryPath(items[0]);
                IShellFolder parentShellFolder = ShellFolder.GetShellFolder(parentDirectory);
                ContextMenuHelper.InvokeCommand(parentShellFolder, parentDirectory, pidls, "delete", new Point(0, 0));
            }
        }

        public void DefaultCommand(FileInfo file)
        {
            DefaultCommand(file.FullName, file.DirectoryName);
        }

        public void DefaultCommand(string path)
        {
            string parentDirectory = Path.GetDirectoryName(path);
            DefaultCommand(path, parentDirectory);
        }

        private void DefaultCommand(string path, string parentDirectory)
        {
            IntPtr[] pidls = GetPIDLs(path);

            IntPtr icontextMenuPtr = IntPtr.Zero, context2Ptr = IntPtr.Zero, context3Ptr = IntPtr.Zero;
            ContextMenu contextMenu = new ContextMenu();
            IShellFolder parentShellFolder = ShellFolder.GetShellFolder(parentDirectory);

            try
            {
                if (ContextMenuHelper.GetIContextMenu(parentShellFolder, pidls, out icontextMenuPtr, out iContextMenu))
                {
                    iContextMenu.QueryContextMenu(
                        contextMenu.Handle,
                        0,
                        ShellAPI.CMD_FIRST,
                        ShellAPI.CMD_LAST,
                        ShellAPI.CMF.DEFAULTONLY);

                    int defaultCommand = ShellAPI.GetMenuDefaultItem(contextMenu.Handle, false, 0);
                    if (defaultCommand >= ShellAPI.CMD_FIRST)
                    {
                        ContextMenuHelper.InvokeCommand(
                            iContextMenu,
                            (uint)defaultCommand - ShellAPI.CMD_FIRST,
                            parentDirectory,
                            Control.MousePosition);
                    }
                }
            }
            catch (Exception) { }
            finally
            {
                if (iContextMenu != null)
                {
                    Marshal.ReleaseComObject(iContextMenu);
                    iContextMenu = null;
                }

                if (contextMenu.Handle != null)
                    Marshal.FreeCoTaskMem(contextMenu.Handle);

                Marshal.Release(icontextMenuPtr);
            }
        }
    }
}
