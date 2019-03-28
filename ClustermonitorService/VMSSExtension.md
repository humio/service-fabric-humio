# Extension Snippet

``` json
"parameters": {
    "clusterMonitor_artifactsLocation": {
        "type": "string",
        "metadata": {
          "description": "The URI where scripts required by this template are located."
        }
      }
}

"variables": {
        "clusterMonitor_scriptFileName": "InstallClusterMonitor.ps1",
        "clusterMonitor_packageDownloadURL": "-downloadUrlScript https://<...>/ClusterMonitor.zip",
        "clusterMonitor_configDownloadURL": "-downloadUrlConfig https://<...>/eventFlowConfig.json",
        "clusterMonitor_scriptParameters": "[concat(variables('clusterMonitor_packageDownloadURL'), ' ', variables('clusterMonitor_configDownloadURL'))]"
}

{
  "name": "[concat('ClusterMonitorExt','_vmNodeType0Name')]",
  "properties": {
    "publisher": "Microsoft.Compute",
    "forceUpdateTag": "1.1",
    "settings": {
      "fileUris": [
          "[concat(parameters('clusterMonitor_artifactsLocation'), variables('clusterMonitor_scriptFileName'))]"
      ]
    },
    "typeHandlerVersion": "1.8",
    "autoUpgradeMinorVersion": true,
    "protectedSettings": {
      "commandToExecute": "[concat('powershell -ExecutionPolicy Unrestricted -File ', variables('clusterMonitor_scriptFileName'), ' ', variables('clusterMonitor_scriptParameters'))]"
    },
    "type": "CustomScriptExtension"
  }
}
```