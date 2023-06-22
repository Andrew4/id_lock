@ECHO OFF

set my_exe_here=id-test.exe


:exe_scan
IF NOT EXIST "%CD%\%my_exe_here%" GOTO delete_result
DEL /F /Q "%CD%\%my_exe_here%"

:delete_result
IF EXIST "%CD%\%my_exe_here%" GOTO still_exist
GOTO start_compiler
:still_exist
ECHO Some files cannot be deleted under directory:
ECHO %CD%
ECHO Script failed.
GOTO script_quit

:start_compiler
set my_params=/out:%my_exe_here%
set my_params=%my_params% /target:exe
set my_params=%my_params% *.cs

ECHO Starting the Compiler.
set old_path=%path%
set path=%path%;%SystemDrive%\Windows\Microsoft.NET\Framework64\v4.0.30319
csc.exe %my_params%
set path=%old_path%
set old_path=
IF NOT EXIST "%CD%\%my_exe_here%" GOTO error_result
ECHO Done.
GOTO script_quit
:error_result
ECHO For now, it did not work out. Fix those errors and try again.

:script_quit
set my_exe_here=
set my_params=
pause
@ECHO ON

