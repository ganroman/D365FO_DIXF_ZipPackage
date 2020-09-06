$AzureResourceGroupName = "retaildevelop01"
$AzureDeployTemplateFile = "de365powertronrscdeploy.json"
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
  