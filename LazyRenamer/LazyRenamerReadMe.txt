LazyRenamer v1.1

----------------
'    Copyright © Alister Hood, 2011
'
'    This file is part of LazyRenamer.
'    http://code.google.com/p/lazyrenamer
'
'    LazyRenamer is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 2 of the License, or
'    (at your option) any later version.
'
'    LazyRenamer is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with LazyRenamer.  If not, see <http://www.gnu.org/licenses/>.


----------------

LazyRenamer is a tool for renaming or copying a group of associated files.
For example, if you have a bundle of files called:
   Land.shp
   Land.dbf
   Land.shx
   Land.prj
   Land.mwsr
   Land.lbl,
To rename all the files at once simply drag one of them onto the program, type the new name, and press [enter].
To copy all the files instead, press [tab] and then [enter].
As you type the new name LazyRenamer checks for existing files or directories with the same base name, to avoid any conflicts or confusion.

Lazyrenamer can handle files with multiple extensions - useful when you have sets of files that keep growing like this:
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

LazyRenamer will also rename a matching file with no extension, but will not currently rename directories.
LazyRenamer also updates the Layer Name attribute in MapWindow MWSR files.

Microsoft .NET Framework 2.0 or later is required to run this program.


----------------

New developers wanted
I am not really a developer, and this program isn't a priority for me, so I won't be doing a lot of improvements.
Just ask if you want to be added to the project. 
