function XmlDocTransform($xml, $xdt)
{
    if (!$xml -or !(Test-Path -path $xml -PathType Leaf)) {
        throw "File not found. $xml";
    }
    if (!$xdt -or !(Test-Path -path $xdt -PathType Leaf)) {
        throw "File not found. $xdt";
    }

    $scriptPath = (Get-Variable MyInvocation -Scope 1).Value.InvocationName | split-path -parent
    Add-Type -LiteralPath "$scriptPath\Microsoft.Web.XmlTransform.dll"

    $xmldoc = New-Object Microsoft.Web.XmlTransform.XmlTransformableDocument;
    $xmldoc.PreserveWhitespace = $true
    $xmldoc.Load($xml);

    $transf = New-Object Microsoft.Web.XmlTransform.XmlTransformation($xdt);
    if ($transf.Apply($xmldoc) -eq $false)
    {
        throw "Transformation failed."
    }
    $xmldoc.Save($xml);
}

function TransformJsonToParams($fileName, $serviceName)
{
    $commandLine = ""
    if (!$fileName -or !(Test-Path -path $fileName -PathType Leaf)) {
        return $commandLine
    }

    $data = Get-Content -Raw -Path $fileName | ConvertFrom-Json | Where {@($_.ServiceName) -eq $serviceName}
    $data = $data.where({$_.ServiceName -eq $serviceName}).CommandLine
    
    foreach($obj_prop in $data.PsObject.Properties)
    {
        $commandLine= $commandLine + $obj_prop.Name + "= "+$obj_prop.Value + " "
    }

    return $commandLine 
}

$Env = (Get-Item env:ENVIRONMENT).value
$ServiceName = (Get-Item env:SERVICE_NAME).value

Write-Host $Env
Write-Host $ServiceName

$EntryPointName="ConsoleApp.exe"
$ConfigFilePath = "$EntryPointName.config"
$EnvironmentBaseConfigFilePath = ".\cfg\app.Base.config"
$EnvironmentConfigFilePath = ".\cfg\app.$Env.config"

$EnvironmentSettingsFilePath = ".\cfg\Services.$Env.json"

Write-Host $EnvironmentConfigFilePath

#config transformation
XmlDocTransform -xml $ConfigFilePath -xdt $EnvironmentBaseConfigFilePath
XmlDocTransform -xml $ConfigFilePath -xdt $EnvironmentConfigFilePath

#parse command line args
$lineArgs = TransformJsonToParams -fileName $EnvironmentSettingsFilePath -serviceName $serviceName

#run app
$file = Get-ChildItem -Path $EntryPointName
& $file $lineArgs
