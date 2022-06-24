@echo off
setlocal

cd "%~dp0"

For %%a in (
"LibNoiseShaderEditor\bin\Release\net48\*.dll"
"LibNoiseShaderEditor\bin\Release\net48\*.exe"
"LibNoiseShaderEditor\bin\Release\net48\*.config"
) do (
xcopy /s /d "%%~a" dist\
)
