$AzureResourceGroupName = "PDE-RGA-FO-Powertron"
$AzureDeployTemplateFile = "basic\de365powertronrscdeploy.json"
#$storagePrefix = "dixf"
#$OneDriveFolder = "/Projects/00 - In progress/978 - Staples/PIM_RTL/Preise/PRICE_INPUT"

#Connect-AzAccount

#New-AzResourceGroup `
#  -Name $AzureResourceGroupName `
#  -Location "westeurope"

New-AzResourceGroupDeployment `
  -Name de365powertron_dixfpackagecreate `
  -ResourceGroupName $AzureResourceGroupName `
  -TemplateFile $AzureDeployTemplateFile `
  
#New-AzResourceGroupDeployment `
#  -Name de365powertron_dixfpackagecreate_serviceAPI `
#  -ResourceGroupName $AzureResourceGroupName `
#  -TemplateFile $AzureDeployTemplateFileServiceAPI `
  
© 2020 GitHub, Inc.