**Version 1.2, March 2012 - many fixes and enhancements, including support for Linux**

[[/SupportFiles/LazyRenamer_screenshot_0.PNG]] [[/SupportFiles/LazyRenamer_screenshot_2.PNG]] [[/SupportFiles/LazyRenamer_screenshot_1.PNG]] [[/SupportFiles/LazyRenamer_screenshot_3.PNG]]

[[/SupportFiles/files_screenshot_0.PNG]] [[/SupportFiles/files_screenshot_1.PNG]]

LazyRenamer is a tool for renaming or copying a group of associated files.
For example, if you have a bundle of files called:
```
   Land.shp
   Land.dbf
   Land.shx
   Land.prj
   Land.mwsr
   Land.lbl
```
To rename all the files at once simply drag one of them onto the program, type the new base name, and press `[enter]`.
To copy all the files instead of renaming, press `[tab]` and then `[enter]`.
As you type the new name LazyRenamer checks for existing files or directories with the same base name, to avoid any conflicts or confusion.

LazyRenamer can handle files with multiple extensions - useful when you have sets of files that keep growing like this:
```
   Sails.stc.dwh.01.bak
   Sails.stc.mdb
   Sails.stc.mdb.01.bak
   Sails.stc.mdb_1_1.out
   Sails.stc.mdb_1_1.out.01.bak
   Sails.stc.mdb_1_165.out
   Sails.stc.mdb_1_165.out.01.bak
   Sails.stc.mdb_1_204.out
   Sails.stc.mdb_1_204.out.01.bak
   Sails.stc.mdb_1_214.out
   Sails.stc.mdb_1_216.out
   Sails.stc
   Sails.stc.01.bak
   Sails.stc.dwh
```

LazyRenamer also renames matching directories and matching files with no extension.
LazyRenamer also updates the Layer Name attribute in MapWindow MWSR files.

Either the open source and cross platform Mono runtime OR Microsoft .NET Framework 2.0 or later is required to run this program.

---

# Known Issues - version 1.2 #
- Does not detect name clashes due to files created after you type the new name and before you click "copy" or "rename"

- Does not report if the file was removed before you click "copy" or "rename"

- Does not validate the new name when you paste into it, or use cut or undo via the mouse.

For more details, or new issues check
https://github.com/AlisterH/lazyrenamer/issues

---

# New developers welcome #
I don't have plans for a lot of improvements.  Patches are welcome and ask if you want to be added to the project.