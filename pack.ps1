param (
	[string]$packageName,
	[string]$nugetServer,
	[string]$nugetApiKey,
	[string]$configuration = "Debug"
)

ForEach ($item in (Get-ChildItem -Path "./src" -Directory)) {
	Write-Host "$($item.FullName)\project.json"
	if(Test-Path "$($item.FullName)\project.json") {
		Set-Location $item.FullName
		if(Test-Path "./bin") {
			Remove-Item -Recurse -Force "./bin"
		}
		dnu restore > $null
		dnu build --configuration $configuration > $null
		dnu pack --configuration $configuration > $null
		if($nugetServer) {
			ForEach($file in (Get-ChildItem -Path "./bin/$configuration" -File)) {
				if($file.Extension.Equals(".nupkg") -and -not($file.Name.Contains("symbols")) ) {
					if($file.Name.Contains($packageName) -or [string]::IsNullOrEmpty($packageName) -or $packageName.Equals("all")) {
						nuget push $file.FullName -s $nugetServer $nugetApiKey
					}
				}
			}
		}
		Set-Location ../../
	}
}