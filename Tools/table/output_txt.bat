SET WORKSPACE=%~dp0
SET SOURCETABLEDIR=%~dp0/../../Tables/Sources/
SET TABLERESDIR=%~dp0/../../UnityProject/SimSwordsman/Assets/StreamingAssets/config/

cd %WORKSPACE%
%~dp0/convertxlsx.exe -i %SOURCETABLEDIR% -o %TABLERESDIR%

pause