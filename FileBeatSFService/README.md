# Filebeat as Service Fabric service

This is filebeat configured to run as a Guest Executable in Service Fabric. Filebeat will ship logfiles to Humio's Eleasticsearch endpoint.

The filebeat configuration in this repo is set up to ship all log files from working directories of any service in the cluster, as well as filebeats own logs :-) Refer to the filebeat documentation to change this behavior.

## Install or Update

The following procedure describes how to install or update the Filebeat service.

1. Update the access token in the [filebeat.yml file](./FileBeatSF/ApplicationPackageRoot/FilebeatSvcPkg/Code/filebeat.yml) and possible the `hosts` parameter if you are not using Humio cloud.

1. If your upgrading an existing version, make sure to update the version of the code package and application. This example shows how this is done: https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-application-upgrade-tutorial-powershell#step-2-update-the-visual-objects-sample

1. Deploy the service package to your Service Fabric cluster - https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-deploy-existing-app#deployment