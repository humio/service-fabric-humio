Param(
    [string]$downloadUrlScript,
    [string]$downloadUrlConfig,
    [string]$downloadLocation = 'Clustermonitor.zip',
    [string]$expandlocation = 'C:\ClusterMonitor\',
    [string]$file = 'C:\ClusterMonitor\ClusterMonitor.exe'
)

# Check if service exists
$service = Get-Service ClusterMonitor  -ErrorAction Ignore

# Clean-up running service if exists
if ($service)
{
    Stop-Service ClusterMonitor
    sc.exe delete ClusterMonitor
    Remove-Item $expandlocation -Recurse -Force
}

# Download binaries and expand content
Write-Host "Downloading binaries"
Invoke-WebRequest -Uri $downloadUrlScript -OutFile $downloadLocation
Expand-Archive -Path $downloadLocation -DestinationPath $expandlocation

# Download config
Invoke-WebRequest -Uri $downloadUrlConfig -OutFile $expandlocation\eventFlowConfig.json

# Create service
Write-Host "Creating service..."
New-Service -Name ClusterMonitor -BinaryPathName $file `
    -Description "EventFlow based monitoring agent for Service Fabric runtime" -StartupType Automatic

# Start service
Write-Host "Starting service..."
Start-Service -Name ClusterMonitor