$login = "NT AUTHORITY\NetworkService"
$psw = ""
$scuritypsw = ConvertTo-SecureString '$psw' -AsPlainText -Force 

$mycreds = New-Object System.Management.Automation.PSCredential($login, $scuritypsw)

sc.exe delete CNCLib.SerialServer

$CNCServerDir = (${env:ProgramFiles(x86)}, ${env:ProgramFiles} -ne $null)[0]
$CNCServerDll = "$CNCServerDir\CNCLib.Serial.Server\CNCLib.Serial.Server.dll"
$CNCServerExe = """dotnet"" ""$CNCServerDll"""

New-Service -Name "CNCLib.SerialServer" -BinaryPathName "$CNCServerExe" -StartupType Automatic -Credential $mycreds 
Start-Service "CNCLib.SerialServer" -Verbose
